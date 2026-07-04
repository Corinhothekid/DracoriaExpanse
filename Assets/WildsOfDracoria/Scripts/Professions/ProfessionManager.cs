using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WildsOfDracoria.Data;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI.Mobile;

namespace WildsOfDracoria.Professions
{
    public class ProfessionManager : MonoBehaviour
    {
        public static ProfessionManager Instance { get; private set; }

        public event Action<ProfessionData, int> ProfessionXPGained;
        public event Action<ProfessionData> ProfessionLeveledUp;
        public event Action<ProfessionData, string> ProfessionUnlocked;

        [SerializeField] private ProfessionUI professionUI;

        public IReadOnlyList<ProfessionData> Professions
        {
            get
            {
                InitializeProfessions();
                return GetCharacterData()?.professions;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializeProfessions();
            RefreshUI();
        }

        public void InitializeProfessions()
        {
            var data = GetCharacterData();
            if (data == null)
            {
                return;
            }

            if (data.professions == null)
            {
                data.professions = new List<ProfessionData>();
            }

            NormalizeExistingProfessionIds(data.professions);

            foreach (var definition in ProfessionRegistry.All)
            {
                var profession = GetProfession(definition.professionId);
                if (profession == null)
                {
                    profession = new ProfessionData(definition.professionId, definition.displayName, definition.description, definition.startsUnlocked);
                    data.professions.Add(profession);
                }
                else
                {
                    profession.ApplyDefinition(definition);
                }
            }
        }

        public ProfessionData GetProfession(string professionId)
        {
            var data = GetCharacterData();
            var normalizedId = ProfessionIds.Normalize(professionId);
            return data?.professions?.FirstOrDefault(profession => ProfessionIds.Normalize(profession.professionId) == normalizedId);
        }

        public void UnlockProfession(string professionId, string journalEntry = null)
        {
            InitializeProfessions();
            var profession = GetProfession(professionId);
            if (profession == null || profession.isUnlocked)
            {
                return;
            }

            profession.isUnlocked = true;
            profession.AddJournalEntry(journalEntry ?? $"Unlocked {profession.displayName}.");
            ProfessionUnlocked?.Invoke(profession, journalEntry);
            NotificationPopupUI.Instance?.Show($"{profession.displayName} unlocked");
            RefreshUI();
        }

        public void AddXP(string professionId, int amount, string journalEntry = null)
        {
            InitializeProfessions();
            var profession = GetProfession(professionId);
            if (profession == null || !profession.isUnlocked || amount <= 0)
            {
                return;
            }

            var leveledUp = profession.AddXP(amount);
            if (!string.IsNullOrWhiteSpace(journalEntry))
            {
                profession.AddJournalEntry(journalEntry);
            }

            ProfessionXPGained?.Invoke(profession, amount);
            NotificationPopupUI.Instance?.Show($"+{amount} {profession.displayName} Profession XP");

            if (leveledUp)
            {
                ProfessionLeveledUp?.Invoke(profession);
                NotificationPopupUI.Instance?.Show($"{profession.displayName} reached level {profession.level}");
            }

            RefreshUI();
        }

        public void GainProfessionXP(string professionId, int amount, string journalEntry = null)
        {
            AddXP(professionId, amount, journalEntry);
        }

        public void RegisterUI(ProfessionUI ui)
        {
            professionUI = ui;
            RefreshUI();
        }

        public void RefreshFromCharacterData()
        {
            InitializeProfessions();
            RefreshUI();
        }

        private void RefreshUI()
        {
            professionUI?.Refresh();
        }

        private CharacterData GetCharacterData()
        {
            return GameManager.Instance != null ? GameManager.Instance.CharacterData : null;
        }

        private static void NormalizeExistingProfessionIds(List<ProfessionData> professions)
        {
            foreach (var profession in professions)
            {
                profession.professionId = ProfessionIds.Normalize(profession.professionId);
            }
        }
    }
}
