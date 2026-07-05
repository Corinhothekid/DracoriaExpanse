using System;
using WildsOfDracoria.Items;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.Contracts
{
    [Serializable]
    public class ContractData
    {
        public string contractId;
        public string title;
        public string description;
        public ContractType contractType;
        public string giverName;
        public string locationName;
        public string requiredItemId;
        public int requiredItemQuantity;
        public string requiredEnemyId;
        public int requiredEnemyKillCount;
        public string requiredCraftedItemId;
        public int requiredCraftedItemQuantity;
        public int rewardGold;
        public string rewardProfessionId;
        public int rewardProfessionXP;
        public int rewardReputation;
        public ContractStatus status;
        public int deadlineDays;

        public int currentItemQuantity;
        public int currentEnemyKillCount;
        public int currentCraftedItemQuantity;

        public ContractData()
        {
        }

        public ContractData(string contractId, string title, ContractType contractType, string giverName, string locationName)
        {
            this.contractId = ContractIds.Normalize(contractId);
            this.title = title;
            this.contractType = contractType;
            this.giverName = giverName;
            this.locationName = locationName;
            status = ContractStatus.Available;
        }

        public ContractData Clone()
        {
            return new ContractData
            {
                contractId = contractId,
                title = title,
                description = description,
                contractType = contractType,
                giverName = giverName,
                locationName = locationName,
                requiredItemId = requiredItemId,
                requiredItemQuantity = requiredItemQuantity,
                requiredEnemyId = requiredEnemyId,
                requiredEnemyKillCount = requiredEnemyKillCount,
                requiredCraftedItemId = requiredCraftedItemId,
                requiredCraftedItemQuantity = requiredCraftedItemQuantity,
                rewardGold = rewardGold,
                rewardProfessionId = rewardProfessionId,
                rewardProfessionXP = rewardProfessionXP,
                rewardReputation = rewardReputation,
                status = status,
                deadlineDays = deadlineDays,
                currentItemQuantity = currentItemQuantity,
                currentEnemyKillCount = currentEnemyKillCount,
                currentCraftedItemQuantity = currentCraftedItemQuantity
            };
        }

        public void ApplyDefinition(ContractData definition)
        {
            if (definition == null)
            {
                return;
            }

            var savedStatus = status;
            var savedItemProgress = currentItemQuantity;
            var savedEnemyProgress = currentEnemyKillCount;
            var savedCraftedProgress = currentCraftedItemQuantity;
            CopyDefinitionFields(definition);
            status = savedStatus == ContractStatus.Available ? definition.status : savedStatus;
            currentItemQuantity = savedItemProgress;
            currentEnemyKillCount = savedEnemyProgress;
            currentCraftedItemQuantity = savedCraftedProgress;
            Normalize();
        }

        public void Normalize()
        {
            contractId = ContractIds.Normalize(contractId);
            requiredItemId = ItemIds.Normalize(requiredItemId);
            requiredCraftedItemId = ItemIds.Normalize(requiredCraftedItemId);
            requiredEnemyId = EnemyIds.Normalize(requiredEnemyId);
            rewardProfessionId = ProfessionIds.Normalize(rewardProfessionId);
            requiredItemQuantity = Math.Max(0, requiredItemQuantity);
            requiredEnemyKillCount = Math.Max(0, requiredEnemyKillCount);
            requiredCraftedItemQuantity = Math.Max(0, requiredCraftedItemQuantity);
            rewardGold = Math.Max(0, rewardGold);
            rewardProfessionXP = Math.Max(0, rewardProfessionXP);
            rewardReputation = Math.Max(0, rewardReputation);
            deadlineDays = Math.Max(0, deadlineDays);
        }

        public bool HasItemObjective => !string.IsNullOrWhiteSpace(requiredItemId) && requiredItemQuantity > 0;
        public bool HasEnemyObjective => !string.IsNullOrWhiteSpace(requiredEnemyId) && requiredEnemyKillCount > 0;
        public bool HasCraftingObjective => !string.IsNullOrWhiteSpace(requiredCraftedItemId) && requiredCraftedItemQuantity > 0;

        public bool IsObjectiveReady()
        {
            var itemReady = !HasItemObjective || currentItemQuantity >= requiredItemQuantity;
            var enemyReady = !HasEnemyObjective || currentEnemyKillCount >= requiredEnemyKillCount;
            var craftedReady = !HasCraftingObjective || currentCraftedItemQuantity >= requiredCraftedItemQuantity;
            return itemReady && enemyReady && craftedReady;
        }

        private void CopyDefinitionFields(ContractData definition)
        {
            contractId = definition.contractId;
            title = definition.title;
            description = definition.description;
            contractType = definition.contractType;
            giverName = definition.giverName;
            locationName = definition.locationName;
            requiredItemId = definition.requiredItemId;
            requiredItemQuantity = definition.requiredItemQuantity;
            requiredEnemyId = definition.requiredEnemyId;
            requiredEnemyKillCount = definition.requiredEnemyKillCount;
            requiredCraftedItemId = definition.requiredCraftedItemId;
            requiredCraftedItemQuantity = definition.requiredCraftedItemQuantity;
            rewardGold = definition.rewardGold;
            rewardProfessionId = definition.rewardProfessionId;
            rewardProfessionXP = definition.rewardProfessionXP;
            rewardReputation = definition.rewardReputation;
            deadlineDays = definition.deadlineDays;
        }
    }
}
