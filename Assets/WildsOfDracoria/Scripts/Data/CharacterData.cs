using System;
using System.Collections.Generic;
using System.Linq;

namespace WildsOfDracoria.Data
{
    [Serializable]
    public class CharacterData
    {
        public string characterName = "New Adventurer";
        public string familyName = "Drakeward";
        public string race = "Human";
        public int health = 100;
        public int stamina = 100;
        public int gold = 25;
        public string currentProfessionFocus = "Fishing";
        public List<SkillData> skills = new List<SkillData>();
        public List<InventoryItem> inventory = new List<InventoryItem>();

        public static CharacterData CreateDefault()
        {
            var data = new CharacterData();
            data.EnsureDefaultSkills();
            return data;
        }

        public void EnsureDefaultSkills()
        {
            AddSkillIfMissing("Fishing");
            AddSkillIfMissing("Mining");
            AddSkillIfMissing("Blacksmithing");
            AddSkillIfMissing("Cooking");
            AddSkillIfMissing("Sailing");
            AddSkillIfMissing("Swordsmanship");
        }

        public SkillData GetSkill(string skillName)
        {
            return skills.FirstOrDefault(skill => skill.skillName == skillName);
        }

        public bool GainSkillXP(string skillName, int amount)
        {
            var skill = GetSkill(skillName);
            if (skill == null)
            {
                skill = new SkillData(skillName);
                skills.Add(skill);
            }

            return skill.GainXP(amount);
        }

        public void AddInventoryItem(string itemName, int quantity = 1)
        {
            if (string.IsNullOrWhiteSpace(itemName) || quantity <= 0)
            {
                return;
            }

            var existing = inventory.FirstOrDefault(item => item.itemName == itemName);
            if (existing != null)
            {
                existing.quantity += quantity;
                return;
            }

            inventory.Add(new InventoryItem(itemName, quantity));
        }

        private void AddSkillIfMissing(string skillName)
        {
            if (GetSkill(skillName) == null)
            {
                skills.Add(new SkillData(skillName));
            }
        }
    }
}
