using System;
using System.Collections.Generic;
using System.Linq;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.Data
{
    [Serializable]
    public class CharacterData
    {
        public string characterName = "New Adventurer";
        public string familyName = "Drakeward";
        public string race = "Human";

        // Kept for compatibility with the first prototype save shape.
        public int health = 100;
        public int stamina = 100;

        public int maxHealth = 100;
        public int currentHealth = 100;
        public int maxStamina = 100;
        public int currentStamina = 100;
        public int gold = 25;
        public string currentProfessionFocus = ProfessionIds.Fishing;
        public string equippedWeapon = "Training Sword";
        public List<SkillData> skills = new List<SkillData>();
        public List<ProfessionData> professions = new List<ProfessionData>();
        public List<InventoryItem> inventory = new List<InventoryItem>();

        public static CharacterData CreateDefault()
        {
            var data = new CharacterData();
            data.EnsureDefaultSkills();
            data.EnsureDefaultProfessions();
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
            AddSkillIfMissing("Endurance");

            if (maxHealth <= 0)
            {
                maxHealth = 100;
            }

            if (maxStamina <= 0)
            {
                maxStamina = 100;
            }

            if (currentHealth <= 0 && health > 0)
            {
                currentHealth = health;
            }

            if (currentStamina <= 0 && stamina > 0)
            {
                currentStamina = stamina;
            }

            currentHealth = Math.Max(1, Math.Min(currentHealth, maxHealth));
            currentStamina = Math.Max(0, Math.Min(currentStamina, maxStamina));

            if (string.IsNullOrWhiteSpace(equippedWeapon))
            {
                equippedWeapon = "Training Sword";
            }

            currentProfessionFocus = ProfessionIds.Normalize(currentProfessionFocus);
            if (string.IsNullOrWhiteSpace(currentProfessionFocus))
            {
                currentProfessionFocus = ProfessionIds.Fishing;
            }
        }

        public void EnsureDefaultProfessions()
        {
            if (professions == null)
            {
                professions = new List<ProfessionData>();
            }

            foreach (var existing in professions)
            {
                existing.professionId = ProfessionIds.Normalize(existing.professionId);
            }

            foreach (var definition in ProfessionRegistry.All)
            {
                var existing = GetProfession(definition.professionId);
                if (existing == null)
                {
                    professions.Add(new ProfessionData(definition.professionId, definition.displayName, definition.description, definition.startsUnlocked));
                }
                else
                {
                    existing.ApplyDefinition(definition);
                }
            }
        }

        public SkillData GetSkill(string skillName)
        {
            return skills.FirstOrDefault(skill => skill.skillName == skillName);
        }

        public ProfessionData GetProfession(string professionId)
        {
            var normalizedId = ProfessionIds.Normalize(professionId);
            return professions?.FirstOrDefault(profession => ProfessionIds.Normalize(profession.professionId) == normalizedId);
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
