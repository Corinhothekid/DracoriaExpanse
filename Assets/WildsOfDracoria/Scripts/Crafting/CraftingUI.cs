using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.Items;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.Crafting
{
    public class CraftingUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text titleText;
        [SerializeField] private Text recipeListText;
        [SerializeField] private Text detailsText;
        [SerializeField] private Text warningText;
        [SerializeField] private Button nextRecipeButton;
        [SerializeField] private Button craftButton;

        private CraftingManager manager;
        private IReadOnlyList<RecipeDefinition> recipes = new List<RecipeDefinition>();
        private int selectedIndex;
        private CraftingStationType currentStationType;

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            if (nextRecipeButton != null)
            {
                nextRecipeButton.onClick.AddListener(SelectNextRecipe);
            }

            if (craftButton != null)
            {
                craftButton.onClick.AddListener(CraftSelected);
            }
        }

        private void Start()
        {
            CraftingManager.Instance?.RegisterUI(this);
        }

        private void Update()
        {
            if (panel != null && panel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }

        public void RegisterManager(CraftingManager craftingManager)
        {
            manager = craftingManager;
        }

        public void Open(CraftingStationType stationType, IReadOnlyList<RecipeDefinition> availableRecipes)
        {
            currentStationType = stationType;
            recipes = availableRecipes ?? new List<RecipeDefinition>();
            selectedIndex = 0;

            if (panel != null)
            {
                panel.SetActive(true);
            }

            RefreshSelectedRecipe();
        }

        public void Close()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        public void SelectNextRecipe()
        {
            if (recipes.Count == 0)
            {
                return;
            }

            selectedIndex = (selectedIndex + 1) % recipes.Count;
            RefreshSelectedRecipe();
        }

        public void CraftSelected()
        {
            var recipe = GetSelectedRecipe();
            if (recipe == null)
            {
                return;
            }

            manager?.TryCraft(recipe.recipeId);
        }

        public void RefreshSelectedRecipe()
        {
            if (titleText != null)
            {
                titleText.text = $"{currentStationType} Crafting";
            }

            RefreshRecipeList();
            RefreshDetails();
        }

        private void RefreshRecipeList()
        {
            if (recipeListText == null)
            {
                return;
            }

            if (recipes.Count == 0)
            {
                recipeListText.text = "No recipes available.";
                return;
            }

            var builder = new StringBuilder();
            for (var i = 0; i < recipes.Count; i++)
            {
                var marker = i == selectedIndex ? "> " : "  ";
                builder.AppendLine($"{marker}{recipes[i].displayName}");
            }

            recipeListText.text = builder.ToString();
        }

        private void RefreshDetails()
        {
            var recipe = GetSelectedRecipe();
            if (recipe == null)
            {
                if (detailsText != null)
                {
                    detailsText.text = string.Empty;
                }

                if (warningText != null)
                {
                    warningText.text = string.Empty;
                }

                if (craftButton != null)
                {
                    craftButton.interactable = false;
                }

                return;
            }

            var details = new StringBuilder();
            details.AppendLine(recipe.displayName);
            details.AppendLine(recipe.description);
            details.AppendLine();
            details.AppendLine($"Profession: {GetProfessionName(recipe.requiredProfessionId)} Lv {recipe.requiredProfessionLevel}");
            details.AppendLine($"Time: {recipe.craftingTimeSeconds:0.#}s");
            details.AppendLine($"XP: {recipe.xpReward}");
            details.AppendLine();
            details.AppendLine("Inputs:");
            foreach (var input in recipe.inputItems)
            {
                details.AppendLine($"- {input.GetDisplayText()}");
            }

            details.AppendLine("Outputs:");
            foreach (var output in recipe.outputItems)
            {
                output.Normalize();
                details.AppendLine($"- {ItemDatabase.GetDisplayName(output.itemId)} x{output.quantity}");
            }

            if (!string.IsNullOrWhiteSpace(recipe.requiredToolItemId))
            {
                details.AppendLine($"Tool: {ItemDatabase.GetDisplayName(recipe.requiredToolItemId)}");
            }

            if (detailsText != null)
            {
                detailsText.text = details.ToString();
            }

            var canCraft = manager != null && manager.CanCraft(recipe, out var warning);
            if (warningText != null)
            {
                warningText.text = canCraft ? "Ready to craft." : warning;
            }

            if (craftButton != null)
            {
                craftButton.interactable = canCraft;
            }
        }

        private RecipeDefinition GetSelectedRecipe()
        {
            if (recipes.Count == 0 || selectedIndex < 0 || selectedIndex >= recipes.Count)
            {
                return null;
            }

            return recipes[selectedIndex];
        }

        private static string GetProfessionName(string professionId)
        {
            var profession = ProfessionManager.Instance?.GetProfession(professionId);
            return profession != null ? profession.displayName : ProfessionIds.Normalize(professionId);
        }
    }
}