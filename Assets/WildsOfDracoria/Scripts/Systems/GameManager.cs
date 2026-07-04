using UnityEngine;
using WildsOfDracoria.Data;
using WildsOfDracoria.Save;
using WildsOfDracoria.UI;

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
        }

        private void Start()
        {
            FindUIIfNeeded();
            RefreshInventoryUI();
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

        public void AddItem(string itemName, int quantity = 1)
        {
            characterData.AddInventoryItem(itemName, quantity);
            RefreshInventoryUI();
        }

        public void GainSkillXP(string skillName, int amount)
        {
            var leveledUp = characterData.GainSkillXP(skillName, amount);
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

            characterData = loaded;
            RefreshInventoryUI();
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
        }

        private void RefreshInventoryUI()
        {
            inventoryUI?.Refresh(characterData);
        }
    }
}
