using UnityEngine;
using UnityEngine.UI;

namespace WildsOfDracoria.UI
{
    public class FloatingDamageText : MonoBehaviour
    {
        [SerializeField] private Text damageText;
        [SerializeField] private float lifetime = 0.9f;
        [SerializeField] private float riseSpeed = 1.4f;

        private float expiresAt;
        private Color startColor = Color.white;

        private void Awake()
        {
            if (damageText == null)
            {
                damageText = GetComponentInChildren<Text>();
            }

            if (damageText != null)
            {
                startColor = damageText.color;
            }
        }

        private void Update()
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            if (damageText != null)
            {
                var remaining = Mathf.InverseLerp(expiresAt - lifetime, expiresAt, Time.time);
                damageText.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0f), remaining);
            }

            if (Time.time >= expiresAt)
            {
                Destroy(gameObject);
            }
        }

        public void Show(int amount)
        {
            if (damageText != null)
            {
                damageText.text = amount.ToString();
            }

            expiresAt = Time.time + lifetime;
        }
    }
}
