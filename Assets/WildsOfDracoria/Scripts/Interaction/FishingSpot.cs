using System.Collections;
using UnityEngine;
using WildsOfDracoria.Player;
using WildsOfDracoria.Professions;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.Interaction
{
    public class FishingSpot : InteractableBase
    {
        [SerializeField] private float catchTimeSeconds = 3f;
        [SerializeField] private int fishingXPReward = 25;

        private readonly string[] catchTable =
        {
            "Small Silverfin",
            "River Trout",
            "Old Boot",
            "Rare Golden Carp"
        };

        private bool isFishing;

        public override void Interact(PlayerInteractor interactor)
        {
            if (isFishing)
            {
                return;
            }

            StartCoroutine(FishRoutine());
        }

        private IEnumerator FishRoutine()
        {
            isFishing = true;
            GameManager.Instance.DialogueUI?.ShowLine("You cast your line into the harbor water...");

            yield return new WaitForSeconds(catchTimeSeconds);

            var caughtItem = catchTable[Random.Range(0, catchTable.Length)];
            GameManager.Instance.AddItem(caughtItem);
            ProfessionManager.Instance?.AddXP(ProfessionIds.Fishing, fishingXPReward, $"Caught {caughtItem} at Ironhaven.");
            GameManager.Instance.DialogueUI?.ShowLine($"You caught: {caughtItem}");

            isFishing = false;
        }
    }
}
