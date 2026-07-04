using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text dialogueText;
        [SerializeField] private float autoHideSeconds = 5f;

        private float hideAtTime;

        private void Awake()
        {
            Hide();
        }

        private void Start()
        {
            GameManager.Instance?.RegisterDialogueUI(this);
        }

        private void Update()
        {
            if (panel != null && panel.activeSelf && Time.time >= hideAtTime)
            {
                Hide();
            }
        }

        public void ShowLine(string line)
        {
            if (dialogueText != null)
            {
                dialogueText.text = line;
            }

            if (panel != null)
            {
                panel.SetActive(true);
            }

            hideAtTime = Time.time + autoHideSeconds;
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
