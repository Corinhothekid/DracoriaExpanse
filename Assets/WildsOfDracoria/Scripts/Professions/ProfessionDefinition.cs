using System.Collections.Generic;

namespace WildsOfDracoria.Professions
{
    public class ProfessionDefinition
    {
        public string professionName;
        public bool startsUnlocked;
        public string family;
        public string description;
        public List<string> defaultUnlocks = new List<string>();
        public List<string> defaultBonuses = new List<string>();

        public ProfessionDefinition(string professionName, bool startsUnlocked, string family, string description)
        {
            this.professionName = professionName;
            this.startsUnlocked = startsUnlocked;
            this.family = family;
            this.description = description;
        }
    }
}
