using UnityEngine;
using WildsOfDracoria.Player;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.Interaction
{
    public class NPCDialogue : InteractableBase
    {
        [SerializeField] private string npcName = "Captain Alden";
        [TextArea]
        [SerializeField] private string dialogueLine = "Welcome to Ironhaven. The sea has a way of finding those who are meant for more.";

        public override void Interact(PlayerInteractor interactor)
        {
            GameManager.Instance.DialogueUI?.ShowLine($"{npcName}: {dialogueLine}");
        }
    }
}
