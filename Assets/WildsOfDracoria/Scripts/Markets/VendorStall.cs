using UnityEngine;
using WildsOfDracoria.Interaction;
using WildsOfDracoria.Player;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.Markets
{
    public class VendorStall : MonoBehaviour, IInteractable
    {
        [SerializeField] private string stallId = MarketIds.IronhavenFishStall;

        public string InteractionLabel
        {
            get
            {
                var stall = MarketManager.Instance != null ? MarketManager.Instance.GetStall(stallId) : MarketRegistry.GetDefinition(stallId);
                return stall != null ? $"Shop: {stall.stallName}" : "Shop";
            }
        }

        public void Configure(string newStallId)
        {
            stallId = MarketIds.Normalize(newStallId);
        }

        public void Interact(PlayerInteractor interactor)
        {
            if (MarketManager.Instance == null)
            {
                GameManager.Instance?.DialogueUI?.ShowLine("The market is not ready yet.");
                return;
            }

            MarketManager.Instance.OpenStall(stallId);
        }
    }
}
