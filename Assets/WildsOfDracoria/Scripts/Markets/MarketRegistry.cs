using System.Collections.Generic;
using WildsOfDracoria.Items;

namespace WildsOfDracoria.Markets
{
    public static class MarketRegistry
    {
        private static readonly List<VendorStallData> definitions = new List<VendorStallData>
        {
            BuildFishStall(),
            BuildForgeStall(),
            BuildFoodStall()
        };

        public static IReadOnlyList<VendorStallData> All => definitions;

        public static VendorStallData GetDefinition(string stallId)
        {
            var normalizedId = MarketIds.Normalize(stallId);
            return definitions.Find(stall => stall.stallId == normalizedId);
        }

        private static VendorStallData BuildFishStall()
        {
            var stall = new VendorStallData(MarketIds.IronhavenFishStall, "Ironhaven Fish Stall", "Fisher Mara", "Ironhaven Market", StallType.FishMarket, 45, false, 1);
            stall.listedItems.Add(new MarketListingData("fish_small_silverfin", ItemIds.SmallSilverfin, 12, 3, stall.ownerName));
            stall.listedItems.Add(new MarketListingData("fish_river_trout", ItemIds.RiverTrout, 8, 5, stall.ownerName));
            return stall;
        }

        private static VendorStallData BuildForgeStall()
        {
            var stall = new VendorStallData(MarketIds.TorensForgeStall, "Toren's Forge Stall", "Blacksmith Toren", "Ironhaven Market", StallType.Forge, 90, false, 2);
            stall.listedItems.Add(new MarketListingData("forge_training_sword", ItemIds.TrainingSword, 3, 20, stall.ownerName));
            stall.listedItems.Add(new MarketListingData("forge_beginner_pickaxe", ItemIds.BeginnerPickaxe, 4, 12, stall.ownerName));
            stall.listedItems.Add(new MarketListingData("forge_beginner_hammer", ItemIds.BeginnerHammer, 4, 12, stall.ownerName));
            return stall;
        }

        private static VendorStallData BuildFoodStall()
        {
            var stall = new VendorStallData(MarketIds.MarasFoodStall, "Mara's Food Stall", "Mara the Cook", "Ironhaven Market", StallType.Food, 60, false, 2);
            stall.listedItems.Add(new MarketListingData("food_simple_bread", ItemIds.SimpleBread, 10, 4, stall.ownerName));
            stall.listedItems.Add(new MarketListingData("food_raw_meat", ItemIds.RawMeat, 10, 3, stall.ownerName));
            return stall;
        }
    }
}
