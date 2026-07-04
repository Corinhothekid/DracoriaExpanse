using System;
using UnityEngine.Serialization;
using WildsOfDracoria.Items;

namespace WildsOfDracoria.Data
{
    [Serializable]
    public class InventoryItem
    {
        [FormerlySerializedAs("itemName")]
        public string itemId;
        public int quantity;

        public InventoryItem()
        {
        }

        public InventoryItem(string itemId, int quantity = 1)
        {
            this.itemId = ItemIds.Normalize(itemId);
            this.quantity = quantity;
        }

        public void Normalize()
        {
            itemId = ItemIds.Normalize(itemId);
        }
    }
}
