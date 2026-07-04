using UnityEngine;
using WildsOfDracoria.Player;

namespace WildsOfDracoria.Interaction
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactionLabel = "Interact";

        public string InteractionLabel => interactionLabel;

        public abstract void Interact(PlayerInteractor interactor);
    }
}
