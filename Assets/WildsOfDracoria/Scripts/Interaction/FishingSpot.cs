using System.Collections;
using UnityEngine;
using WildsOfDracoria.Items;
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
            ItemIds.SmallSilverfin,
            ItemIds.RiverTrout,
            ItemIds.OldBoot,
            ItemIds.RareGoldenCarp
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

            var caughtItemId = catchTable[Random.Range(0, catchTable.Length)];
            var caughtItemName = ItemDatabase.GetDisplayName(caughtItemId);
            GameManager.Instance.AddItem(caughtItemId);
            ProfessionManager.Instance?.AddXP(ProfessionIds.Fishing, fishingXPReward, $"Caught {caughtItemName} at Ironhaven.");
            GameManager.Instance.DialogueUI?.ShowLine($"You caught: {caughtItemName}");

            isFishing = false;
        }
    }
}
