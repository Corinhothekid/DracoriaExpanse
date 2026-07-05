using System;
using System.Collections.Generic;

namespace WildsOfDracoria.Markets
{
    [Serializable]
    public class VendorStallData
    {
        public string stallId;
        public string stallName;
        public string ownerName;
        public string locationName;
        public StallType stallType;
        public List<MarketListingData> listedItems = new List<MarketListingData>();
        public int goldBalance;
        public bool isPlayerOwned;
        public int reputation;

        public VendorStallData()
        {
        }

        public VendorStallData(string stallId, string stallName, string ownerName, string locationName, StallType stallType, int goldBalance, bool isPlayerOwned, int reputation)
        {
            this.stallId = MarketIds.Normalize(stallId);
            this.stallName = string.IsNullOrWhiteSpace(stallName) ? "Vendor Stall" : stallName.Trim();
            this.ownerName = string.IsNullOrWhiteSpace(ownerName) ? "Local Vendor" : ownerName.Trim();
            this.locationName = string.IsNullOrWhiteSpace(locationName) ? "Ironhaven" : locationName.Trim();
            this.stallType = stallType;
            this.goldBalance = Math.Max(0, goldBalance);
            this.isPlayerOwned = isPlayerOwned;
            this.reputation = Math.Max(0, reputation);
        }

        public VendorStallData Clone()
        {
            var clone = new VendorStallData
            {
                stallId = stallId,
                stallName = stallName,
                ownerName = ownerName,
                locationName = locationName,
                stallType = stallType,
                goldBalance = goldBalance,
                isPlayerOwned = isPlayerOwned,
                reputation = reputation,
                listedItems = new List<MarketListingData>()
            };

            if (listedItems != null)
            {
                foreach (var listing in listedItems)
                {
                    if (listing != null)
                    {
                        clone.listedItems.Add(listing.Clone());
                    }
                }
            }

            return clone;
        }

        public void ApplyDefinition(VendorStallData definition)
        {
            if (definition == null)
            {
                Normalize();
                return;
            }

            stallId = MarketIds.Normalize(definition.stallId);
            stallName = definition.stallName;
            ownerName = definition.ownerName;
            locationName = definition.locationName;
            stallType = definition.stallType;
            isPlayerOwned = definition.isPlayerOwned;
            Normalize();
        }

        public void Normalize()
        {
            stallId = MarketIds.Normalize(stallId);
            stallName = string.IsNullOrWhiteSpace(stallName) ? "Vendor Stall" : stallName.Trim();
            ownerName = string.IsNullOrWhiteSpace(ownerName) ? "Local Vendor" : ownerName.Trim();
            locationName = string.IsNullOrWhiteSpace(locationName) ? "Ironhaven" : locationName.Trim();
            goldBalance = Math.Max(0, goldBalance);
            reputation = Math.Max(0, reputation);

            if (listedItems == null)
            {
                listedItems = new List<MarketListingData>();
                return;
            }

            listedItems.RemoveAll(listing => listing == null);
            foreach (var listing in listedItems)
            {
                listing.Normalize();
            }
        }
    }
}
