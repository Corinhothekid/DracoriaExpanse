using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WildsOfDracoria.Data;
using WildsOfDracoria.Items;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI.Mobile;

namespace WildsOfDracoria.Markets
{
    public class MarketManager : MonoBehaviour
    {
        public static MarketManager Instance { get; private set; }

        public event Action<VendorStallData> StallOpened;
        public event Action<VendorStallData, MarketListingData, int> ListingPurchased;
        public event Action<VendorStallData, MarketListingData> ListingRemoved;

        [SerializeField] private VendorStallUI vendorStallUI;

        public IReadOnlyList<VendorStallData> Stalls
        {
            get
            {
                EnsureMarketData();
                return GetSavedStalls();
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            EnsureMarketData();
            if (vendorStallUI == null)
            {
                vendorStallUI = UnityEngine.Object.FindAnyObjectByType<VendorStallUI>(FindObjectsInactive.Include);
            }

            vendorStallUI?.RegisterManager(this);
        }

        public void RegisterUI(VendorStallUI ui)
        {
            vendorStallUI = ui;
            vendorStallUI?.RegisterManager(this);
        }

        public void OpenStall(string stallId)
        {
            EnsureMarketData();
            var stall = GetStall(stallId);
            if (stall == null)
            {
                Notify("That stall is not registered yet.");
                return;
            }

            if (vendorStallUI == null)
            {
                vendorStallUI = UnityEngine.Object.FindAnyObjectByType<VendorStallUI>(FindObjectsInactive.Include) ?? VendorStallUIFactory.Create();
                vendorStallUI.RegisterManager(this);
            }

            StallOpened?.Invoke(stall);
            vendorStallUI.Open(stall.stallId);
        }

        public VendorStallData GetStall(string stallId)
        {
            var normalizedId = MarketIds.Normalize(stallId);
            return GetSavedStalls().FirstOrDefault(stall => stall.stallId == normalizedId);
        }

        public MarketListingData GetListing(string stallId, string listingId)
        {
            var stall = GetStall(stallId);
            if (stall?.listedItems == null)
            {
                return null;
            }

            var normalizedListingId = MarketListingData.NormalizeListingId(listingId);
            return stall.listedItems.FirstOrDefault(listing => listing.listingId == normalizedListingId);
        }

        public bool BuyItem(string stallId, string listingId, int quantity = 1)
        {
            EnsureMarketData();
            var data = GetCharacterData();
            if (data == null)
            {
                Notify("Character data is not ready.");
                return false;
            }

            var stall = GetStall(stallId);
            var listing = GetListing(stallId, listingId);
            if (stall == null || listing == null)
            {
                Notify("That market listing could not be found.");
                return false;
            }

            listing.Normalize();
            quantity = Mathf.Max(1, quantity);
            if (!listing.isAvailable || listing.quantity < quantity)
            {
                Notify("That item is sold out.");
                return false;
            }

            if (!ItemDatabase.Exists(listing.itemId))
            {
                Notify("That item is not in the item database yet.");
                return false;
            }

            var totalPrice = listing.pricePerItem * quantity;
            if (data.gold < totalPrice)
            {
                Notify($"You need {totalPrice} gold to buy that.");
                return false;
            }

            data.gold -= totalPrice;
            data.AddInventoryItem(listing.itemId, quantity);
            stall.goldBalance += totalPrice;
            listing.quantity -= quantity;
            if (listing.quantity <= 0)
            {
                listing.quantity = 0;
                listing.isAvailable = false;
            }

            ListingPurchased?.Invoke(stall, listing, quantity);
            Notify($"Bought {ItemDatabase.GetDisplayName(listing.itemId)} x{quantity} for {totalPrice} gold.");
            GameManager.Instance?.RefreshPlayerUI();
            vendorStallUI?.Refresh();
            return true;
        }

        public bool RemoveListing(string stallId, string listingId)
        {
            var stall = GetStall(stallId);
            var listing = GetListing(stallId, listingId);
            if (stall == null || listing == null)
            {
                return false;
            }

            listing.isAvailable = false;
            listing.quantity = 0;
            ListingRemoved?.Invoke(stall, listing);
            vendorStallUI?.Refresh();
            return true;
        }

        public VendorStallData CreatePlayerOwnedStall(string stallName = "Player Stall", string ownerName = "Player", string locationName = "Ironhaven Market")
        {
            EnsureMarketData();
            var normalizedOwner = string.IsNullOrWhiteSpace(ownerName) ? "Player" : ownerName.Trim();
            var stallId = $"player_{normalizedOwner.ToLowerInvariant().Replace(" ", "_")}_stall";
            var existing = GetStall(stallId);
            if (existing != null)
            {
                return existing;
            }

            var stall = new VendorStallData(stallId, stallName, normalizedOwner, locationName, StallType.GeneralGoods, 0, true, 0);
            GetSavedStalls().Add(stall);
            Notify($"Created local player stall stub: {stall.stallName}");
            return stall;
        }

        public bool AddListingFromPlayerInventory(string stallId, string itemId, int quantity, int pricePerItem)
        {
            // Stub for the future player-shop loop. The data path exists, but player selling needs confirmation UI and economy rules first.
            Notify("Player stall selling is stubbed for a later market pass.");
            return false;
        }

        public void EnsureMarketData()
        {
            var data = GetCharacterData();
            if (data == null)
            {
                return;
            }

            data.EnsureMarketStalls();
            foreach (var definition in MarketRegistry.All)
            {
                var saved = data.marketStalls.FirstOrDefault(stall => stall.stallId == definition.stallId);
                if (saved == null)
                {
                    data.marketStalls.Add(definition.Clone());
                    continue;
                }

                saved.ApplyDefinition(definition);
                if (saved.listedItems == null || saved.listedItems.Count == 0)
                {
                    saved.listedItems = definition.Clone().listedItems;
                }
            }
        }

        private List<VendorStallData> GetSavedStalls()
        {
            var data = GetCharacterData();
            if (data == null)
            {
                return new List<VendorStallData>();
            }

            data.EnsureMarketStalls();
            return data.marketStalls;
        }

        private CharacterData GetCharacterData()
        {
            return GameManager.Instance != null ? GameManager.Instance.CharacterData : null;
        }

        private static void Notify(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            NotificationPopupUI.Instance?.Show(message);
            GameManager.Instance?.DialogueUI?.ShowLine(message);
        }
    }
}
