using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.Combat;

namespace WildsOfDracoria.UI
{
    public class EnemyWorldHealthBar : MonoBehaviour
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private Slider slider;

        private void Awake()
        {
            if (enemyHealth == null)
            {
                enemyHealth = GetComponentInParent<EnemyHealth>();
            }
        }

        private void Update()
        {
            if (enemyHealth == null || slider == null)
            {
                return;
            }

            slider.maxValue = enemyHealth.MaxHealth;
            slider.value = enemyHealth.CurrentHealth;
            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
            }
        }
    }
}
