using System;
using System.Collections.Generic;

namespace WildsOfDracoria.Professions
{
    [Serializable]
    public class ProfessionData
    {
        public string professionName;
        public int level;
        public int currentXP;
        public int xpRequired;
        public string masteryRank;
        public int reputation;
        public bool isUnlocked;
        public List<string> unlocks = new List<string>();
        public List<string> activeBonuses = new List<string>();
        public List<string> professionJournal = new List<string>();

        public ProfessionData(string professionName, bool isUnlocked = false)
        {
            this.professionName = professionName;
            this.level = 1;
            this.currentXP = 0;
            this.xpRequired = 100;
            this.masteryRank = "Novice";
            this.reputation = 0;
            this.isUnlocked = isUnlocked;
        }

        public bool GainXP(int amount)
        {
            if (!isUnlocked || amount <= 0)
            {
                return false;
            }

            var leveledUp = false;
            currentXP += amount;

            while (currentXP >= xpRequired)
            {
                currentXP -= xpRequired;
                level++;
                xpRequired = CalculateXPRequired(level);
                masteryRank = CalculateMasteryRank(level);
                leveledUp = true;
            }

            return leveledUp;
        }

        public void AddJournalEntry(string entry)
        {
            if (!string.IsNullOrWhiteSpace(entry))
            {
                professionJournal.Add(entry);
            }
        }

        public static int CalculateXPRequired(int level)
        {
            return 100 + ((level - 1) * 50);
        }

        public static string CalculateMasteryRank(int level)
        {
            if (level >= 40) return "Master";
            if (level >= 25) return "Expert";
            if (level >= 10) return "Adept";
            return "Novice";
        }
    }
}
