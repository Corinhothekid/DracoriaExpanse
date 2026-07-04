using UnityEngine;
using WildsOfDracoria.Interaction;
using WildsOfDracoria.Player;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.Crafting
{
    public class CraftingStation : InteractableBase
    {
        [SerializeField] private CraftingStationType stationType = CraftingStationType.Forge;

        public CraftingStationType StationType => stationType;

        public void Configure(CraftingStationType type)
        {
            stationType = type;
        }

        public override void Interact(PlayerInteractor interactor)
        {
            if (CraftingManager.Instance == null)
            {
                GameManager.Instance.DialogueUI?.ShowLine("Crafting is not ready yet.");
                return;
            }

            CraftingManager.Instance.OpenStation(stationType);
        }
    }
}