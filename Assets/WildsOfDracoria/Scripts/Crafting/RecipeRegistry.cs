using System.Collections.Generic;
using WildsOfDracoria.Items;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.Crafting
{
    public static class RecipeRegistry
    {
        private static readonly List<RecipeDefinition> Recipes = new List<RecipeDefinition>
        {
            new RecipeDefinition("cook_simple_bread", "Simple Bread", "Cook a plain travel meal at a campfire.", ProfessionIds.Cooking, 1, CraftingStationType.Campfire, 2f, 20)
            {
                inputItems = new List<CraftingItemStack>
                {
                    new CraftingItemStack(ItemIds.RawMeat, 2)
                },
                outputItems = new List<CraftingItemStack>
                {
                    new CraftingItemStack(ItemIds.SimpleBread, 1)
                },
                successChance = 1f,
                qualityChanceBonus = 0f
            },
            new RecipeDefinition("forge_training_sword", "Training Sword", "Forge a starter blade for basic swordsmanship practice.", ProfessionIds.Blacksmithing, 1, CraftingStationType.Forge, 3f, 30)
            {
                inputItems = new List<CraftingItemStack>
                {
                    new CraftingItemStack(ItemIds.CopperOre, 3),
                    new CraftingItemStack(ItemIds.OakLog, 1)
                },
                outputItems = new List<CraftingItemStack>
                {
                    new CraftingItemStack(ItemIds.TrainingSword, 1)
                },
                successChance = 1f,
                qualityChanceBonus = 0f
            }
        };

        public static IReadOnlyList<RecipeDefinition> All => Recipes;
    }
}