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
                case Human:
                case "human": return Human;
                case Elf:
                case "elf": return Elf;
                case Orc:
                case "orc": return Orc;
                case Goblin:
                case "goblin": return Goblin;
                case Dragonborn:
                case "dragonborn": return Dragonborn;
                default: return raceIdOrName.Trim().ToLowerInvariant().Replace(" ", "_");
            }
        }
    }
}