namespace WildsOfDracoria.Professions
{
    public static class ProfessionIds
    {
        public const string Fishing = "fishing";
        public const string Mining = "mining";
        public const string Blacksmithing = "blacksmithing";
        public const string Cooking = "cooking";
        public const string Sailing = "sailing";
        public const string Merchanting = "merchanting";
        public const string Logging = "logging";
        public const string Farming = "farming";
        public const string Hunting = "hunting";
        public const string Navigation = "navigation";

        public static string Normalize(string professionId)
        {
            if (string.IsNullOrWhiteSpace(professionId))
            {
                return string.Empty;
            }

            switch (professionId.Trim().ToLowerInvariant())
            {
                case "fishing": return Fishing;
                case "mining": return Mining;
                case "blacksmithing": return Blacksmithing;
                case "cooking": return Cooking;
                case "sailing": return Sailing;
                case "merchant":
                case "merchanting": return Merchanting;
                case "logging": return Logging;
                case "farming": return Farming;
                case "hunter":
                case "hunting": return Hunting;
                case "navigator":
                case "navigation": return Navigation;
                default: return professionId.Trim().ToLowerInvariant().Replace(" ", "_");
            }
        }
    }
}
