using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WildsOfDracoria.Data;
using WildsOfDracoria.Items;
using WildsOfDracoria.Professions;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI.Mobile;

namespace WildsOfDracoria.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        public static CraftingManager Instance { get; private set; }

        [SerializeField] private CraftingUI craftingUI;

        private readonly Dictionary<string, RecipeDefinition> recipes = new Dictionary<string, RecipeDefinition>();

        private void Awake()
        {
            Instance = this;
            RegisterRecipes(RecipeRegistry.All);
        }

        private void Start()
        {
            if (craftingUI == null)
            {
                craftingUI = FindObjectOfType<CraftingUI>(true);
            }

            craftingUI?.RegisterManager(this);
        }

        public void RegisterUI(CraftingUI ui)
        {
            craftingUI = ui;
            craftingUI?.RegisterManager(this);
        }

        public void RegisterRecipes(IEnumerable<RecipeDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                RegisterRecipe(definition);
            }
        }

        public void RegisterRecipe(RecipeDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.recipeId))
            {
                return;
            }

            definition.Normalize();
            recipes[definition.recipeId] = definition;
        }

        public RecipeDefinition GetRecipe(string recipeId)
        {
            return !string.IsNullOrWhiteSpace(recipeId) && recipes.TryGetValue(recipeId, out var recipe) ? recipe : null;
        }

        public IReadOnlyList<RecipeDefinition> GetRecipesForStation(CraftingStationType stationType)
        {
            return recipes.Values.Where(recipe => recipe.craftingStationType == stationType).OrderBy(recipe => recipe.displayName).ToList();
        }

        public void OpenStation(CraftingStationType stationType)
        {
            var stationRecipes = GetRecipesForStation(stationType);
            if (craftingUI == null)
            {
                craftingUI = FindObjectOfType<CraftingUI>(true);
            }

            if (craftingUI != null)
            {
                craftingUI.Open(stationType, stationRecipes);
                return;
            }

            GameManager.Instance.DialogueUI?.ShowLine($"{stationType} has {stationRecipes.Count} recipe(s), but no crafting UI is present.");
        }

        public bool CanCraft(RecipeDefinition recipe, out string warning)
        {
            warning = string.Empty;
            var data = GetCharacterData();
            if (recipe == null)
            {
                warning = "No recipe selected.";
                return false;
            }

            if (data == null)
            {
                warning = "Character data is not ready.";
                return false;
            }

            var profession = ProfessionManager.Instance?.GetProfession(recipe.requiredProfessionId);
            if (profession == null)
            {
                warning = "Required profession is missing.";
                return false;
            }

            if (!profession.isUnlocked)
            {
                warning = $"{profession.displayName} is locked.";
                return false;
            }

            if (profession.level < recipe.requiredProfessionLevel)
            {
                warning = $"Requires {profession.displayName} level {recipe.requiredProfessionLevel}.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(recipe.requiredToolItemId) && !data.HasInventoryItem(recipe.requiredToolItemId, 1))
            {
                warning = $"Requires {ItemDatabase.GetDisplayName(recipe.requiredToolItemId)}.";
                return false;
            }

            foreach (var input in recipe.inputItems)
            {
                input.Normalize();
                if (!data.HasInventoryItem(input.itemId, input.quantity))
                {
                    warning = $"Missing {input.GetDisplayText()}.";
                    return false;
                }
            }

            return true;
        }

        public bool TryCraft(string recipeId)
        {
            var recipe = GetRecipe(recipeId);
            if (!CanCraft(recipe, out var warning))
            {
                Notify(warning);
                craftingUI?.RefreshSelectedRecipe();
                return false;
            }

            var data = GetCharacterData();
            foreach (var input in recipe.inputItems)
            {
                data.RemoveInventoryItem(input.itemId, input.quantity);
            }

            foreach (var output in recipe.outputItems)
            {
                GameManager.Instance.AddItem(output.itemId, output.quantity);
            }

            ProfessionManager.Instance?.AddXP(recipe.requiredProfessionId, recipe.xpReward, $"Crafted {recipe.displayName}.");
            Notify($"Crafted {recipe.displayName}");
            craftingUI?.RefreshSelectedRecipe();
            return true;
        }

        private CharacterData GetCharacterData()
        {
            return GameManager.Instance != null ? GameManager.Instance.CharacterData : null;
        }

        private static void Notify(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            NotificationPopupUI.Instance?.Show(message);
            GameManager.Instance.DialogueUI?.ShowLine(message);
        }
    }
}