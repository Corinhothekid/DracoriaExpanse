#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.Professions;

namespace WildsOfDracoria.EditorTools
{
    public static class ProfessionFrameworkSceneBuilder
    {
        [MenuItem("Wilds of Dracoria/Add Profession Framework To Current Scene")]
        public static void AddProfessionFrameworkToCurrentScene()
        {
            EnsureProfessionManager();
            var canvas = FindOrCreateCanvas();
            var existing = GameObject.Find("Profession Panel");
            if (existing != null)
            {
                Object.DestroyImmediate(existing);
            }

            var panel = CreatePanel(canvas.transform, "Profession Panel", new Vector2(0f, 1f), new Vector2(18f, -84f), new Vector2(430f, 520f));
            var text = CreateText(panel.transform, "Profession Text", "Professions", 15, TextAnchor.UpperLeft);
            var professionUI = panel.AddComponent<ProfessionUI>();
            SetSerializedField(professionUI, "panel", panel);
            SetSerializedField(professionUI, "professionText", text);
            panel.SetActive(false);

            var manager = Object.FindAnyObjectByType<ProfessionManager>();
            manager?.RegisterUI(professionUI);
            Selection.activeObject = panel;
        }

        private static void EnsureProfessionManager()
        {
            var manager = Object.FindAnyObjectByType<ProfessionManager>();
            if (manager != null)
            {
                return;
            }

            var gameManager = GameObject.Find("GameManager");
            if (gameManager == null)
            {
                gameManager = new GameObject("GameManager");
            }

            gameManager.AddComponent<ProfessionManager>();
        }

        private static Canvas FindOrCreateCanvas()
        {
            var canvas = Object.FindAnyObjectByType<Canvas>();
            if (canvas != null)
            {
                return canvas;
            }

            var canvasObject = new GameObject("Prototype UI");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchor, Vector2 anchoredPosition, Vector2 size)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            var image = panel.AddComponent<Image>();
            image.color = new Color(0.05f, 0.06f, 0.07f, 0.88f);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
            return panel;
        }

        private static Text CreateText(Transform parent, string name, string text, int size, TextAnchor alignment)
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
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(12f, 12f);
            rect.offsetMax = new Vector2(-12f, -12f);
            return uiText;
        }

        private static void SetSerializedField(Object target, string fieldName, Object value)
        {
            var serializedObject = new SerializedObject(target);
            serializedObject.FindProperty(fieldName).objectReferenceValue = value;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
