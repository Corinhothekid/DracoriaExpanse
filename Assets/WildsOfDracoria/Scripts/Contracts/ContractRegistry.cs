using System.Collections.Generic;
using System.Linq;
using WildsOfDracoria.Items;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.Contracts
{
    public static class ContractRegistry
    {
        private static readonly List<ContractData> Definitions = new List<ContractData>
        {
            new ContractData(ContractIds.FreshFishForIronhaven, "Fresh Fish for Ironhaven", ContractType.Fishing, "Mara the Cook", "Ironhaven")
            {
                description = "Mara needs fresh fish for the village stew pots. Bring her five Small Silverfin.",
                requiredItemId = ItemIds.SmallSilverfin,
                requiredItemQuantity = 5,
                rewardGold = 18,
                rewardProfessionId = ProfessionIds.Fishing,
                rewardProfessionXP = 30,
                rewardReputation = 2
            },
            new ContractData(ContractIds.TimberForDockRepairs, "Timber for Dock Repairs", ContractType.Gathering, "Dockmaster Rowan", "Ironhaven Docks")
            {
                description = "The harbor crews need sturdy oak for dock patchwork. Gather ten Oak Logs.",
                requiredItemId = ItemIds.OakLog,
                requiredItemQuantity = 10,
                rewardGold = 24,
                rewardProfessionId = ProfessionIds.Logging,
                rewardProfessionXP = 35,
                rewardReputation = 2
            },
            new ContractData(ContractIds.CopperForTheForge, "Copper for the Forge", ContractType.Gathering, "Blacksmith Toren", "Ironhaven Forge")
            {
                description = "Toren is short on starter ore. Bring eight Copper Ore to keep the forge working.",
                requiredItemId = ItemIds.CopperOre,
                requiredItemQuantity = 8,
                rewardGold = 26,
                rewardProfessionId = ProfessionIds.Mining,
                rewardProfessionXP = 40,
                rewardReputation = 2
            },
            new ContractData(ContractIds.WolvesInTheWheat, "Wolves in the Wheat", ContractType.Combat, "Farmer Elric", "Ironhaven Farms")
            {
                description = "Forest Wolves have been stalking the farm paths. Defeat three wolves near Ironhaven.",
                requiredEnemyId = EnemyIds.ForestWolf,
                requiredEnemyKillCount = 3,
                rewardGold = 32,
                rewardProfessionId = "swordsmanship",
                rewardProfessionXP = 35,
                rewardReputation = 3
            },
            new ContractData(ContractIds.ProperTrainingBlade, "A Proper Training Blade", ContractType.Crafting, "Guard Captain Alden", "Ironhaven Guard Post")
            {
                description = "Captain Alden wants a fresh training blade for new recruits. Craft and turn in one Training Sword.",
                requiredCraftedItemId = ItemIds.TrainingSword,
                requiredCraftedItemQuantity = 1,
                rewardGold = 35,
                rewardProfessionId = ProfessionIds.Blacksmithing,
                rewardProfessionXP = 45,
                rewardReputation = 3
            }
        };

        public static IReadOnlyList<ContractData> All => Definitions;

        public static ContractData GetDefinition(string contractId)
        {
            var normalizedId = ContractIds.Normalize(contractId);
            var definition = Definitions.FirstOrDefault(contract => contract.contractId == normalizedId);
            definition?.Normalize();
            return definition;
        }
    }
}
