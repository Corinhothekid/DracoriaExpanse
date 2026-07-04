using System.Collections.Generic;
using System.Linq;

namespace WildsOfDracoria.Items
{
    public static class ItemDatabase
    {
        private static readonly Dictionary<string, ItemDefinition> Items = new Dictionary<string, ItemDefinition>();

        static ItemDatabase()
        {
            RegisterDefaults();
        }

        public static IReadOnlyCollection<ItemDefinition> AllItems => Items.Values;

        public static void Register(ItemDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.itemId))
            {
                return;
            }

            Items[ItemIds.Normalize(definition.itemId)] = definition;
        }

        public static ItemDefinition Get(string itemId)
        {
            var normalizedId = ItemIds.Normalize(itemId);
            return Items.TryGetValue(normalizedId, out var definition) ? definition : null;
        }

        public static bool Exists(string itemId)
        {
            return Get(itemId) != null;
        }

        public static string GetDisplayName(string itemId)
        {
            var definition = Get(itemId);
            return definition != null ? definition.displayName : itemId;
        }

        public static int GetStackLimit(string itemId)
        {
            var definition = Get(itemId);
            return definition != null ? definition.stackLimit : 99;
        }

        public static IEnumerable<ItemDefinition> GetByType(ItemType itemType)
        {
            return Items.Values.Where(item => item.itemType == itemType);
        }

        private static void RegisterDefaults()
        {
            Register(new ItemDefinition(ItemIds.SmallSilverfin, "Small Silverfin", "A small bright fish common near Ironhaven docks.", ItemType.Fish, ItemRarity.Common, 99, 3, 0.2f, "icon_fish_common", true, true, true));
            Register(new ItemDefinition(ItemIds.RiverTrout, "River Trout", "A sturdy freshwater fish used in simple meals.", ItemType.Fish, ItemRarity.Common, 99, 5, 0.35f, "icon_fish_trout", true, true, true));
            Register(new ItemDefinition(ItemIds.OldBoot, "Old Boot", "Waterlogged footwear. Not valuable, but memorable.", ItemType.Misc, ItemRarity.Common, 10, 1, 0.6f, "icon_misc_boot", true, false, false));
            Register(new ItemDefinition(ItemIds.RareGoldenCarp, "Rare Golden Carp", "A shimmering carp prized by collectors and cooks.", ItemType.Fish, ItemRarity.Rare, 10, 45, 0.5f, "icon_fish_rare", true, true, true));

            Register(new ItemDefinition(ItemIds.WolfPelt, "Wolf Pelt", "A rough pelt from a Forest Wolf.", ItemType.MonsterDrop, ItemRarity.Common, 50, 8, 0.8f, "icon_drop_pelt", true, false, true));
            Register(new ItemDefinition(ItemIds.RawMeat, "Raw Meat", "Fresh meat suitable for cooking.", ItemType.Food, ItemRarity.Common, 50, 4, 0.4f, "icon_food_raw_meat", true, true, true));
            Register(new ItemDefinition(ItemIds.SmallFang, "Small Fang", "A small sharp fang from a wild beast.", ItemType.MonsterDrop, ItemRarity.Uncommon, 50, 12, 0.1f, "icon_drop_fang", true, false, true));

            Register(new ItemDefinition(ItemIds.TrainingSword, "Training Sword", "A simple starter blade for learning basic swordsmanship.", ItemType.Weapon, ItemRarity.Common, 1, 15, 2.5f, "icon_weapon_sword", true, false, false));
            Register(new ItemDefinition(ItemIds.RustyAxe, "Rusty Axe", "A worn axe that still hits hard enough for practice.", ItemType.Weapon, ItemRarity.Common, 1, 12, 3.2f, "icon_weapon_axe", true, false, false));
            Register(new ItemDefinition(ItemIds.WoodenBow, "Wooden Bow", "A basic bow placeholder for future ranged combat.", ItemType.Weapon, ItemRarity.Common, 1, 14, 1.8f, "icon_weapon_bow", true, false, false));

            Register(new ItemDefinition(ItemIds.CopperOre, "Copper Ore", "A starter ore for future mining and smithing.", ItemType.Ore, ItemRarity.Common, 99, 4, 0.7f, "icon_ore_copper", true, false, true));
            Register(new ItemDefinition(ItemIds.IronOre, "Iron Ore", "A useful ore for future blacksmithing.", ItemType.Ore, ItemRarity.Common, 99, 7, 0.9f, "icon_ore_iron", true, false, true));
            Register(new ItemDefinition(ItemIds.OakLog, "Oak Log", "A sturdy wood resource for future crafting and building.", ItemType.Wood, ItemRarity.Common, 99, 5, 1.0f, "icon_wood_oak", true, false, true));
            Register(new ItemDefinition(ItemIds.Wheat, "Wheat", "A starter crop for future farming and cooking recipes.", ItemType.Food, ItemRarity.Common, 99, 2, 0.1f, "icon_crop_wheat", true, false, true));
            Register(new ItemDefinition(ItemIds.SimpleBread, "Simple Bread", "Plain travel bread for future cooking and survival loops.", ItemType.Food, ItemRarity.Common, 20, 2, 0.2f, "icon_food_bread", true, true, false));
            Register(new ItemDefinition(ItemIds.BasicFishingRod, "Basic Fishing Rod", "A beginner tool for future equipment-based fishing.", ItemType.Tool, ItemRarity.Common, 1, 10, 1.2f, "icon_tool_fishing_rod", true, false, false));
            Register(new ItemDefinition(ItemIds.BeginnerPickaxe, "Beginner Pickaxe", "A starter mining tool placeholder.", ItemType.Tool, ItemRarity.Common, 1, 10, 2.0f, "icon_tool_pickaxe", true, false, false));
            Register(new ItemDefinition(ItemIds.BeginnerHammer, "Beginner Hammer", "A starter blacksmithing tool placeholder.", ItemType.Tool, ItemRarity.Common, 1, 10, 1.8f, "icon_tool_hammer", true, false, false));
        }
    }
}