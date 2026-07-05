using UnityEngine;
using WildsOfDracoria.Combat;
using WildsOfDracoria.Inputs;

namespace WildsOfDracoria.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonPlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float sprintStaminaCostPerSecond = 12f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -24f;

        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private PlayerVitals vitals;

        private CharacterController characterController;
        private Vector2 moveInput;
        private Vector2 externalMoveInput;
        private float verticalVelocity;
        private bool runInput;
        private bool externalRunInput;
        private bool jumpRequested;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            interactor = interactor != null ? interactor : GetComponent<PlayerInteractor>();
            vitals = vitals != null ? vitals : GetComponent<PlayerVitals>();

            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            ReadKeyboardInput();
            Move();
        }

        public void SetMoveInput(Vector2 input)
        {
            externalMoveInput = Vector2.ClampMagnitude(input, 1f);
        }

        public void SetRunInput(bool isRunning)
        {
            externalRunInput = isRunning;
        }

        public void Jump()
        {
            jumpRequested = true;
        }

        public void Interact()
        {
            interactor?.InteractWithFocusedObject();
        }

        private void ReadKeyboardInput()
        {
            var keyboardInput = DracoriaInput.GetMoveVector();
            moveInput = keyboardInput.sqrMagnitude > 0.001f ? Vector2.ClampMagnitude(keyboardInput, 1f) : externalMoveInput;
            runInput = DracoriaInput.GetKey(KeyCode.LeftShift) || externalRunInput;

            if (DracoriaInput.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (DracoriaInput.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        private void Move()
        {
            var cameraForward = cameraTransform != null ? cameraTransform.forward : Vector3.forward;
            var cameraRight = cameraTransform != null ? cameraTransform.right : Vector3.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            var moveDirection = (cameraForward * moveInput.y) + (cameraRight * moveInput.x);
            moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

            if (moveDirection.sqrMagnitude > 0.001f)
            {
                var targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (characterController.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            if (jumpRequested && characterController.isGrounded)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            jumpRequested = false;
            verticalVelocity += gravity * Time.deltaTime;

            var wantsToSprint = runInput && moveDirection.sqrMagnitude > 0.001f;
            var canSprint = wantsToSprint && (vitals == null || vitals.TrySpendStamina(sprintStaminaCostPerSecond * Time.deltaTime));
            var speed = canSprint ? runSpeed : walkSpeed;
            var velocity = (moveDirection * speed) + (Vector3.up * verticalVelocity);
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
