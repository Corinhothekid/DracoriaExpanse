using System;
using System.Collections.Generic;
using System.Linq;
using WildsOfDracoria.CharacterCreation;
using WildsOfDracoria.Contracts;
using WildsOfDracoria.Items;
using WildsOfDracoria.Markets;
using WildsOfDracoria.Professions;
using WildsOfDracoria.Visuals;

namespace WildsOfDracoria.Data
{
    [Serializable]
    public class CharacterData
    {
        public string characterName = "New Adventurer";
        public string familyName = "Drakeward";
        public string race = "Human";
        public string bodyType = "Average";
        public string skinTone = "Warm";
        public string hairStyle = "Short";
        public string hairColor = "Brown";
        public string facialHairStyle = "None";
        public string eyeColor = "Hazel";
        public string startingHomeland = "Ironhaven";
        public CharacterVisualProfile visualProfile = new CharacterVisualProfile();

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
        public List<ContractData> contracts = new List<ContractData>();
        public List<VendorStallData> marketStalls = new List<VendorStallData>();

        public static CharacterData CreateDefault()
        {
            var data = new CharacterData();
            data.EnsureDefaultSkills();
            data.EnsureDefaultProfessions();
            data.NormalizeInventory();
            data.EnsureContracts();
            data.EnsureMarketStalls();
            data.EnsureStarterGatheringTools();
            data.EnsureVisualProfile();
            return data;
        }

        public static CharacterData CreateFromCharacterCreation(CharacterCreationData creationData)
        {
            var data = CreateDefault();
            data.ApplyCharacterCreation(creationData);
            return data;
        }

        public void ApplyCharacterCreation(CharacterCreationData creationData)
        {
            if (creationData == null)
            {
                return;
            }

            var raceDefinition = RaceRegistry.Get(creationData.race);
            characterName = CleanName(creationData.characterName, "New");
            familyName = CleanName(creationData.familyName, "Drakeward");
            race = raceDefinition.displayName;
            bodyType = CleanName(creationData.bodyType, "Average");
            skinTone = CleanName(creationData.skinTone, "Warm");
            hairStyle = CleanName(creationData.hairStyle, "Short");
            hairColor = CleanName(creationData.hairColor, "Brown");
            facialHairStyle = CleanName(creationData.facialHairStyle, "None");
            eyeColor = CleanName(creationData.eyeColor, "Hazel");
            startingHomeland = CleanName(creationData.startingHomeland, raceDefinition.homelandName);
            visualProfile = creationData.visualProfile ?? BuildVisualProfileFromLegacyFields(creationData.race);
            visualProfile.Normalize();
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

            if (string.IsNullOrWhiteSpace(startingHomeland))
            {
                startingHomeland = "Ironhaven";
            }

            EnsureVisualProfile();
            EnsureStarterGatheringTools();

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

        public void NormalizeInventory()
        {
            if (inventory == null)
            {
                inventory = new List<InventoryItem>();
                return;
            }

            inventory.RemoveAll(item => item == null);
            foreach (var item in inventory)
            {
                item.Normalize();
            }
        }

        public void EnsureContracts()
        {
            if (contracts == null)
            {
                contracts = new List<ContractData>();
                return;
            }

            contracts.RemoveAll(contract => contract == null);
            foreach (var contract in contracts)
            {
                contract.Normalize();
            }
        }

        public void EnsureMarketStalls()
        {
            if (marketStalls == null)
            {
                marketStalls = new List<VendorStallData>();
                return;
            }

            marketStalls.RemoveAll(stall => stall == null);
            foreach (var stall in marketStalls)
            {
                stall.Normalize();
            }
        }

        public void EnsureVisualProfile()
        {
            if (visualProfile == null)
            {
                visualProfile = BuildVisualProfileFromLegacyFields(race);
            }

            visualProfile.Normalize();
        }

        public void EnsureStarterGatheringTools()
        {
            if (!HasInventoryItem(ItemIds.BeginnerPickaxe, 1))
            {
                AddInventoryItem(ItemIds.BeginnerPickaxe, 1);
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

        public int GetInventoryQuantity(string itemId)
        {
            if (inventory == null)
            {
                return 0;
            }

            var normalizedId = ItemIds.Normalize(itemId);
            var existing = inventory.FirstOrDefault(item => ItemIds.Normalize(item.itemId) == normalizedId);
            return existing != null ? existing.quantity : 0;
        }

        public bool HasInventoryItem(string itemId, int quantity = 1)
        {
            return GetInventoryQuantity(itemId) >= quantity;
        }

        public void AddInventoryItem(string itemId, int quantity = 1)
        {
            var normalizedId = ItemIds.Normalize(itemId);
            if (string.IsNullOrWhiteSpace(normalizedId) || quantity <= 0 || !ItemDatabase.Exists(normalizedId))
            {
                return;
            }

            if (inventory == null)
            {
                inventory = new List<InventoryItem>();
            }

            var existing = inventory.FirstOrDefault(item => ItemIds.Normalize(item.itemId) == normalizedId);
            if (existing != null)
            {
                existing.quantity += quantity;
                return;
            }

            inventory.Add(new InventoryItem(normalizedId, quantity));
        }

        public bool RemoveInventoryItem(string itemId, int quantity = 1)
        {
            if (inventory == null)
            {
                return false;
            }

            var normalizedId = ItemIds.Normalize(itemId);
            if (string.IsNullOrWhiteSpace(normalizedId) || quantity <= 0)
            {
                return false;
            }

            var existing = inventory.FirstOrDefault(item => ItemIds.Normalize(item.itemId) == normalizedId);
            if (existing == null || existing.quantity < quantity)
            {
                return false;
            }

            existing.quantity -= quantity;
            if (existing.quantity <= 0)
            {
                inventory.Remove(existing);
            }

            return true;
        }

        private CharacterVisualProfile BuildVisualProfileFromLegacyFields(string raceIdOrName)
        {
            var normalizedRace = RaceIds.Normalize(raceIdOrName);
            return new CharacterVisualProfile
            {
                race = normalizedRace,
                bodyType = NormalizeVisualId(bodyType, "average"),
                skinToneId = NormalizeVisualId(skinTone, RaceVisualRegistry.Get(normalizedRace).defaultSkinTone),
                hairStyleId = MapHairStyle(hairStyle),
                hairColorId = NormalizeVisualId(hairColor, "brown"),
                eyeColorId = NormalizeVisualId(eyeColor, "hazel"),
                faceId = "face_soft",
                facialHairId = MapFacialHair(facialHairStyle),
                outfitId = "outfit_traveler",
                armorSetId = "armor_none",
                capeId = "cape_none",
                weaponVisualId = "weapon_training_sword"
            };
        }

        private void AddSkillIfMissing(string skillName)
        {
            if (GetSkill(skillName) == null)
            {
                skills.Add(new SkillData(skillName));
            }
        }

        private static string CleanName(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }

        private static string NormalizeVisualId(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim().ToLowerInvariant().Replace(" ", "_");
        }

        private static string MapHairStyle(string value)
        {
            switch (NormalizeVisualId(value, "short"))
            {
                case "long": return "hair_long";
                case "braided":
                case "spiky": return "hair_spiky";
                case "shaved": return "hair_none";
                default: return "hair_short";
            }
        }

        private static string MapFacialHair(string value)
        {
            switch (NormalizeVisualId(value, "none"))
            {
                case "trimmed": return "facial_trimmed";
                case "full": return "facial_full";
                default: return "facial_none";
            }
        }
    }
}
