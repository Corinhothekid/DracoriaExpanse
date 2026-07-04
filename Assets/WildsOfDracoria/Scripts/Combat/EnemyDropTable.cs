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

    [Serializable]
    public class EnemyDropTable
    {
        public List<EnemyDrop> drops = new List<EnemyDrop>();

        public IEnumerable<(string itemName, int quantity)> RollDrops()
        {
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
                yield return (drop.itemName, Mathf.Max(1, quantity));
            }
        }
    }
}
