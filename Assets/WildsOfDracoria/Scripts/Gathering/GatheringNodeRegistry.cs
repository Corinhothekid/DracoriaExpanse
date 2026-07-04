using System.Collections.Generic;
using System.Linq;
using WildsOfDracoria.Items;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.Gathering
{
    public static class GatheringNodeRegistry
    {
        public const string CopperVein = "copper_vein";
        public const string OakTree = "oak_tree";
        public const string WheatPatch = "wheat_patch";

        private static readonly List<GatheringNodeDefinition> Definitions = new List<GatheringNodeDefinition>
        {
            new GatheringNodeDefinition(CopperVein, "Copper Vein", ProfessionIds.Mining, 1, ItemIds.BeginnerPickaxe, 2.5f, 18f, 18, GatheringDepletionBehavior.DepleteAndRespawn)
            {
                lootTable = new List<GatheringLootEntry>
                {
                    new GatheringLootEntry(ItemIds.CopperOre, 1, 3)
                }
            },
            new GatheringNodeDefinition(OakTree, "Oak Tree", ProfessionIds.Logging, 1, string.Empty, 2f, 16f, 15, GatheringDepletionBehavior.DepleteAndRespawn)
            {
                lootTable = new List<GatheringLootEntry>
                {
                    new GatheringLootEntry(ItemIds.OakLog, 1, 2)
                }
            },
            new GatheringNodeDefinition(WheatPatch, "Wheat Patch", ProfessionIds.Farming, 1, string.Empty, 1.75f, 14f, 12, GatheringDepletionBehavior.DepleteAndRespawn)
            {
                lootTable = new List<GatheringLootEntry>
                {
                    new GatheringLootEntry(ItemIds.Wheat, 2, 4)
                }
            }
        };

        public static IReadOnlyList<GatheringNodeDefinition> All => Definitions;

        public static GatheringNodeDefinition Get(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                return null;
            }

            var normalizedId = nodeId.Trim().ToLowerInvariant().Replace(" ", "_");
            var definition = Definitions.FirstOrDefault(node => node.nodeId == normalizedId);
            definition?.Normalize();
            return definition;
        }
    }
}