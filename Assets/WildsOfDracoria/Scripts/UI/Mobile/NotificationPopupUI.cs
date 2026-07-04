using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WildsOfDracoria.UI.Mobile
{
    public class NotificationPopupUI : MonoBehaviour
    {
        public static NotificationPopupUI Instance { get; private set; }

        [SerializeField] private GameObject panel;
        [SerializeField] private Text messageText;
        [SerializeField] private float visibleSeconds = 2.25f;

        private readonly Queue<string> queue = new Queue<string>();
        private float hideAtTime;

        private void Awake()
        {
            Instance = this;
            Hide();
        }

        private void Update()
        {
            if (panel != null && panel.activeSelf && Time.time >= hideAtTime)
            {
                if (queue.Count > 0)
                {
                    ShowImmediate(queue.Dequeue());
                }
                else
                {
                    Hide();
                }
            }
        }

        public void Show(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (panel != null && panel.activeSelf)
            {
                queue.Enqueue(message);
                return;
            }

            ShowImmediate(message);
        }

        private void ShowImmediate(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
            }

            if (panel != null)
            {
                panel.SetActive(true);
            }

            hideAtTime = Time.time + visibleSeconds;
        }

        private void Hide()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }
}
