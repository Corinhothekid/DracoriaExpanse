using WildsOfDracoria.Contracts;
using WildsOfDracoria.Player;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.Interaction
{
    public class NoticeBoard : InteractableBase
    {
        public override void Interact(PlayerInteractor interactor)
        {
            if (ContractManager.Instance != null)
            {
                ContractManager.Instance.OpenBoard();
                GameManager.Instance.DialogueUI?.ShowLine("Ironhaven contracts are posted on the board.");
                return;
            }

            GameManager.Instance.DialogueUI?.ShowLine("Ironhaven Notices: Fishers wanted. Dockhands needed. Guild charters coming soon.");
        }
    }
}
