using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WildsOfDracoria.Data;
using WildsOfDracoria.Items;
using WildsOfDracoria.Professions;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI.Mobile;

namespace WildsOfDracoria.Contracts
{
    public class ContractManager : MonoBehaviour
    {
        public static ContractManager Instance { get; private set; }

        public event Action<ContractData> ContractAccepted;
        public event Action<ContractData> ContractReadyToTurnIn;
        public event Action<ContractData> ContractCompleted;

        [SerializeField] private ContractBoardUI contractBoardUI;

        public IReadOnlyList<ContractData> AvailableContracts
        {
            get
            {
                EnsureContracts();
                var savedContracts = GetSavedContracts();
                return ContractRegistry.All
                    .Where(definition => savedContracts.All(saved => saved.contractId != definition.contractId || saved.status == ContractStatus.Abandoned))
                    .Select(definition => definition.Clone())
                    .ToList();
            }
        }

        public IReadOnlyList<ContractData> PlayerContracts
        {
            get
            {
                EnsureContracts();
                return GetSavedContracts();
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            EnsureContracts();
            if (contractBoardUI == null)
            {
                contractBoardUI = UnityEngine.Object.FindAnyObjectByType<ContractBoardUI>(FindObjectsInactive.Include);
            }

            contractBoardUI?.RegisterManager(this);
            RefreshAllProgress();
        }

        public void RegisterUI(ContractBoardUI ui)
        {
            contractBoardUI = ui;
            contractBoardUI?.RegisterManager(this);
            contractBoardUI?.Refresh();
        }

        public void OpenBoard()
        {
            EnsureContracts();
            RefreshAllProgress();
            if (contractBoardUI == null)
            {
                contractBoardUI = UnityEngine.Object.FindAnyObjectByType<ContractBoardUI>(FindObjectsInactive.Include) ?? ContractBoardUIFactory.Create();
                contractBoardUI.RegisterManager(this);
            }

            contractBoardUI.Open();
        }

        public bool AcceptContract(string contractId)
        {
            EnsureContracts();
            var normalizedId = ContractIds.Normalize(contractId);
            var savedContracts = GetSavedContracts();
            if (savedContracts.Any(contract => contract.contractId == normalizedId && contract.status != ContractStatus.Abandoned))
            {
                Notify("Contract already accepted or completed.");
                return false;
            }

            var definition = ContractRegistry.GetDefinition(normalizedId);
            if (definition == null)
            {
                Notify("Contract is not registered.");
                return false;
            }

            var contract = definition.Clone();
            contract.status = ContractStatus.Accepted;
            contract.Normalize();
            savedContracts.Add(contract);
            RefreshProgress(contract);
            ContractAccepted?.Invoke(contract);
            Notify($"Accepted: {contract.title}");
            contractBoardUI?.Refresh();
            return true;
        }

        public bool CompleteContract(string contractId)
        {
            EnsureContracts();
            var contract = GetPlayerContract(contractId);
            if (contract == null)
            {
                Notify("Contract is not accepted.");
                return false;
            }

            RefreshProgress(contract);
            if (contract.status != ContractStatus.ReadyToTurnIn)
            {
                Notify("Contract objective is not complete yet.");
                return false;
            }

            var data = GetCharacterData();
            if (!ConsumeTurnInItems(data, contract, out var warning))
            {
                Notify(warning);
                RefreshProgress(contract);
                contractBoardUI?.Refresh();
                return false;
            }

            data.gold += contract.rewardGold;
            AwardProgression(contract);
            contract.status = ContractStatus.Completed;
            ContractCompleted?.Invoke(contract);
            Notify($"Completed: {contract.title} (+{contract.rewardGold} gold)");
            GameManager.Instance?.RefreshPlayerUI();
            contractBoardUI?.Refresh();
            return true;
        }

        public void RefreshAllProgress()
        {
            EnsureContracts();
            foreach (var contract in GetSavedContracts())
            {
                RefreshProgress(contract);
            }

            contractBoardUI?.Refresh();
        }

        public void RefreshProgressForInventoryChange()
        {
            RefreshAllProgress();
        }

        public void RecordEnemyDefeated(string enemyIdOrName)
        {
            EnsureContracts();
            var enemyId = EnemyIds.Normalize(enemyIdOrName);
            var changed = false;
            foreach (var contract in GetSavedContracts())
            {
                if ((contract.status == ContractStatus.Accepted || contract.status == ContractStatus.ReadyToTurnIn) && contract.HasEnemyObjective && contract.requiredEnemyId == enemyId)
                {
                    contract.currentEnemyKillCount = Mathf.Min(contract.requiredEnemyKillCount, contract.currentEnemyKillCount + 1);
                    RefreshProgress(contract);
                    changed = true;
                }
            }

            if (changed)
            {
                contractBoardUI?.Refresh();
            }
        }

        public void RecordCraftedItem(string itemId, int quantity)
        {
            if (quantity <= 0)
            {
                return;
            }

            EnsureContracts();
            var normalizedItemId = ItemIds.Normalize(itemId);
            var changed = false;
            foreach (var contract in GetSavedContracts())
            {
                if ((contract.status == ContractStatus.Accepted || contract.status == ContractStatus.ReadyToTurnIn) && contract.HasCraftingObjective && contract.requiredCraftedItemId == normalizedItemId)
                {
                    contract.currentCraftedItemQuantity = Mathf.Min(contract.requiredCraftedItemQuantity, contract.currentCraftedItemQuantity + quantity);
                    RefreshProgress(contract);
                    changed = true;
                }
            }

            if (changed)
            {
                contractBoardUI?.Refresh();
            }
        }

        public string GetObjectiveText(ContractData contract)
        {
            if (contract == null)
            {
                return string.Empty;
            }

            if (contract.HasItemObjective)
            {
                return $"Turn in {ItemDatabase.GetDisplayName(contract.requiredItemId)}: {Mathf.Min(contract.currentItemQuantity, contract.requiredItemQuantity)}/{contract.requiredItemQuantity}";
            }

            if (contract.HasEnemyObjective)
            {
                return $"Defeat {GetEnemyDisplayName(contract.requiredEnemyId)}: {Mathf.Min(contract.currentEnemyKillCount, contract.requiredEnemyKillCount)}/{contract.requiredEnemyKillCount}";
            }

            if (contract.HasCraftingObjective)
            {
                return $"Craft and turn in {ItemDatabase.GetDisplayName(contract.requiredCraftedItemId)}: {Mathf.Min(contract.currentCraftedItemQuantity, contract.requiredCraftedItemQuantity)}/{contract.requiredCraftedItemQuantity}";
            }

            return "No objective configured.";
        }

        public ContractData GetPlayerContract(string contractId)
        {
            var normalizedId = ContractIds.Normalize(contractId);
            return GetSavedContracts().FirstOrDefault(contract => contract.contractId == normalizedId);
        }

        private void EnsureContracts()
        {
            var data = GetCharacterData();
            if (data == null)
            {
                return;
            }

            data.EnsureContracts();
            foreach (var saved in data.contracts)
            {
                var definition = ContractRegistry.GetDefinition(saved.contractId);
                if (definition != null)
                {
                    saved.ApplyDefinition(definition);
                }
                else
                {
                    saved.Normalize();
                }
            }
        }

        private void RefreshProgress(ContractData contract)
        {
            if (contract == null || contract.status == ContractStatus.Completed || contract.status == ContractStatus.Failed || contract.status == ContractStatus.Abandoned)
            {
                return;
            }

            contract.Normalize();
            var data = GetCharacterData();
            if (data == null)
            {
                return;
            }

            if (contract.HasItemObjective)
            {
                contract.currentItemQuantity = Mathf.Min(contract.requiredItemQuantity, data.GetInventoryQuantity(contract.requiredItemId));
            }

            if (contract.HasCraftingObjective)
            {
                var inventoryQuantity = data.GetInventoryQuantity(contract.requiredCraftedItemId);
                contract.currentCraftedItemQuantity = Mathf.Min(contract.requiredCraftedItemQuantity, Mathf.Max(contract.currentCraftedItemQuantity, inventoryQuantity));
            }

            var wasReady = contract.status == ContractStatus.ReadyToTurnIn;
            contract.status = contract.IsObjectiveReady() ? ContractStatus.ReadyToTurnIn : ContractStatus.Accepted;
            if (!wasReady && contract.status == ContractStatus.ReadyToTurnIn)
            {
                ContractReadyToTurnIn?.Invoke(contract);
                Notify($"Ready to turn in: {contract.title}");
            }
        }

        private static bool ConsumeTurnInItems(CharacterData data, ContractData contract, out string warning)
        {
            warning = string.Empty;
            if (data == null)
            {
                warning = "Character data is not ready.";
                return false;
            }

            if (contract.HasItemObjective && !data.RemoveInventoryItem(contract.requiredItemId, contract.requiredItemQuantity))
            {
                warning = $"Missing {ItemDatabase.GetDisplayName(contract.requiredItemId)} x{contract.requiredItemQuantity}.";
                return false;
            }

            if (contract.HasCraftingObjective && !data.RemoveInventoryItem(contract.requiredCraftedItemId, contract.requiredCraftedItemQuantity))
            {
                warning = $"Missing {ItemDatabase.GetDisplayName(contract.requiredCraftedItemId)} x{contract.requiredCraftedItemQuantity}.";
                return false;
            }

            return true;
        }

        private void AwardProgression(ContractData contract)
        {
            if (contract.rewardProfessionXP <= 0 || string.IsNullOrWhiteSpace(contract.rewardProfessionId))
            {
                return;
            }

            if (contract.rewardProfessionId == "swordsmanship")
            {
                GameManager.Instance?.GainSkillXP("Swordsmanship", contract.rewardProfessionXP);
                return;
            }

            ProfessionManager.Instance?.AddXP(contract.rewardProfessionId, contract.rewardProfessionXP, $"Completed contract: {contract.title}.");
            var profession = ProfessionManager.Instance?.GetProfession(contract.rewardProfessionId);
            if (profession != null && contract.rewardReputation > 0)
            {
                profession.reputation += contract.rewardReputation;
            }
        }

        private List<ContractData> GetSavedContracts()
        {
            var data = GetCharacterData();
            if (data == null)
            {
                return new List<ContractData>();
            }

            data.EnsureContracts();
            return data.contracts;
        }

        private CharacterData GetCharacterData()
        {
            return GameManager.Instance != null ? GameManager.Instance.CharacterData : null;
        }

        private static string GetEnemyDisplayName(string enemyId)
        {
            return EnemyIds.Normalize(enemyId) == EnemyIds.ForestWolf ? "Forest Wolf" : enemyId;
        }

        private static void Notify(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            NotificationPopupUI.Instance?.Show(message);
            GameManager.Instance?.DialogueUI?.ShowLine(message);
        }
    }
}
