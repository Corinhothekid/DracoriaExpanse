using UnityEngine;
using WildsOfDracoria.Combat;
using WildsOfDracoria.Player;
using WildsOfDracoria.Professions;
using WildsOfDracoria.UI;

namespace WildsOfDracoria.UI.Mobile
{
    public class MobileControlsRouter : MonoBehaviour
    {
        [SerializeField] private VirtualJoystick movementJoystick;
        [SerializeField] private ThirdPersonPlayerController playerController;
        [SerializeField] private PlayerCombat playerCombat;
        [SerializeField] private InventoryUI inventoryUI;
        [SerializeField] private ProfessionUI professionUI;
        [SerializeField] private CharacterSheetUI characterSheetUI;
        [SerializeField] private NotificationPopupUI notificationPopupUI;

        private bool sprintHeld;
        private bool blockHeld;

        private void Start()
        {
            if (playerController == null)
            {
                playerController = FindObjectOfType<ThirdPersonPlayerController>();
            }

            if (playerCombat == null)
            {
                playerCombat = FindObjectOfType<PlayerCombat>();
            }

            if (inventoryUI == null)
            {
                inventoryUI = FindObjectOfType<InventoryUI>(true);
            }

            if (professionUI == null)
            {
                professionUI = FindObjectOfType<ProfessionUI>(true);
            }

            if (characterSheetUI == null)
            {
                characterSheetUI = FindObjectOfType<CharacterSheetUI>(true);
            }

            if (notificationPopupUI == null)
            {
                notificationPopupUI = NotificationPopupUI.Instance ?? FindObjectOfType<NotificationPopupUI>(true);
            }
        }

        private void Update()
        {
            if (playerController != null && movementJoystick != null)
            {
                playerController.SetMoveInput(movementJoystick.Direction);
                playerController.SetRunInput(sprintHeld);
            }

            playerCombat?.SetBlockInput(blockHeld);
        }

        public void SetJoystick(VirtualJoystick joystick)
        {
            movementJoystick = joystick;
        }

        public void HandleButtonDown(MobileControlAction action)
        {
            switch (action)
            {
                case MobileControlAction.Attack:
                    playerCombat?.BasicAttack();
                    break;
                case MobileControlAction.Block:
                    blockHeld = true;
                    break;
                case MobileControlAction.Dodge:
                    playerCombat?.Dodge();
                    break;
                case MobileControlAction.Sprint:
                    sprintHeld = true;
                    break;
                case MobileControlAction.Interact:
                    playerController?.Interact();
                    break;
            }
        }

        public void HandleButtonUp(MobileControlAction action)
        {
            switch (action)
            {
                case MobileControlAction.Block:
                    blockHeld = false;
                    break;
                case MobileControlAction.Sprint:
                    sprintHeld = false;
                    break;
            }
        }

        public void HandleButtonClick(MobileControlAction action)
        {
            switch (action)
            {
                case MobileControlAction.Inventory:
                    inventoryUI?.Toggle();
                    break;
                case MobileControlAction.Skills:
                    professionUI?.Toggle();
                    break;
                case MobileControlAction.Map:
                    ShowPrototypeNotice("Map placeholder");
                    break;
                case MobileControlAction.Character:
                    characterSheetUI?.Toggle();
                    break;
            }
        }

        public void ShowPrototypeNotice(string message)
        {
            notificationPopupUI?.Show(message);
        }
    }
}