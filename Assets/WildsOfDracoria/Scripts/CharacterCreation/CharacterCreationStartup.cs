using UnityEngine;
using WildsOfDracoria.Combat;
using WildsOfDracoria.Data;
using WildsOfDracoria.Player;
using WildsOfDracoria.Save;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.CharacterCreation
{
    public class CharacterCreationStartup : MonoBehaviour
    {
        [SerializeField] private CharacterCreationUI characterCreationUI;
        [SerializeField] private GameObject playerRoot;

        private void Start()
        {
            if (characterCreationUI == null)
            {
                characterCreationUI = Object.FindAnyObjectByType<CharacterCreationUI>(FindObjectsInactive.Include);
            }

            if (characterCreationUI == null)
            {
                characterCreationUI = CharacterCreationUIFactory.Create();
            }

            if (playerRoot == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                playerRoot = player;
            }

            if (JsonSaveSystem.HasSave())
            {
                GameManager.Instance.LoadGame();
                SetPlayerControlEnabled(true);
                characterCreationUI?.Close();
                return;
            }

            SetPlayerControlEnabled(false);
            characterCreationUI?.Open(this);
        }

        public void ConfirmCharacter(CharacterCreationData creationData)
        {
            var data = CharacterData.CreateFromCharacterCreation(creationData);
            GameManager.Instance.SetCharacterData(data, true);
            characterCreationUI?.Close();
            SetPlayerControlEnabled(true);
        }

        private void SetPlayerControlEnabled(bool isEnabled)
        {
            if (playerRoot == null)
            {
                return;
            }

            var movement = playerRoot.GetComponent<ThirdPersonPlayerController>();
            if (movement != null)
            {
                movement.enabled = isEnabled;
            }

            var combat = playerRoot.GetComponent<PlayerCombat>();
            if (combat != null)
            {
                combat.enabled = isEnabled;
            }

            var interactor = playerRoot.GetComponent<PlayerInteractor>();
            if (interactor != null)
            {
                interactor.enabled = isEnabled;
            }
        }
    }
}
