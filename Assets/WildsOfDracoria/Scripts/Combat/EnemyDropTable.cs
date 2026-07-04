using System;
using System.Collections.Generic;
using UnityEngine;

namespace WildsOfDracoria.Combat
{
    [Serializable]
    public class EnemyDrop
    {
        public string itemName;
        [Range(0f, 1f)] public float dropChance = 0.5f;
        public int minQuantity = 1;
        public int maxQuantity = 1;
    }

    public class RolledDrop
    {
        public string itemName;
        public int quantity;

        public RolledDrop(string itemName, int quantity)
        {
            this.itemName = itemName;
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
                if (drop == null || string.IsNullOrWhiteSpace(drop.itemName))
                {
                    continue;
                }

                if (UnityEngine.Random.value > drop.dropChance)
                {
                    continue;
                }

                var quantity = UnityEngine.Random.Range(drop.minQuantity, drop.maxQuantity + 1);
                rolledDrops.Add(new RolledDrop(drop.itemName, Mathf.Max(1, quantity)));
            }

            return rolledDrops;
        }
    }
}
