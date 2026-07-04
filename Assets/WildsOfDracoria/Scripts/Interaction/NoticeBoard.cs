using WildsOfDracoria.Player;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.Interaction
{
    public class NoticeBoard : InteractableBase
    {
        public override void Interact(PlayerInteractor interactor)
        {
            GameManager.Instance.DialogueUI?.ShowLine("Ironhaven Notices: Fishers wanted. Dockhands needed. Guild charters coming soon.");
        }
    }
}
