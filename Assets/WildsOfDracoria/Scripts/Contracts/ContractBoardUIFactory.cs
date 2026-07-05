using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using UnityEngine.UI;

namespace WildsOfDracoria.Contracts
{
    public static class ContractBoardUIFactory
    {
        public static ContractBoardUI Create()
        {
            EnsureEventSystem();
            var canvas = FindOrCreateCanvas();
            var panel = CreatePanel(canvas.transform, "Contract Board Panel", new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(760f, 560f), new Color(0.05f, 0.06f, 0.07f, 0.94f));
            var title = CreateText(panel.transform, "Contract Board Title", "Ironhaven Contract Board", 22, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(18f, -52f), new Vector2(-18f, -12f));
            var available = CreateText(panel.transform, "Available Contracts Text", "Available", 14, TextAnchor.UpperLeft, new Vector2(0f, 0.35f), new Vector2(0.48f, 0.92f), new Vector2(18f, 8f), new Vector2(-8f, -58f));
            var journal = CreateText(panel.transform, "Contract Journal Text", "Journal", 14, TextAnchor.UpperLeft, new Vector2(0.5f, 0.35f), new Vector2(1f, 0.92f), new Vector2(8f, 8f), new Vector2(-18f, -58f));
            var selected = CreateText(panel.transform, "Selected Contract Text", "Selected Contract", 14, TextAnchor.UpperLeft, new Vector2(0f, 0.1f), new Vector2(1f, 0.34f), new Vector2(18f, 8f), new Vector2(-18f, -8f));

            var nextAvailable = CreateButton(panel.transform, "Next Available Contract Button", "Next Posted", new Vector2(0f, 0f), new Vector2(18f, 18f), new Vector2(126f, 36f));
            var accept = CreateButton(panel.transform, "Accept Contract Button", "Accept", new Vector2(0f, 0f), new Vector2(154f, 18f), new Vector2(100f, 36f));
            var nextAccepted = CreateButton(panel.transform, "Next Accepted Contract Button", "Next Journal", new Vector2(0f, 0f), new Vector2(272f, 18f), new Vector2(126f, 36f));
            var complete = CreateButton(panel.transform, "Turn In Contract Button", "Turn In", new Vector2(0f, 0f), new Vector2(408f, 18f), new Vector2(100f, 36f));
            var close = CreateButton(panel.transform, "Close Contract Board Button", "Close", new Vector2(1f, 0f), new Vector2(-118f, 18f), new Vector2(100f, 36f));

            var ui = panel.AddComponent<ContractBoardUI>();
            ui.Configure(panel, title, available, journal, selected, nextAvailable, accept, nextAccepted, complete, close);
            panel.SetActive(false);
            ContractManager.Instance?.RegisterUI(ui);
            return ui;
        }

        private static Canvas FindOrCreateCanvas()
        {
            var canvas = Object.FindAnyObjectByType<Canvas>(FindObjectsInactive.Include);
            if (canvas != null)
            {
                return canvas;
            }

            var canvasObject = new GameObject("Prototype UI");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static void EnsureEventSystem()
        {
            if (Object.FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include) != null)
            {
                return;
            }

            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
            eventSystem.AddComponent<InputSystemUIInputModule>();
#else
            eventSystem.AddComponent<StandaloneInputModule>();
#endif
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchor, Vector2 anchoredPosition, Vector2 size, Color color)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            var image = panel.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = true;
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
            return panel;
        }

        private static Text CreateText(Transform parent, string name, string value, int size, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var text = textObject.AddComponent<Text>();
            text.text = value;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = size;
            text.alignment = alignment;
            text.color = Color.white;
            var rect = text.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            return text;
        }

        private static Button CreateButton(Transform parent, string name, string label, Vector2 anchor, Vector2 position, Vector2 size)
        {
            var buttonObject = CreatePanel(parent, name, anchor, position, size, new Color(0.12f, 0.15f, 0.18f, 0.95f));
            CreateText(buttonObject.transform, "Label", label, 14, TextAnchor.MiddleCenter, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return buttonObject.AddComponent<Button>();
        }
    }
}
