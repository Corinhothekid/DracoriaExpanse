using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.Items;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.Markets
{
    public class VendorStallUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text stallNameText;
        [SerializeField] private Text ownerText;
        [SerializeField] private Text playerGoldText;
        [SerializeField] private Text listingText;
        [SerializeField] private Text selectedText;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button closeButton;

        private MarketManager manager;
        private string currentStallId;
        private int selectedListingIndex;
        private bool buttonsBound;

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            BindButtons();
        }

        private void Start()
        {
            if (manager == null && MarketManager.Instance != null)
            {
                MarketManager.Instance.RegisterUI(this);
            }
        }

        public void Configure(GameObject panelObject, Text stallName, Text owner, Text playerGold, Text listing, Text selected, Button previous, Button next, Button buy, Button close)
        {
            panel = panelObject;
            stallNameText = stallName;
            ownerText = owner;
            playerGoldText = playerGold;
            listingText = listing;
            selectedText = selected;
            previousButton = previous;
            nextButton = next;
            buyButton = buy;
            closeButton = close;
            BindButtons();
        }

        public void RegisterManager(MarketManager marketManager)
        {
            manager = marketManager;
            Refresh();
        }

        public void Open(string stallId)
        {
            currentStallId = MarketIds.Normalize(stallId);
            selectedListingIndex = 0;
            if (panel != null)
            {
                panel.SetActive(true);
            }

            Refresh();
        }

        public void Close()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        public void Refresh()
        {
            var stall = manager != null ? manager.GetStall(currentStallId) : null;
            if (stall == null)
            {
                SetText(stallNameText, "Vendor Stall");
                SetText(ownerText, "Owner: Unknown");
                SetText(playerGoldText, string.Empty);
                SetText(listingText, "No stall selected.");
                SetText(selectedText, string.Empty);
                SetButtonState(buyButton, false);
                return;
            }

            var availableListings = GetAvailableListings(stall);
            if (availableListings.Count > 0)
            {
                selectedListingIndex = Mathf.Clamp(selectedListingIndex, 0, availableListings.Count - 1);
            }
            else
            {
                selectedListingIndex = 0;
            }

            SetText(stallNameText, stall.stallName);
            SetText(ownerText, $"Owner: {stall.ownerName} | {stall.locationName}");
            SetText(playerGoldText, $"Your Gold: {GameManager.Instance?.CharacterData?.gold ?? 0}");
            SetText(listingText, BuildListingText(availableListings));
            SetText(selectedText, BuildSelectedText(availableListings));
            SetButtonState(buyButton, availableListings.Count > 0);
        }

        private void SelectPrevious()
        {
            var stall = manager != null ? manager.GetStall(currentStallId) : null;
            var listings = GetAvailableListings(stall);
            if (listings.Count == 0)
            {
                return;
            }

            selectedListingIndex = (selectedListingIndex - 1 + listings.Count) % listings.Count;
            Refresh();
        }

        private void SelectNext()
        {
            var stall = manager != null ? manager.GetStall(currentStallId) : null;
            var listings = GetAvailableListings(stall);
            if (listings.Count == 0)
            {
                return;
            }

            selectedListingIndex = (selectedListingIndex + 1) % listings.Count;
            Refresh();
        }

        private void BuySelected()
        {
            var stall = manager != null ? manager.GetStall(currentStallId) : null;
            var listings = GetAvailableListings(stall);
            if (stall == null || listings.Count == 0)
            {
                return;
            }

            selectedListingIndex = Mathf.Clamp(selectedListingIndex, 0, listings.Count - 1);
            manager.BuyItem(stall.stallId, listings[selectedListingIndex].listingId, 1);
        }

        private static List<MarketListingData> GetAvailableListings(VendorStallData stall)
        {
            if (stall?.listedItems == null)
            {
                return new List<MarketListingData>();
            }

            return stall.listedItems.Where(listing => listing != null && listing.isAvailable && listing.quantity > 0).ToList();
        }

        private string BuildListingText(IReadOnlyList<MarketListingData> listings)
        {
            if (listings.Count == 0)
            {
                return "Sold out.";
            }

            var lines = new List<string>();
            for (var i = 0; i < listings.Count; i++)
            {
                var marker = i == selectedListingIndex ? "> " : "  ";
                var listing = listings[i];
                lines.Add($"{marker}{ItemDatabase.GetDisplayName(listing.itemId)} x{listing.quantity} - {listing.pricePerItem}g each");
            }

            return string.Join("\n", lines);
        }

        private string BuildSelectedText(IReadOnlyList<MarketListingData> listings)
        {
            if (listings.Count == 0)
            {
                return "No items available.";
            }

            var listing = listings[Mathf.Clamp(selectedListingIndex, 0, listings.Count - 1)];
            return $"Selected: {ItemDatabase.GetDisplayName(listing.itemId)} | Price: {listing.pricePerItem}g | Seller: {listing.sellerName}";
        }

        private void BindButtons()
        {
            if (buttonsBound || previousButton == null || nextButton == null || buyButton == null || closeButton == null)
            {
                return;
            }

            previousButton.onClick.AddListener(SelectPrevious);
            nextButton.onClick.AddListener(SelectNext);
            buyButton.onClick.AddListener(BuySelected);
            closeButton.onClick.AddListener(Close);
            buttonsBound = true;
        }

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value;
            }
        }

        private static void SetButtonState(Button button, bool enabled)
        {
            if (button != null)
            {
                button.interactable = enabled;
            }
        }
    }
}
