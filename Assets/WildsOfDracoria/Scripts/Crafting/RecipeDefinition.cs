using System;
using System.Collections.Generic;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.Crafting
{
    [Serializable]
    public class RecipeDefinition
    {
        public string recipeId;
        public string displayName;
        public string description;
        public string requiredProfessionId;
        public int requiredProfessionLevel = 1;
        public CraftingStationType craftingStationType;
        public List<CraftingItemStack> inputItems = new List<CraftingItemStack>();
        public List<CraftingItemStack> outputItems = new List<CraftingItemStack>();
        public float craftingTimeSeconds = 2f;
        public int xpReward = 10;
        public string requiredToolItemId;
        public float successChance = 1f;
        public float qualityChanceBonus;

        public RecipeDefinition()
        {
        }

        public RecipeDefinition(string recipeId, string displayName, string description, string requiredProfessionId, int requiredProfessionLevel, CraftingStationType craftingStationType, float craftingTimeSeconds, int xpReward)
        {
            this.recipeId = recipeId;
            this.displayName = displayName;
            this.description = description;
            this.requiredProfessionId = ProfessionIds.Normalize(requiredProfessionId);
            this.requiredProfessionLevel = requiredProfessionLevel;
            this.craftingStationType = craftingStationType;
            this.craftingTimeSeconds = craftingTimeSeconds;
            this.xpReward = xpReward;
        }

        public void Normalize()
        {
            requiredProfessionId = ProfessionIds.Normalize(requiredProfessionId);
            foreach (var input in inputItems)
            {
                input.Normalize();
            }

            foreach (var output in outputItems)
            {
                output.Normalize();
            }
        }
    }
}