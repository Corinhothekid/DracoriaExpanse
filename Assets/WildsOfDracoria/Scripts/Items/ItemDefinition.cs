using System;

namespace WildsOfDracoria.Items
{
    [Serializable]
    public class ItemDefinition
    {
        public string itemId;
        public string displayName;
        public string description;
        public ItemType itemType;
        public ItemRarity rarity;
        public int stackLimit;
        public int baseValue;
        public float weight;
        public string iconPlaceholderRef;
        public bool isTradable;
        public bool isConsumable;
        public bool isCraftingMaterial;

        public ItemDefinition(
            string itemId,
            string displayName,
            string description,
            ItemType itemType,
            ItemRarity rarity,
            int stackLimit,
            int baseValue,
            float weight,
            string iconPlaceholderRef,
            bool isTradable,
            bool isConsumable,
            bool isCraftingMaterial)
        {
            this.itemId = ItemIds.Normalize(itemId);
            this.displayName = displayName;
            this.description = description;
            this.itemType = itemType;
            this.rarity = rarity;
            this.stackLimit = stackLimit;
            this.baseValue = baseValue;
            this.weight = weight;
            this.iconPlaceholderRef = iconPlaceholderRef;
            this.isTradable = isTradable;
            this.isConsumable = isConsumable;
            this.isCraftingMaterial = isCraftingMaterial;
        }
    }
}
