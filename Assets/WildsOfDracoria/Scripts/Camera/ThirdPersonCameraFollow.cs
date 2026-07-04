using UnityEngine;

namespace WildsOfDracoria.CameraRig
{
    public class ThirdPersonCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 4f, -6f);
        [SerializeField] private float followSharpness = 10f;
        [SerializeField] private float lookHeight = 1.4f;
        [SerializeField] private float yaw;
        [SerializeField] private float pitch = 18f;
        [SerializeField] private float minPitch = 8f;
        [SerializeField] private float maxPitch = 45f;
        [SerializeField] private float mouseLookSensitivity = 2.5f;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void AddLookInput(Vector2 lookDelta)
        {
            yaw += lookDelta.x;
            pitch = Mathf.Clamp(pitch - lookDelta.y, minPitch, maxPitch);
        }

        private void Update()
        {
            if (Input.GetMouseButton(2))
            {
                AddLookInput(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseLookSensitivity);
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var rotation = Quaternion.Euler(pitch, target.eulerAngles.y + yaw, 0f);
            var desiredPosition = target.position + rotation * offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSharpness * Time.deltaTime);
            transform.LookAt(target.position + Vector3.up * lookHeight);
        }
    }
}
