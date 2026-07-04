using UnityEngine;
using WildsOfDracoria.CharacterCreation;
using WildsOfDracoria.Combat;
using WildsOfDracoria.Crafting;
using WildsOfDracoria.Data;
using WildsOfDracoria.Professions;
using WildsOfDracoria.Save;
using WildsOfDracoria.UI;
using WildsOfDracoria.UI.Mobile;

namespace WildsOfDracoria.Systems
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Runtime Data")]
        [SerializeField] private CharacterData characterData;

        [Header("UI")]
        [SerializeField] private InventoryUI inventoryUI;
        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private CharacterSheetUI characterSheetUI;

        public CharacterData CharacterData => characterData;
        public DialogueUI DialogueUI => dialogueUI;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            characterData = CharacterData.CreateDefault();
            characterData.NormalizeInventory();
            EnsureProfessionManager();
            EnsureCraftingManager();
            EnsureCharacterCreationStartup();
        }

        private void Start()
        {
            FindUIIfNeeded();
            ProfessionManager.Instance?.RefreshFromCharacterData();
            RefreshInventoryUI();
            RefreshCharacterSheetUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryUI?.Toggle();
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveGame();
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadGame();
            }
        }

        public void SetCharacterData(CharacterData data, bool saveImmediately)
        {
            if (data == null)
            {
                return;
            }

            characterData = data;
            characterData.EnsureDefaultSkills();
            characterData.EnsureDefaultProfessions();
            characterData.NormalizeInventory();
            ProfessionManager.Instance?.RefreshFromCharacterData();
            ApplyLoadedDataToPlayer();
            RefreshInventoryUI();
            RefreshCharacterSheetUI();

            if (saveImmediately)
            {
                SaveGame();
            }
        }

        public void ApplyCharacterCreation(CharacterCreationData creationData)
        {
            SetCharacterData(CharacterData.CreateFromCharacterCreation(creationData), true);
        }

        public void AddItem(string itemId, int quantity = 1)
        {
            characterData.AddInventoryItem(itemId, quantity);
            RefreshInventoryUI();
        }

        public void GainSkillXP(string skillName, int amount)
        {
            var leveledUp = characterData.GainSkillXP(skillName, amount);
            NotificationPopupUI.Instance?.Show($"+{amount} {skillName} XP");
            if (leveledUp)
            {
                dialogueUI?.ShowLine($"{skillName} increased to level {characterData.GetSkill(skillName).level}!");
            }
        }

        public void SaveGame()
        {
            JsonSaveSystem.Save(characterData);
            dialogueUI?.ShowLine("Progress saved.");
        }

        public void LoadGame()
        {
            var loaded = JsonSaveSystem.Load();
            if (loaded == null)
            {
                dialogueUI?.ShowLine("No save file found.");
                return;
            }

            SetCharacterData(loaded, false);
            dialogueUI?.ShowLine("Progress loaded.");
        }

        public void RegisterInventoryUI(InventoryUI ui)
        {
            inventoryUI = ui;
            RefreshInventoryUI();
        }

        public void RegisterDialogueUI(DialogueUI ui)
        {
            dialogueUI = ui;
        }

        public void RegisterCharacterSheetUI(CharacterSheetUI ui)
        {
            characterSheetUI = ui;
            RefreshCharacterSheetUI();
        }

        private void EnsureProfessionManager()
        {
            if (ProfessionManager.Instance != null || GetComponent<ProfessionManager>() != null)
            {
                return;
            }

            gameObject.AddComponent<ProfessionManager>();
        }

        private void EnsureCraftingManager()
        {
            if (CraftingManager.Instance != null || GetComponent<CraftingManager>() != null)
            {
                return;
            }

            gameObject.AddComponent<CraftingManager>();
        }

        private void EnsureCharacterCreationStartup()
        {
            if (GetComponent<CharacterCreationStartup>() != null)
            {
                return;
            }

            gameObject.AddComponent<CharacterCreationStartup>();
        }

        private void ApplyLoadedDataToPlayer()
        {
            var vitals = FindObjectOfType<PlayerVitals>();
            vitals?.ApplyCharacterData(characterData);

            var combat = FindObjectOfType<PlayerCombat>();
            if (combat != null)
            {
                combat.EquipWeapon(characterData.equippedWeapon);
            }
        }

        private void FindUIIfNeeded()
        {
            if (inventoryUI == null)
            {
                inventoryUI = FindObjectOfType<InventoryUI>(true);
            }

            if (dialogueUI == null)
            {
                dialogueUI = FindObjectOfType<DialogueUI>(true);
            }

            if (characterSheetUI == null)
            {
                characterSheetUI = FindObjectOfType<CharacterSheetUI>(true);
            }
        }

        private void RefreshInventoryUI()
        {
            inventoryUI?.Refresh(characterData);
        }

        private void RefreshCharacterSheetUI()
        {
            characterSheetUI?.Refresh(characterData);
        }
    }
}