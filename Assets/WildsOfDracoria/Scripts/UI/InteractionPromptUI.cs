using UnityEngine;
using UnityEngine.UI;

namespace WildsOfDracoria.UI
{
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text promptText;

        public void Show(string label)
        {
            if (promptText != null)
            {
                promptText.text = $"E - {label}";
            }

            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        public void Hide()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }
}
