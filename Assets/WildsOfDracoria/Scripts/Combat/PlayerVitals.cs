using UnityEngine;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI;

namespace WildsOfDracoria.Combat
{
    public class PlayerVitals : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaRegenPerSecond = 16f;
        [SerializeField] private float regenDelayAfterSpend = 0.75f;

        private int currentHealth;
        private float currentStamina;
        private float nextRegenTime;
        private PlayerCombat combat;

        public int MaxHealth => maxHealth;
        public int CurrentHealth => currentHealth;
        public float MaxStamina => maxStamina;
        public float CurrentStamina => currentStamina;

        private void Awake()
        {
            combat = GetComponent<PlayerCombat>();
            var data = GameManager.Instance?.CharacterData;
            maxHealth = data != null ? data.maxHealth : maxHealth;
            maxStamina = data != null ? data.maxStamina : maxStamina;
            currentHealth = data != null && data.currentHealth > 0 ? data.currentHealth : maxHealth;
            currentStamina = data != null && data.currentStamina > 0 ? data.currentStamina : maxStamina;
            SyncToCharacterData();
        }

        private void Update()
        {
            RegenerateStamina();
            CombatUI.Instance?.SetPlayerVitals(currentHealth, maxHealth, currentStamina, maxStamina);
        }

        public bool CanSpendStamina(float amount)
        {
            return !IsDead() && currentStamina >= amount;
        }

        public bool TrySpendStamina(float amount)
        {
            if (!CanSpendStamina(amount))
            {
                return false;
            }

            currentStamina -= amount;
            nextRegenTime = Time.time + regenDelayAfterSpend;
            SyncToCharacterData();
            return true;
        }

        public void TakeDamage(int amount)
        {
            if (IsDead() || amount <= 0)
            {
                return;
            }

            if (combat != null && combat.IsBlocking)
            {
                amount = Mathf.CeilToInt(amount * 0.4f);
                TrySpendStamina(8f);
            }

            currentHealth = Mathf.Max(0, currentHealth - amount);
            SyncToCharacterData();

            if (currentHealth <= 0)
            {
                GameManager.Instance.DialogueUI?.ShowLine("You were defeated. Rest and try again.");
            }
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }

        private void RegenerateStamina()
        {
            if (Time.time < nextRegenTime || currentStamina >= maxStamina)
            {
                return;
            }

            currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenPerSecond * Time.deltaTime);
            SyncToCharacterData();
        }

        private void SyncToCharacterData()
        {
            var data = GameManager.Instance?.CharacterData;
            if (data == null)
            {
                return;
            }

            data.maxHealth = maxHealth;
            data.currentHealth = currentHealth;
            data.maxStamina = maxStamina;
            data.currentStamina = Mathf.RoundToInt(currentStamina);
            data.health = currentHealth;
            data.stamina = Mathf.RoundToInt(currentStamina);
        }
    }
}
