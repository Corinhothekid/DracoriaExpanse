using System;
using System.Collections.Generic;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.Gathering
{
    [Serializable]
    public class GatheringNodeDefinition
    {
        public string nodeId;
        public string displayName;
        public string requiredProfessionId;
        public int requiredProfessionLevel = 1;
        public string requiredToolItemId;
        public float gatherTimeSeconds = 2f;
        public float respawnTimeSeconds = 15f;
        public List<GatheringLootEntry> lootTable = new List<GatheringLootEntry>();
        public int xpReward = 10;
        public GatheringDepletionBehavior depletionBehavior = GatheringDepletionBehavior.DepleteAndRespawn;

        public GatheringNodeDefinition()
        {
        }

        public GatheringNodeDefinition(string nodeId, string displayName, string requiredProfessionId, int requiredProfessionLevel, string requiredToolItemId, float gatherTimeSeconds, float respawnTimeSeconds, int xpReward, GatheringDepletionBehavior depletionBehavior)
        {
            this.nodeId = nodeId;
            this.displayName = displayName;
            this.requiredProfessionId = ProfessionIds.Normalize(requiredProfessionId);
            this.requiredProfessionLevel = requiredProfessionLevel;
            this.requiredToolItemId = requiredToolItemId;
            this.gatherTimeSeconds = gatherTimeSeconds;
            this.respawnTimeSeconds = respawnTimeSeconds;
            this.xpReward = xpReward;
            this.depletionBehavior = depletionBehavior;
        }

        public void Normalize()
        {
            requiredProfessionId = ProfessionIds.Normalize(requiredProfessionId);
            foreach (var loot in lootTable)
            {
                loot.Normalize();
            }
        }
    }
}