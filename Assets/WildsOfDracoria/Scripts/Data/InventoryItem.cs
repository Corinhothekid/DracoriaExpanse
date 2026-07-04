using System;

namespace WildsOfDracoria.Data
{
    [Serializable]
    public class InventoryItem
    {
        public string itemName;
        public int quantity;

        public InventoryItem(string itemName, int quantity = 1)
        {
            this.itemName = itemName;
            this.quantity = quantity;
        }
    }
}
