using System.Collections;
using System.Text;
using UnityEngine;
using WildsOfDracoria.Interaction;
using WildsOfDracoria.Items;
using WildsOfDracoria.Player;
using WildsOfDracoria.Professions;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI.Mobile;

namespace WildsOfDracoria.Gathering
{
    public class GatheringNode : MonoBehaviour, IInteractable
    {
        [SerializeField] private string nodeId = GatheringNodeRegistry.CopperVein;
        [SerializeField] private GatheringNodeDefinition overrideDefinition;
        [SerializeField] private GameObject visualRoot;

        private GatheringNodeDefinition definition;
        private bool isGathering;
        private bool isDepleted;

        public string InteractionLabel
        {
            get
            {
                var node = GetDefinition();
                if (node == null)
                {
                    return "Gather";
                }

                var profession = ProfessionRegistry.GetDefinition(node.requiredProfessionId);
                var professionName = profession != null ? profession.displayName : ProfessionIds.Normalize(node.requiredProfessionId);
                return $"{node.displayName} - {professionName} Lv {node.requiredProfessionLevel}";
            }
        }

        public void Configure(string gatheringNodeId)
        {
            nodeId = gatheringNodeId;
            definition = null;
        }

        public void Interact(PlayerInteractor interactor)
        {
            if (isGathering)
            {
                ShowFeedback("Already gathering.");
                return;
            }

            if (isDepleted)
            {
                ShowFeedback($"{GetDefinition()?.displayName ?? "Node"} is depleted.");
                return;
            }

            var node = GetDefinition();
            if (node == null)
            {
                ShowFeedback("This node is not configured.");
                return;
            }

            if (!CanGather(node, out var warning))
            {
                ShowFeedback(warning);
                return;
            }

            StartCoroutine(GatherRoutine(node));
        }

        private IEnumerator GatherRoutine(GatheringNodeDefinition node)
        {
            isGathering = true;
            ShowFeedback($"Gathering {node.displayName}... {node.gatherTimeSeconds:0.#}s");
            yield return new WaitForSeconds(Mathf.Max(0f, node.gatherTimeSeconds));

            var rewardsText = AwardLoot(node);
            ProfessionManager.Instance?.AddXP(node.requiredProfessionId, node.xpReward, $"Gathered {node.displayName}.");
            ShowFeedback(string.IsNullOrWhiteSpace(rewardsText) ? $"Gathered {node.displayName}." : $"Gathered {rewardsText}");
            isGathering = false;

            if (node.depletionBehavior == GatheringDepletionBehavior.DepleteAndRespawn)
            {
                StartCoroutine(RespawnRoutine(node));
            }
        }

        private IEnumerator RespawnRoutine(GatheringNodeDefinition node)
        {
            SetDepleted(true);
            yield return new WaitForSeconds(Mathf.Max(0f, node.respawnTimeSeconds));
            SetDepleted(false);
            ShowNotificationOnly($"{node.displayName} has respawned.");
        }

        private bool CanGather(GatheringNodeDefinition node, out string warning)
        {
            warning = string.Empty;
            var data = GameManager.Instance?.CharacterData;
            if (data == null)
            {
                warning = "Character data is not ready.";
                return false;
            }

            var profession = ProfessionManager.Instance?.GetProfession(node.requiredProfessionId);
            var professionName = profession != null ? profession.displayName : ProfessionIds.Normalize(node.requiredProfessionId);
            if (profession == null)
            {
                warning = $"Requires {professionName}.";
                return false;
            }

            if (profession.level < node.requiredProfessionLevel)
            {
                warning = $"Requires {professionName} level {node.requiredProfessionLevel}.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(node.requiredToolItemId) && !data.HasInventoryItem(node.requiredToolItemId, 1))
            {
                warning = $"Requires {ItemDatabase.GetDisplayName(node.requiredToolItemId)}.";
                return false;
            }

            return true;
        }

        private string AwardLoot(GatheringNodeDefinition node)
        {
            var builder = new StringBuilder();
            foreach (var loot in node.lootTable)
            {
                loot.Normalize();
                if (string.IsNullOrWhiteSpace(loot.itemId) || !ItemDatabase.Exists(loot.itemId))
                {
                    continue;
                }

                if (Random.value > loot.dropChance)
                {
                    continue;
                }

                var quantity = Random.Range(loot.minQuantity, loot.maxQuantity + 1);
                GameManager.Instance.AddItem(loot.itemId, quantity);
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(ItemDatabase.GetDisplayName(loot.itemId));
                builder.Append(" x");
                builder.Append(quantity);
            }

            return builder.ToString();
        }

        private GatheringNodeDefinition GetDefinition()
        {
            if (overrideDefinition != null)
            {
                overrideDefinition.Normalize();
                return overrideDefinition;
            }

            if (definition == null)
            {
                definition = GatheringNodeRegistry.Get(nodeId);
            }

            return definition;
        }

        private void SetDepleted(bool depleted)
        {
            isDepleted = depleted;
            if (visualRoot != null)
            {
                visualRoot.SetActive(!depleted);
            }
            else
            {
                var renderer = GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = !depleted;
                }
            }
        }

        private static void ShowFeedback(string message)
        {
            GameManager.Instance.DialogueUI?.ShowLine(message);
            NotificationPopupUI.Instance?.Show(message);
        }

        private static void ShowNotificationOnly(string message)
        {
            NotificationPopupUI.Instance?.Show(message);
        }
    }
}