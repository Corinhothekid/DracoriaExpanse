using System;
using WildsOfDracoria.Items;

namespace WildsOfDracoria.Markets
{
    [Serializable]
    public class MarketListingData
    {
        public string listingId;
        public string itemId;
        public int quantity;
        public int pricePerItem;
        public string sellerName;
        public string listingTime;
        public bool isAvailable = true;

        public MarketListingData()
        {
        }

        public MarketListingData(string listingId, string itemId, int quantity, int pricePerItem, string sellerName)
        {
            this.listingId = NormalizeListingId(listingId);
            this.itemId = ItemIds.Normalize(itemId);
            this.quantity = Math.Max(0, quantity);
            this.pricePerItem = Math.Max(0, pricePerItem);
            this.sellerName = string.IsNullOrWhiteSpace(sellerName) ? "Unknown Seller" : sellerName.Trim();
            listingTime = DateTime.UtcNow.ToString("o");
            isAvailable = this.quantity > 0;
        }

        public MarketListingData Clone()
        {
            return new MarketListingData
            {
                listingId = listingId,
                itemId = itemId,
                quantity = quantity,
                pricePerItem = pricePerItem,
                sellerName = sellerName,
                listingTime = listingTime,
                isAvailable = isAvailable
            };
        }

        public void Normalize()
        {
            listingId = NormalizeListingId(listingId);
            itemId = ItemIds.Normalize(itemId);
            quantity = Math.Max(0, quantity);
            pricePerItem = Math.Max(0, pricePerItem);
            sellerName = string.IsNullOrWhiteSpace(sellerName) ? "Unknown Seller" : sellerName.Trim();
            if (string.IsNullOrWhiteSpace(listingTime))
            {
                listingTime = DateTime.UtcNow.ToString("o");
            }

            isAvailable = isAvailable && quantity > 0;
        }

        public static string NormalizeListingId(string listingId)
        {
            return string.IsNullOrWhiteSpace(listingId) ? Guid.NewGuid().ToString("N") : listingId.Trim().ToLowerInvariant().Replace(" ", "_");
        }
    }
}
