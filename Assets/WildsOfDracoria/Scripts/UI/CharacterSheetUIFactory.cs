using UnityEngine;
using UnityEngine.UI;

namespace WildsOfDracoria.UI
{
    public static class CharacterSheetUIFactory
    {
        public static CharacterSheetUI Create()
        {
            var canvasObject = new GameObject("Character Sheet UI");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var panel = new GameObject("Character Sheet Panel");
            panel.transform.SetParent(canvasObject.transform, false);
            var image = panel.AddComponent<Image>();
            image.color = new Color(0.05f, 0.06f, 0.07f, 0.88f);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(-18f, -388f);
            rect.sizeDelta = new Vector2(360f, 330f);

            var textObject = new GameObject("Character Sheet Text");
            textObject.transform.SetParent(panel.transform, false);
            var text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 15;
            text.alignment = TextAnchor.UpperLeft;
            text.color = Color.white;
            var textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(14f, 12f);
            textRect.offsetMax = new Vector2(-14f, -12f);

            var ui = panel.AddComponent<CharacterSheetUI>();
            ui.Configure(panel, text);
            panel.SetActive(false);
            return ui;
        }
    }
}