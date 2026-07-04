namespace WildsOfDracoria.CharacterCreation
{
    public static class RaceIds
    {
        public const string Human = "human";
        public const string Elf = "elf";
        public const string Orc = "orc";
        public const string Goblin = "goblin";
        public const string Dragonborn = "dragonborn";

        public static string Normalize(string raceIdOrName)
        {
            if (string.IsNullOrWhiteSpace(raceIdOrName))
            {
                return Human;
            }

            switch (raceIdOrName.Trim().ToLowerInvariant())
            {
                case "human": return Human;
                case "elf": return Elf;
                case "orc": return Orc;
                case "goblin": return Goblin;
                case "dragonborn": return Dragonborn;
                default: return raceIdOrName.Trim().ToLowerInvariant().Replace(" ", "_");
            }
        }
    }
}