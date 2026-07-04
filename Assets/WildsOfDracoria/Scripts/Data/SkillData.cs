using System;

namespace WildsOfDracoria.Data
{
    [Serializable]
    public class SkillData
    {
        public string skillName;
        public int level;
        public int currentXP;
        public int xpToNextLevel;

        public SkillData(string skillName, int level = 1, int currentXP = 0, int xpToNextLevel = 100)
        {
            this.skillName = skillName;
            this.level = level;
            this.currentXP = currentXP;
            this.xpToNextLevel = xpToNextLevel;
        }

        public bool GainXP(int amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            var leveledUp = false;
            currentXP += amount;

            while (currentXP >= xpToNextLevel)
            {
                currentXP -= xpToNextLevel;
                level++;
                xpToNextLevel = CalculateNextLevelXP(level);
                leveledUp = true;
            }

            return leveledUp;
        }

        private static int CalculateNextLevelXP(int newLevel)
        {
            return 100 + ((newLevel - 1) * 35);
        }
    }
}
