using UnityEngine;
using WildsOfDracoria.CharacterCreation;
using WildsOfDracoria.Combat;
using WildsOfDracoria.Contracts;
using WildsOfDracoria.Crafting;
using WildsOfDracoria.Data;
using WildsOfDracoria.Gathering;
using WildsOfDracoria.Inputs;
using WildsOfDracoria.Professions;
using WildsOfDracoria.Save;
using WildsOfDracoria.UI;
using WildsOfDracoria.UI.Mobile;
using WildsOfDracoria.Visuals;

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
            EnsureContractManager();
            EnsureCharacterCreationStartup();
            EnsureGatheringNodeBootstrap();
        }

        private void Start()
        {
            FindUIIfNeeded();
            ProfessionManager.Instance?.RefreshFromCharacterData();
            ContractManager.Instance?.RefreshAllProgress();
            ApplyLoadedDataToPlayer();
            RefreshPlayerUI();
        }

        private void Update()
        {
            if (DracoriaInput.GetKeyDown(KeyCode.I))
            {
                inventoryUI?.Toggle();
            }

            if (DracoriaInput.GetKeyDown(KeyCode.F5))
            {
                SaveGame();
            }

            if (DracoriaInput.GetKeyDown(KeyCode.F9))
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
            characterData.EnsureVisualProfile();
            characterData.NormalizeInventory();
            characterData.EnsureContracts();
            ProfessionManager.Instance?.RefreshFromCharacterData();
            ContractManager.Instance?.RefreshAllProgress();
            ApplyLoadedDataToPlayer();
            RefreshPlayerUI();

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
            ContractManager.Instance?.RefreshProgressForInventoryChange();
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
            characterData.EnsureVisualProfile();
            characterData.EnsureContracts();
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

        public void RefreshPlayerUI()
        {
            RefreshInventoryUI();
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

        private void EnsureContractManager()
        {
            if (ContractManager.Instance != null || GetComponent<ContractManager>() != null)
            {
                return;
            }

            gameObject.AddComponent<ContractManager>();
        }

        private void EnsureCharacterCreationStartup()
        {
            if (GetComponent<CharacterCreationStartup>() != null)
            {
                return;
            }

            gameObject.AddComponent<CharacterCreationStartup>();
        }

        private void EnsureGatheringNodeBootstrap()
        {
            if (GetComponent<GatheringNodeSceneBootstrap>() != null)
            {
                return;
            }

            gameObject.AddComponent<GatheringNodeSceneBootstrap>();
        }

        private void ApplyLoadedDataToPlayer()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var vitals = Object.FindAnyObjectByType<PlayerVitals>();
            vitals?.ApplyCharacterData(characterData);

            var combat = Object.FindAnyObjectByType<PlayerCombat>();
            if (combat != null)
            {
                combat.EquipWeapon(characterData.equippedWeapon);
            }

            if (player != null)
            {
                var visuals = player.GetComponent<CharacterVisualManager>() ?? player.AddComponent<CharacterVisualManager>();
                visuals.ApplyProfile(characterData.visualProfile);
            }
        }

        private void FindUIIfNeeded()
        {
            if (inventoryUI == null)
            {
                inventoryUI = Object.FindAnyObjectByType<InventoryUI>(FindObjectsInactive.Include);
            }

            if (dialogueUI == null)
            {
                dialogueUI = Object.FindAnyObjectByType<DialogueUI>(FindObjectsInactive.Include);
            }

            if (characterSheetUI == null)
            {
                characterSheetUI = Object.FindAnyObjectByType<CharacterSheetUI>(FindObjectsInactive.Include) ?? CharacterSheetUIFactory.Create();
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
