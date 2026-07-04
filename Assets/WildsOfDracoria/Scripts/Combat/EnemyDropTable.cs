using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WildsOfDracoria.Items;

namespace WildsOfDracoria.Combat
{
    [Serializable]
    public class EnemyDrop
    {
        [FormerlySerializedAs("itemName")]
        public string itemId;
        [Range(0f, 1f)] public float dropChance = 0.5f;
        public int minQuantity = 1;
        public int maxQuantity = 1;

        public void Normalize()
        {
            itemId = ItemIds.Normalize(itemId);
        }
    }

    public class RolledDrop
    {
        public string itemId;
        public int quantity;

        public RolledDrop(string itemId, int quantity)
        {
            this.itemId = ItemIds.Normalize(itemId);
            this.quantity = quantity;
        }
    }

    [Serializable]
    public class EnemyDropTable
    {
        public List<EnemyDrop> drops = new List<EnemyDrop>();

        public List<RolledDrop> RollDrops()
        {
            var rolledDrops = new List<RolledDrop>();
            foreach (var drop in drops)
            {
                if (drop == null)
                {
                    continue;
                }

                drop.Normalize();
                if (string.IsNullOrWhiteSpace(drop.itemId) || !ItemDatabase.Exists(drop.itemId))
                {
                    continue;
                }

                if (UnityEngine.Random.value > drop.dropChance)
                {
                    continue;
                }

                var quantity = UnityEngine.Random.Range(drop.minQuantity, drop.maxQuantity + 1);
                rolledDrops.Add(new RolledDrop(drop.itemId, Mathf.Max(1, quantity)));
            }

            return rolledDrops;
        }
    }
}
