#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using UnityEngine.UI;
using WildsOfDracoria.Player;
using WildsOfDracoria.UI.Mobile;

namespace WildsOfDracoria.EditorTools
{
    public static class MobileControlsSceneBuilder
    {
        [MenuItem("Wilds of Dracoria/Add Mobile Controls To Current Scene")]
        public static void AddMobileControlsToCurrentScene()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("No Player object found. Build Ironhaven first, then add mobile controls.");
                return;
            }

            var existing = GameObject.Find("Mobile Controls UI");
            if (existing != null)
            {
                Object.DestroyImmediate(existing);
            }

            EnsureEventSystem();
            var canvasObject = CreateCanvas();
            var router = canvasObject.AddComponent<MobileControlsRouter>();
            canvasObject.AddComponent<MobileControlsBootstrap>();

            CreateCameraDragArea(canvasObject.transform);
            var joystick = CreateJoystick(canvasObject.transform);
            SetSerializedField(router, "movementJoystick", joystick);

            CreateCombatButtons(canvasObject.transform, router);
            CreateMenuButtons(canvasObject.transform, router);
            CreateActionBar(canvasObject.transform);
            var notifications = CreateNotificationPanel(canvasObject.transform);
            SetSerializedField(router, "notificationPopupUI", notifications);

            Selection.activeObject = canvasObject;
        }

        private static GameObject CreateCanvas()
        {
            var canvasObject = new GameObject("Mobile Controls UI");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();
            return canvasObject;
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

        private static void CreateCameraDragArea(Transform parent)
        {
            var area = CreatePanel(parent, "Camera Drag Area", new Vector2(0f, 0f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.001f));
            area.transform.SetAsFirstSibling();
            area.AddComponent<CameraDragInput>();
        }

        private static VirtualJoystick CreateJoystick(Transform parent)
        {
            var joystickObject = CreatePanel(parent, "Movement Joystick", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(170f, 170f), new Vector2(120f, 110f), new Color(0.05f, 0.06f, 0.07f, 0.45f));
            var handle = CreatePanel(joystickObject.transform, "Handle", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(72f, 72f), Vector2.zero, new Color(1f, 1f, 1f, 0.55f));
            var joystick = joystickObject.AddComponent<VirtualJoystick>();
            SetSerializedField(joystick, "background", joystickObject.GetComponent<RectTransform>());
            SetSerializedField(joystick, "handle", handle.GetComponent<RectTransform>());
            return joystick;
        }

        private static void CreateCombatButtons(Transform parent, MobileControlsRouter router)
        {
            CreateButton(parent, "Attack Button", "ATK", new Vector2(1f, 0f), new Vector2(-145f, 150f), new Vector2(96f, 96f), router, MobileControlAction.Attack);
            CreateButton(parent, "Block Button", "BLK", new Vector2(1f, 0f), new Vector2(-260f, 145f), new Vector2(82f, 82f), router, MobileControlAction.Block);
            CreateButton(parent, "Dodge Button", "DOD", new Vector2(1f, 0f), new Vector2(-145f, 270f), new Vector2(82f, 82f), router, MobileControlAction.Dodge);
            CreateButton(parent, "Sprint Button", "RUN", new Vector2(1f, 0f), new Vector2(-270f, 260f), new Vector2(82f, 82f), router, MobileControlAction.Sprint);
            CreateButton(parent, "Interact Button", "USE", new Vector2(1f, 0f), new Vector2(-390f, 145f), new Vector2(82f, 82f), router, MobileControlAction.Interact);
        }

        private static void CreateMenuButtons(Transform parent, MobileControlsRouter router)
        {
            CreateButton(parent, "Inventory Menu Button", "INV", new Vector2(1f, 1f), new Vector2(-330f, -42f), new Vector2(74f, 44f), router, MobileControlAction.Inventory);
            CreateButton(parent, "Skills Menu Button", "SKL", new Vector2(1f, 1f), new Vector2(-246f, -42f), new Vector2(74f, 44f), router, MobileControlAction.Skills);
            CreateButton(parent, "Map Menu Button", "MAP", new Vector2(1f, 1f), new Vector2(-162f, -42f), new Vector2(74f, 44f), router, MobileControlAction.Map);
            CreateButton(parent, "Character Menu Button", "CHR", new Vector2(1f, 1f), new Vector2(-78f, -42f), new Vector2(74f, 44f), router, MobileControlAction.Character);
        }

        private static void CreateActionBar(Transform parent)
        {
            var bar = CreatePanel(parent, "Action Bar", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(456f, 64f), new Vector2(-228f, 18f), new Color(0.05f, 0.06f, 0.07f, 0.5f));
            var actionBar = bar.AddComponent<ActionBarUI>();
            var labels = new Text[6];

            for (var i = 0; i < labels.Length; i++)
            {
                var slot = CreatePanel(bar.transform, $"Action Slot {i + 1}", new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(64f, 52f), new Vector2(8f + i * 74f, -26f), new Color(0.02f, 0.02f, 0.025f, 0.72f));
                labels[i] = CreateText(slot.transform, "Label", "Empty", 12, TextAnchor.MiddleCenter);
            }

            var serializedObject = new SerializedObject(actionBar);
            var slotLabels = serializedObject.FindProperty("slotLabels");
            slotLabels.arraySize = labels.Length;
            for (var i = 0; i < labels.Length; i++)
            {
                slotLabels.GetArrayElementAtIndex(i).objectReferenceValue = labels[i];
            }
            serializedObject.ApplyModifiedProperties();
        }

        private static NotificationPopupUI CreateNotificationPanel(Transform parent)
        {
            var panel = CreatePanel(parent, "Notification Popup", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(360f, 46f), new Vector2(-180f, -88f), new Color(0.05f, 0.06f, 0.07f, 0.82f));
            var text = CreateText(panel.transform, "Message", "", 16, TextAnchor.MiddleCenter);
            var popup = panel.AddComponent<NotificationPopupUI>();
            SetSerializedField(popup, "panel", panel);
            SetSerializedField(popup, "messageText", text);
            return popup;
        }

        private static GameObject CreateButton(Transform parent, string name, string label, Vector2 anchor, Vector2 position, Vector2 size, MobileControlsRouter router, MobileControlAction action)
        {
            var buttonObject = CreatePanel(parent, name, anchor, anchor, size, position, new Color(0.08f, 0.1f, 0.12f, 0.72f));
            CreateText(buttonObject.transform, "Label", label, 15, TextAnchor.MiddleCenter);
            buttonObject.AddComponent<Button>().transition = Selectable.Transition.ColorTint;
            var actionButton = buttonObject.AddComponent<MobileActionButton>();
            actionButton.Configure(router, action);
            return buttonObject;
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Color color)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            var image = panel.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = true;
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = anchorMin == anchorMax ? anchorMin : new Vector2(0.5f, 0.5f);
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
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
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
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
