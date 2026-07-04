using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WildsOfDracoria.CharacterCreation
{
    public static class CharacterCreationUIFactory
    {
        public static CharacterCreationUI Create()
        {
            EnsureEventSystem();
            var canvasObject = new GameObject("Character Creation UI");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var panel = CreatePanel(canvasObject.transform, "Character Creation Panel", new Vector2(0.5f, 0.5f), new Vector2(860f, 620f), Vector2.zero, new Color(0.05f, 0.06f, 0.07f, 0.94f));
            var title = CreateText(panel.transform, "Step Title", "Character Creation", 28, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(28f, -72f), new Vector2(-28f, -20f));
            var primary = CreateText(panel.transform, "Primary Text", "Race", 24, TextAnchor.MiddleLeft, new Vector2(0.38f, 0.68f), new Vector2(1f, 0.86f), new Vector2(18f, 0f), new Vector2(-28f, 0f));
            var secondary = CreateText(panel.transform, "Secondary Text", "", 17, TextAnchor.UpperLeft, new Vector2(0.38f, 0.26f), new Vector2(1f, 0.68f), new Vector2(18f, 0f), new Vector2(-28f, -4f));
            var finalName = CreateText(panel.transform, "Final Name Preview", "Corey Kranert\nHouse Kranert", 20, TextAnchor.MiddleCenter, new Vector2(0f, 0.72f), new Vector2(0.36f, 0.92f), new Vector2(24f, 0f), new Vector2(-8f, 0f));

            var preview = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            preview.name = "Race Preview Capsule";
            preview.transform.SetParent(panel.transform, false);
            preview.transform.localPosition = new Vector3(-245f, -24f, 0f);
            preview.transform.localScale = new Vector3(70f, 120f, 70f);
            var previewRenderer = preview.GetComponent<Renderer>();
            Object.Destroy(preview.GetComponent<Collider>());

            var firstNameInput = CreateInput(panel.transform, "Character Name Input", "Character First Name", new Vector2(0.38f, 0.55f), new Vector2(1f, 0.64f), new Vector2(18f, 0f), new Vector2(-28f, 0f));
            var familyInput = CreateInput(panel.transform, "Family Name Input", "Family / House Name", new Vector2(0.38f, 0.42f), new Vector2(1f, 0.51f), new Vector2(18f, 0f), new Vector2(-28f, 0f));

            var previous = CreateButton(panel.transform, "Previous Step Button", "Back", new Vector2(0f, 0f), new Vector2(24f, 24f), new Vector2(110f, 42f));
            var next = CreateButton(panel.transform, "Next Step Button", "Next", new Vector2(1f, 0f), new Vector2(-134f, 24f), new Vector2(110f, 42f));
            var confirm = CreateButton(panel.transform, "Confirm Character Button", "Confirm", new Vector2(1f, 0f), new Vector2(-154f, 24f), new Vector2(130f, 42f));
            var optionA = CreateButton(panel.transform, "Option A Button", "Previous", new Vector2(0.38f, 0.16f), new Vector2(18f, 0f), new Vector2(150f, 42f));
            var optionB = CreateButton(panel.transform, "Option B Button", "Next", new Vector2(0.38f, 0.16f), new Vector2(178f, 0f), new Vector2(150f, 42f));
            var optionC = CreateButton(panel.transform, "Option C Button", "Use Race Home", new Vector2(0.38f, 0.16f), new Vector2(338f, 0f), new Vector2(170f, 42f));

            var ui = panel.AddComponent<CharacterCreationUI>();
            ui.Configure(panel, title, primary, secondary, finalName, firstNameInput, familyInput, previous, next, optionA, optionB, optionC, confirm, GetButtonLabel(optionA), GetButtonLabel(optionB), GetButtonLabel(optionC), previewRenderer, preview.transform);
            return ui;
        }

        private static void EnsureEventSystem()
        {
            if (Object.FindObjectOfType<EventSystem>() != null)
            {
                return;
            }

            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchor, Vector2 size, Vector2 position, Color color)
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
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            return panel;
        }

        private static Text CreateText(Transform parent, string name, string text, int size, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var uiText = textObject.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            uiText.fontSize = size;
            uiText.alignment = alignment;
            uiText.color = Color.white;
            var rect = uiText.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            return uiText;
        }

        private static InputField CreateInput(Transform parent, string name, string placeholder, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            var inputObject = CreatePanel(parent, name, Vector2.zero, Vector2.zero, Vector2.zero, new Color(0.1f, 0.12f, 0.14f, 0.95f));
            var rect = inputObject.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            var text = CreateText(inputObject.transform, "Text", "", 18, TextAnchor.MiddleLeft, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-12f, 0f));
            var placeholderText = CreateText(inputObject.transform, "Placeholder", placeholder, 18, TextAnchor.MiddleLeft, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-12f, 0f));
            placeholderText.color = new Color(1f, 1f, 1f, 0.45f);

            var input = inputObject.AddComponent<InputField>();
            input.textComponent = text;
            input.placeholder = placeholderText;
            return input;
        }

        private static Button CreateButton(Transform parent, string name, string label, Vector2 anchor, Vector2 position, Vector2 size)
        {
            var buttonObject = CreatePanel(parent, name, anchor, size, position, new Color(0.12f, 0.15f, 0.18f, 0.95f));
            CreateText(buttonObject.transform, "Label", label, 15, TextAnchor.MiddleCenter, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return buttonObject.AddComponent<Button>();
        }

        private static Text GetButtonLabel(Button button)
        {
            return button.GetComponentInChildren<Text>();
        }
    }
}