using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WildsOfDracoria.UI.Mobile
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField] private float handleRange = 52f;
        [SerializeField] private float deadZone = 0.08f;

        public Vector2 Direction { get; private set; }

        private Camera uiCamera;

        private void Awake()
        {
            if (background == null)
            {
                background = GetComponent<RectTransform>();
            }

            if (handle == null)
            {
                var handleObject = new GameObject("Handle");
                handleObject.transform.SetParent(transform, false);
                handle = handleObject.AddComponent<RectTransform>();
                handle.sizeDelta = new Vector2(72f, 72f);
                var image = handleObject.AddComponent<Image>();
                image.color = new Color(1f, 1f, 1f, 0.55f);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (background == null)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, uiCamera, out var localPoint);
            var radius = Mathf.Max(1f, background.sizeDelta.x * 0.5f);
            var normalized = Vector2.ClampMagnitude(localPoint / radius, 1f);
            Direction = normalized.magnitude < deadZone ? Vector2.zero : normalized;

            if (handle != null)
            {
                handle.anchoredPosition = Direction * handleRange;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Direction = Vector2.zero;
            if (handle != null)
            {
                handle.anchoredPosition = Vector2.zero;
            }
        }
    }
}
