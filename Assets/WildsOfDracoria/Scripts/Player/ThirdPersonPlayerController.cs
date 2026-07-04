using UnityEngine;
using WildsOfDracoria.Combat;

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
        private float verticalVelocity;
        private bool runInput;
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
            moveInput = Vector2.ClampMagnitude(input, 1f);
        }

        public void SetRunInput(bool isRunning)
        {
            runInput = isRunning;
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
            var keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            SetMoveInput(keyboardInput.sqrMagnitude > 0.001f ? keyboardInput : Vector2.zero);
            SetRunInput(Input.GetKey(KeyCode.LeftShift));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.E))
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
            var canSprint = vitals == null || vitals.TrySpendStamina(sprintStaminaCostPerSecond * Time.deltaTime);
            var speed = wantsToSprint && canSprint ? runSpeed : walkSpeed;
            var velocity = (moveDirection * speed) + (Vector3.up * verticalVelocity);
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
