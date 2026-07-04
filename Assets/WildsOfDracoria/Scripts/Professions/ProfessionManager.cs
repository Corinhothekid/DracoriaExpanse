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

        [SerializeField] private ProfessionUI professionUI;

        public IReadOnlyList<ProfessionData> Professions => GetCharacterData()?.professions;

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

            foreach (var definition in ProfessionRegistry.All)
            {
                var profession = GetProfession(definition.professionName);
                if (profession == null)
                {
                    profession = new ProfessionData(definition.professionName, definition.startsUnlocked);
                    profession.unlocks.AddRange(definition.defaultUnlocks);
                    profession.activeBonuses.AddRange(definition.defaultBonuses);
                    data.professions.Add(profession);
                }
                else
                {
                    profession.xpRequired = profession.xpRequired <= 0 ? ProfessionData.CalculateXPRequired(profession.level) : profession.xpRequired;
                    profession.masteryRank = string.IsNullOrWhiteSpace(profession.masteryRank) ? ProfessionData.CalculateMasteryRank(profession.level) : profession.masteryRank;
                    if (definition.startsUnlocked)
                    {
                        profession.isUnlocked = true;
                    }
                }
            }
        }

        public ProfessionData GetProfession(string professionName)
        {
            var data = GetCharacterData();
            return data?.professions?.FirstOrDefault(profession => profession.professionName == professionName);
        }

        public void UnlockProfession(string professionName, string journalEntry = null)
        {
            InitializeProfessions();
            var profession = GetProfession(professionName);
            if (profession == null || profession.isUnlocked)
            {
                return;
            }

            profession.isUnlocked = true;
            profession.AddJournalEntry(journalEntry ?? $"Unlocked {professionName}.");
            NotificationPopupUI.Instance?.Show($"{professionName} unlocked");
            RefreshUI();
        }

        public void GainProfessionXP(string professionName, int amount, string journalEntry = null)
        {
            InitializeProfessions();
            var profession = GetProfession(professionName);
            if (profession == null || !profession.isUnlocked || amount <= 0)
            {
                return;
            }

            var leveledUp = profession.GainXP(amount);
            if (!string.IsNullOrWhiteSpace(journalEntry))
            {
                profession.AddJournalEntry(journalEntry);
            }

            NotificationPopupUI.Instance?.Show($"+{amount} {professionName} Profession XP");
            if (leveledUp)
            {
                NotificationPopupUI.Instance?.Show($"{professionName} reached level {profession.level}");
            }

            RefreshUI();
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
    }
}
