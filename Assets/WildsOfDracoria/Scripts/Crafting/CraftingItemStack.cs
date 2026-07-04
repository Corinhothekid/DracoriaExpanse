using System;
using WildsOfDracoria.Items;

namespace WildsOfDracoria.Crafting
{
    [Serializable]
    public class CraftingItemStack
    {
        public string itemId;
        public int quantity;

        public CraftingItemStack()
        {
        }

        public CraftingItemStack(string itemId, int quantity)
        {
            this.itemId = ItemIds.Normalize(itemId);
            this.quantity = quantity;
        }

        public void Normalize()
        {
            itemId = ItemIds.Normalize(itemId);
        }

        public string GetDisplayText()
        {
            Normalize();
            return $"{ItemDatabase.GetDisplayName(itemId)} x{quantity}";
        }
    }
}