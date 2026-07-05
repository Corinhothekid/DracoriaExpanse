namespace WildsOfDracoria.Markets
{
    public static class MarketIds
    {
        public const string IronhavenFishStall = "ironhaven_fish_stall";
        public const string TorensForgeStall = "torens_forge_stall";
        public const string MarasFoodStall = "maras_food_stall";

        public static string Normalize(string stallIdOrName)
        {
            if (string.IsNullOrWhiteSpace(stallIdOrName))
            {
                return string.Empty;
            }

            switch (stallIdOrName.Trim().ToLowerInvariant())
            {
                case "ironhaven fish stall":
                case IronhavenFishStall: return IronhavenFishStall;
                case "toren's forge stall":
                case "torens forge stall":
                case TorensForgeStall: return TorensForgeStall;
                case "mara's food stall":
                case "maras food stall":
                case MarasFoodStall: return MarasFoodStall;
                default: return stallIdOrName.Trim().ToLowerInvariant().Replace(" ", "_").Replace("'", string.Empty);
            }
        }
    }
}
