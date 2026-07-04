using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace WildsOfDracoria.Professions
{
    [Serializable]
    public class ProfessionData
    {
        [FormerlySerializedAs("professionName")]
        public string professionId;
        public string displayName;
        public string description;
        public int level;
        public int currentXP;
        [FormerlySerializedAs("xpRequired")]
        public int xpToNextLevel;
        public string masteryRank;
        public int reputation;
        public bool isUnlocked;
        [FormerlySerializedAs("unlocks")]
        public List<string> discoveredUnlocks = new List<string>();
        [FormerlySerializedAs("professionJournal")]
        public List<string> journalEntries = new List<string>();

        public ProfessionData(string professionId, string displayName, string description, bool isUnlocked = false)
        {
            this.professionId = ProfessionIds.Normalize(professionId);
            this.displayName = displayName;
            this.description = description;
            this.level = 1;
            this.currentXP = 0;
            this.xpToNextLevel = 100;
            this.masteryRank = "Novice";
            this.reputation = 0;
            this.isUnlocked = isUnlocked;
        }

        public void ApplyDefinition(ProfessionDefinition definition)
        {
            professionId = ProfessionIds.Normalize(professionId);
            if (definition == null)
            {
                return;
            }

            professionId = definition.professionId;
            displayName = definition.displayName;
            description = definition.description;

            if (definition.startsUnlocked)
            {
                isUnlocked = true;
            }

            if (level <= 0)
            {
                level = 1;
            }

            if (xpToNextLevel <= 0)
            {
                xpToNextLevel = CalculateXPToNextLevel(level);
            }

            if (string.IsNullOrWhiteSpace(masteryRank))
            {
                masteryRank = CalculateMasteryRank(level);
            }

            if (discoveredUnlocks == null)
            {
                discoveredUnlocks = new List<string>();
            }

            if (journalEntries == null)
            {
                journalEntries = new List<string>();
            }
        }

        public bool AddXP(int amount)
        {
            if (!isUnlocked || amount <= 0)
            {
                return false;
            }

            var leveledUp = false;
            currentXP += amount;

            while (currentXP >= xpToNextLevel)
            {
                currentXP -= xpToNextLevel;
                level++;
                xpToNextLevel = CalculateXPToNextLevel(level);
                masteryRank = CalculateMasteryRank(level);
                leveledUp = true;
            }

            return leveledUp;
        }

        public void AddJournalEntry(string entry)
        {
            if (!string.IsNullOrWhiteSpace(entry))
            {
                journalEntries.Add(entry);
            }
        }

        public void AddUnlock(string unlockId)
        {
            if (!string.IsNullOrWhiteSpace(unlockId) && !discoveredUnlocks.Contains(unlockId))
            {
                discoveredUnlocks.Add(unlockId);
            }
        }

        public static int CalculateXPToNextLevel(int level)
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
