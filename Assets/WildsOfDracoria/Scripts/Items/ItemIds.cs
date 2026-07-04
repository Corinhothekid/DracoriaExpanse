namespace WildsOfDracoria.Items
{
    public static class ItemIds
    {
        public const string SmallSilverfin = "small_silverfin";
        public const string RiverTrout = "river_trout";
        public const string OldBoot = "old_boot";
        public const string RareGoldenCarp = "rare_golden_carp";
        public const string WolfPelt = "wolf_pelt";
        public const string RawMeat = "raw_meat";
        public const string SmallFang = "small_fang";
        public const string TrainingSword = "training_sword";
        public const string RustyAxe = "rusty_axe";
        public const string WoodenBow = "wooden_bow";
        public const string CopperOre = "copper_ore";
        public const string IronOre = "iron_ore";
        public const string OakLog = "oak_log";
        public const string Wheat = "wheat";
        public const string SimpleBread = "simple_bread";
        public const string BasicFishingRod = "basic_fishing_rod";
        public const string BeginnerPickaxe = "beginner_pickaxe";
        public const string BeginnerHammer = "beginner_hammer";

        public static string Normalize(string itemIdOrName)
        {
            if (string.IsNullOrWhiteSpace(itemIdOrName))
            {
                return string.Empty;
            }

            switch (itemIdOrName.Trim().ToLowerInvariant())
            {
                case "small silverfin":
                case SmallSilverfin: return SmallSilverfin;
                case "river trout":
                case RiverTrout: return RiverTrout;
                case "old boot":
                case OldBoot: return OldBoot;
                case "rare golden carp":
                case RareGoldenCarp: return RareGoldenCarp;
                case "wolf pelt":
                case WolfPelt: return WolfPelt;
                case "raw meat":
                case RawMeat: return RawMeat;
                case "small fang":
                case SmallFang: return SmallFang;
                case "training sword":
                case TrainingSword: return TrainingSword;
                case "rusty axe":
                case RustyAxe: return RustyAxe;
                case "wooden bow":
                case WoodenBow: return WoodenBow;
                case "copper ore":
                case CopperOre: return CopperOre;
                case "iron ore":
                case IronOre: return IronOre;
                case "oak log":
                case OakLog: return OakLog;
                case "wheat":
                case Wheat: return Wheat;
                case "simple bread":
                case SimpleBread: return SimpleBread;
                case "basic fishing rod":
                case BasicFishingRod: return BasicFishingRod;
                case "beginner pickaxe":
                case BeginnerPickaxe: return BeginnerPickaxe;
                case "beginner hammer":
                case BeginnerHammer: return BeginnerHammer;
                default: return itemIdOrName.Trim().ToLowerInvariant().Replace(" ", "_");
            }
        }
    }
}