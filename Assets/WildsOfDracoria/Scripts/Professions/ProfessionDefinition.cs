namespace WildsOfDracoria.Professions
{
    public class ProfessionDefinition
    {
        public string professionId;
        public string displayName;
        public string description;
        public bool startsUnlocked;

        public ProfessionDefinition(string professionId, string displayName, string description, bool startsUnlocked)
        {
            this.professionId = ProfessionIds.Normalize(professionId);
            this.displayName = displayName;
            this.description = description;
            this.startsUnlocked = startsUnlocked;
        }
    }
}
