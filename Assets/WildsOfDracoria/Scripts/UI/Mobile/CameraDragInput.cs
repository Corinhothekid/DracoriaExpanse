using UnityEngine;
using UnityEngine.EventSystems;
using WildsOfDracoria.CameraRig;

namespace WildsOfDracoria.UI.Mobile
{
    public class CameraDragInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private ThirdPersonCameraFollow cameraFollow;
        [SerializeField] private float dragSensitivity = 0.12f;

        private bool isDragging;

        private void Start()
        {
            if (cameraFollow == null && Camera.main != null)
            {
                cameraFollow = Camera.main.GetComponent<ThirdPersonCameraFollow>();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging || cameraFollow == null)
            {
                return;
            }

            cameraFollow.AddLookInput(eventData.delta * dragSensitivity);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
        }
    }
}
