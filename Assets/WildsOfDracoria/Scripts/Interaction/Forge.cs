using WildsOfDracoria.Player;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.Interaction
{
    public class Forge : InteractableBase
    {
        public override void Interact(PlayerInteractor interactor)
        {
            GameManager.Instance.DialogueUI?.ShowLine("The Ironhaven forge is hot, but blacksmithing is not built yet.");
        }
    }
}
