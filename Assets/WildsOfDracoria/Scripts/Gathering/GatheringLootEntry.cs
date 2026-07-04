using System;
using UnityEngine;
using WildsOfDracoria.Items;

namespace WildsOfDracoria.Gathering
{
    [Serializable]
    public class GatheringLootEntry
    {
        public string itemId;
        public int minQuantity = 1;
        public int maxQuantity = 1;
        [Range(0f, 1f)] public float dropChance = 1f;

        public GatheringLootEntry()
        {
        }

        public GatheringLootEntry(string itemId, int minQuantity, int maxQuantity, float dropChance = 1f)
        {
            this.itemId = ItemIds.Normalize(itemId);
            this.minQuantity = minQuantity;
            this.maxQuantity = maxQuantity;
            this.dropChance = dropChance;
        }

        public void Normalize()
        {
            itemId = ItemIds.Normalize(itemId);
            minQuantity = Mathf.Max(1, minQuantity);
            maxQuantity = Mathf.Max(minQuantity, maxQuantity);
        }
    }
}