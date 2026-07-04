using UnityEngine;
using WildsOfDracoria.Items;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI;

namespace WildsOfDracoria.Combat
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private string enemyName = "Forest Wolf";
        [SerializeField] private int maxHealth = 45;
        [SerializeField] private int swordsmanshipXPReward = 35;
        [SerializeField] private int enduranceXPReward = 10;
        [SerializeField] private EnemyDropTable dropTable = new EnemyDropTable();
        [SerializeField] private GameObject floatingDamagePrefab;

        private int currentHealth;
        private bool rewardsGranted;

        public string EnemyName => enemyName;
        public int MaxHealth => maxHealth;
        public int CurrentHealth => currentHealth;
        public int SwordsmanshipXPReward => swordsmanshipXPReward;
        public int EnduranceXPReward => enduranceXPReward;

        private void Awake()
        {
            currentHealth = maxHealth;
            if (dropTable.drops.Count == 0)
            {
                dropTable.drops.Add(new EnemyDrop { itemId = ItemIds.WolfPelt, dropChance = 0.65f });
                dropTable.drops.Add(new EnemyDrop { itemId = ItemIds.RawMeat, dropChance = 0.8f });
                dropTable.drops.Add(new EnemyDrop { itemId = ItemIds.SmallFang, dropChance = 0.25f });
            }
        }

        public void TakeDamage(int amount)
        {
            if (IsDead() || amount <= 0)
            {
                return;
            }

            currentHealth = Mathf.Max(0, currentHealth - amount);
            CombatUI.Instance?.SetTarget(this);
            SpawnFloatingDamage(amount);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }

        public void GrantDrops()
        {
            if (rewardsGranted)
            {
                return;
            }

            rewardsGranted = true;
            foreach (var drop in dropTable.RollDrops())
            {
                GameManager.Instance.AddItem(drop.itemId, drop.quantity);
            }
        }

        private void Die()
        {
            GameManager.Instance.DialogueUI?.ShowLine($"Defeated {enemyName}.");
            var player = FindObjectOfType<PlayerCombat>();
            player?.NotifyEnemyDefeated(this);
            gameObject.SetActive(false);
        }

        private void SpawnFloatingDamage(int amount)
        {
            if (floatingDamagePrefab == null)
            {
                return;
            }

            var damageObject = Instantiate(floatingDamagePrefab, transform.position + Vector3.up * 1.8f, Quaternion.identity);
            var floatingText = damageObject.GetComponent<FloatingDamageText>();
            floatingText?.Show(amount);
        }
    }
}
