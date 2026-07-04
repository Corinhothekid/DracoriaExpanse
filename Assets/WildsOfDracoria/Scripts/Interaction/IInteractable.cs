using WildsOfDracoria.Player;

namespace WildsOfDracoria.Interaction
{
    public interface IInteractable
    {
        string InteractionLabel { get; }
        void Interact(PlayerInteractor interactor);
    }
}
