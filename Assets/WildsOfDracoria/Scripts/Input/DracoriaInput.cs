using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace WildsOfDracoria.Inputs
{
    public static class DracoriaInput
    {
        public static Vector2 GetMoveVector()
        {
            return new Vector2(GetAxisRaw("Horizontal"), GetAxisRaw("Vertical"));
        }

        public static float GetAxisRaw(string axisName)
        {
#if ENABLE_INPUT_SYSTEM
            return Mathf.Clamp(GetDigitalAxis(axisName), -1f, 1f);
#else
            return Input.GetAxisRaw(axisName);
#endif
        }

        public static float GetAxis(string axisName)
        {
#if ENABLE_INPUT_SYSTEM
            if (axisName == "Mouse X" || axisName == "Mouse Y")
            {
                return GetMouseDeltaAxis(axisName);
            }

            return GetAxisRaw(axisName);
#else
            return Input.GetAxis(axisName);
#endif
        }

        public static bool GetKey(KeyCode keyCode)
        {
#if ENABLE_INPUT_SYSTEM
            var key = MapKey(keyCode);
            return key != null && key.isPressed;
#else
            return Input.GetKey(keyCode);
#endif
        }

        public static bool GetKeyDown(KeyCode keyCode)
        {
#if ENABLE_INPUT_SYSTEM
            var key = MapKey(keyCode);
            return key != null && key.wasPressedThisFrame;
#else
            return Input.GetKeyDown(keyCode);
#endif
        }

        public static bool GetMouseButton(int button)
        {
#if ENABLE_INPUT_SYSTEM
            var mouseButton = MapMouseButton(button);
            return mouseButton != null && mouseButton.isPressed;
#else
            return Input.GetMouseButton(button);
#endif
        }

        public static bool GetMouseButtonDown(int button)
        {
#if ENABLE_INPUT_SYSTEM
            var mouseButton = MapMouseButton(button);
            return mouseButton != null && mouseButton.wasPressedThisFrame;
#else
            return Input.GetMouseButtonDown(button);
#endif
        }

#if ENABLE_INPUT_SYSTEM
        private static float GetDigitalAxis(string axisName)
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return 0f;
            }

            switch (axisName)
            {
                case "Horizontal":
                    return ReadPositiveNegative(keyboard.dKey, keyboard.rightArrowKey, keyboard.aKey, keyboard.leftArrowKey);
                case "Vertical":
                    return ReadPositiveNegative(keyboard.wKey, keyboard.upArrowKey, keyboard.sKey, keyboard.downArrowKey);
                default:
                    return 0f;
            }
        }

        private static float GetMouseDeltaAxis(string axisName)
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                return 0f;
            }

            var delta = mouse.delta.ReadValue();
            return axisName == "Mouse Y" ? delta.y : delta.x;
        }

        private static float ReadPositiveNegative(ButtonControl positiveA, ButtonControl positiveB, ButtonControl negativeA, ButtonControl negativeB)
        {
            var value = 0f;
            if (positiveA.isPressed || positiveB.isPressed)
            {
                value += 1f;
            }

            if (negativeA.isPressed || negativeB.isPressed)
            {
                value -= 1f;
            }

            return value;
        }

        private static KeyControl MapKey(KeyCode keyCode)
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return null;
            }

            switch (keyCode)
            {
                case KeyCode.E: return keyboard.eKey;
                case KeyCode.Escape: return keyboard.escapeKey;
                case KeyCode.F5: return keyboard.f5Key;
                case KeyCode.F9: return keyboard.f9Key;
                case KeyCode.I: return keyboard.iKey;
                case KeyCode.LeftAlt: return keyboard.leftAltKey;
                case KeyCode.LeftShift: return keyboard.leftShiftKey;
                case KeyCode.Space: return keyboard.spaceKey;
                default: return null;
            }
        }

        private static ButtonControl MapMouseButton(int button)
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                return null;
            }

            switch (button)
            {
                case 0: return mouse.leftButton;
                case 1: return mouse.rightButton;
                case 2: return mouse.middleButton;
                default: return null;
            }
        }
#endif
    }
}
