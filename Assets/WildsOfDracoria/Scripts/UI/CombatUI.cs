using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.Combat;

namespace WildsOfDracoria.UI
{
    public class CombatUI : MonoBehaviour
    {
        public static CombatUI Instance { get; private set; }

        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider staminaSlider;
        [SerializeField] private GameObject enemyPanel;
        [SerializeField] private Text enemyNameText;
        [SerializeField] private Slider enemyHealthSlider;

        private IDamageable target;

        private void Awake()
        {
            Instance = this;
            if (enemyPanel != null)
            {
                enemyPanel.SetActive(false);
            }
        }

        public void SetPlayerVitals(int currentHealth, int maxHealth, float currentStamina, float maxStamina)
        {
            SetSlider(healthSlider, currentHealth, maxHealth);
            SetSlider(staminaSlider, currentStamina, maxStamina);
        }

        public void SetTarget(IDamageable newTarget)
        {
            target = newTarget;
            RefreshTarget();
        }

        public void ClearTarget(IDamageable clearedTarget)
        {
            if (target != clearedTarget)
            {
                return;
            }

            target = null;
            if (enemyPanel != null)
            {
                enemyPanel.SetActive(false);
            }
        }

        private void Update()
        {
            RefreshTarget();
        }

        private void RefreshTarget()
        {
            if (target == null || target.IsDead())
            {
                if (enemyPanel != null)
                {
                    enemyPanel.SetActive(false);
                }
                return;
            }

            if (target is EnemyHealth enemy)
            {
                if (enemyPanel != null)
                {
                    enemyPanel.SetActive(true);
                }

                if (enemyNameText != null)
                {
                    enemyNameText.text = enemy.EnemyName;
                }

                SetSlider(enemyHealthSlider, enemy.CurrentHealth, enemy.MaxHealth);
            }
        }

        private static void SetSlider(Slider slider, float current, float max)
        {
            if (slider == null)
            {
                return;
            }

            slider.minValue = 0f;
            slider.maxValue = Mathf.Max(1f, max);
            slider.value = Mathf.Clamp(current, 0f, slider.maxValue);
        }
    }
}
