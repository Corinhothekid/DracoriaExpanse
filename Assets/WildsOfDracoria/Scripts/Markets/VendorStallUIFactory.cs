using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using UnityEngine.UI;

namespace WildsOfDracoria.Markets
{
    public static class VendorStallUIFactory
    {
        public static VendorStallUI Create()
        {
            var canvas = Object.FindAnyObjectByType<Canvas>(FindObjectsInactive.Include);
            if (canvas == null)
            {
                var canvasObject = new GameObject("Prototype UI Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasObject.AddComponent<GraphicRaycaster>();
            }

            EnsureEventSystem();

            var panel = new GameObject("Vendor Stall Panel");
            panel.transform.SetParent(canvas.transform, false);
            var panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.08f, 0.12f);
            panelRect.anchorMax = new Vector2(0.92f, 0.88f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            var image = panel.AddComponent<Image>();
            image.color = new Color(0.08f, 0.07f, 0.05f, 0.92f);

            var title = CreateText(panel.transform, "Stall Name", 24, TextAnchor.UpperLeft, new Vector2(0.04f, 0.84f), new Vector2(0.96f, 0.96f));
            var owner = CreateText(panel.transform, "Owner", 16, TextAnchor.UpperLeft, new Vector2(0.04f, 0.77f), new Vector2(0.96f, 0.84f));
            var gold = CreateText(panel.transform, "Gold", 16, TextAnchor.UpperRight, new Vector2(0.04f, 0.77f), new Vector2(0.96f, 0.84f));
            var listings = CreateText(panel.transform, "Listings", 18, TextAnchor.UpperLeft, new Vector2(0.04f, 0.30f), new Vector2(0.96f, 0.73f));
            var selected = CreateText(panel.transform, "Selected", 16, TextAnchor.UpperLeft, new Vector2(0.04f, 0.20f), new Vector2(0.96f, 0.29f));

            var previous = CreateButton(panel.transform, "Prev", new Vector2(0.04f, 0.06f), new Vector2(0.22f, 0.16f));
            var next = CreateButton(panel.transform, "Next", new Vector2(0.25f, 0.06f), new Vector2(0.43f, 0.16f));
            var buy = CreateButton(panel.transform, "Buy", new Vector2(0.50f, 0.06f), new Vector2(0.68f, 0.16f));
            var close = CreateButton(panel.transform, "Close", new Vector2(0.74f, 0.06f), new Vector2(0.94f, 0.16f));

            var ui = panel.AddComponent<VendorStallUI>();
            ui.Configure(panel, title, owner, gold, listings, selected, previous, next, buy, close);
            MarketManager.Instance?.RegisterUI(ui);
            panel.SetActive(false);
            return ui;
        }

        private static Text CreateText(Transform parent, string name, int fontSize, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = new Color(0.96f, 0.91f, 0.78f);
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        private static Button CreateButton(Transform parent, string label, Vector2 anchorMin, Vector2 anchorMax)
        {
            var buttonObject = new GameObject(label + " Button");
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.27f, 0.20f, 0.12f, 0.95f);
            var button = buttonObject.AddComponent<Button>();
            var text = CreateText(buttonObject.transform, label + " Label", 16, TextAnchor.MiddleCenter, Vector2.zero, Vector2.one);
            text.text = label;
            text.color = Color.white;
            return button;
        }

        private static void EnsureEventSystem()
        {
            if (Object.FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include) != null)
            {
                return;
            }

            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
#else
            eventSystemObject.AddComponent<StandaloneInputModule>();
#endif
        }
    }
}
