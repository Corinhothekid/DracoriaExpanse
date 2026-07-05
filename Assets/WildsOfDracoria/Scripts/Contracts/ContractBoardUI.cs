using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WildsOfDracoria.Contracts
{
    public class ContractBoardUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text titleText;
        [SerializeField] private Text availableText;
        [SerializeField] private Text journalText;
        [SerializeField] private Text selectedText;
        [SerializeField] private Button nextAvailableButton;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button nextAcceptedButton;
        [SerializeField] private Button completeButton;
        [SerializeField] private Button closeButton;

        private ContractManager manager;
        private IReadOnlyList<ContractData> availableContracts = new List<ContractData>();
        private IReadOnlyList<ContractData> playerContracts = new List<ContractData>();
        private int selectedAvailableIndex;
        private int selectedPlayerIndex;

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            nextAvailableButton?.onClick.AddListener(SelectNextAvailable);
            acceptButton?.onClick.AddListener(AcceptSelected);
            nextAcceptedButton?.onClick.AddListener(SelectNextPlayerContract);
            completeButton?.onClick.AddListener(CompleteSelected);
            closeButton?.onClick.AddListener(Close);
        }

        private void Start()
        {
            ContractManager.Instance?.RegisterUI(this);
        }

        public void Configure(GameObject panelObject, Text title, Text available, Text journal, Text selected, Button nextAvailable, Button accept, Button nextAccepted, Button complete, Button close)
        {
            panel = panelObject;
            titleText = title;
            availableText = available;
            journalText = journal;
            selectedText = selected;
            nextAvailableButton = nextAvailable;
            acceptButton = accept;
            nextAcceptedButton = nextAccepted;
            completeButton = complete;
            closeButton = close;
        }

        public void RegisterManager(ContractManager contractManager)
        {
            manager = contractManager;
            Refresh();
        }

        public void Open()
        {
            if (panel != null)
            {
                panel.SetActive(true);
            }

            Refresh();
        }

        public void Close()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        public void Toggle()
        {
            if (panel == null)
            {
                return;
            }

            panel.SetActive(!panel.activeSelf);
            if (panel.activeSelf)
            {
                Refresh();
            }
        }

        public void Refresh()
        {
            if (manager == null)
            {
                return;
            }

            availableContracts = manager.AvailableContracts;
            playerContracts = manager.PlayerContracts;
            selectedAvailableIndex = ClampIndex(selectedAvailableIndex, availableContracts.Count);
            selectedPlayerIndex = ClampIndex(selectedPlayerIndex, playerContracts.Count);

            if (titleText != null)
            {
                titleText.text = "Ironhaven Contract Board";
            }

            RefreshAvailableText();
            RefreshJournalText();
            RefreshSelectedText();
        }

        private void SelectNextAvailable()
        {
            if (availableContracts.Count == 0)
            {
                return;
            }

            selectedAvailableIndex = (selectedAvailableIndex + 1) % availableContracts.Count;
            RefreshSelectedText();
            RefreshAvailableText();
        }

        private void SelectNextPlayerContract()
        {
            if (playerContracts.Count == 0)
            {
                return;
            }

            selectedPlayerIndex = (selectedPlayerIndex + 1) % playerContracts.Count;
            RefreshSelectedText();
            RefreshJournalText();
        }

        private void AcceptSelected()
        {
            var selected = GetSelectedAvailableContract();
            if (selected == null)
            {
                return;
            }

            manager?.AcceptContract(selected.contractId);
        }

        private void CompleteSelected()
        {
            var selected = GetSelectedPlayerContract();
            if (selected == null)
            {
                return;
            }

            manager?.CompleteContract(selected.contractId);
        }

        private void RefreshAvailableText()
        {
            if (availableText == null)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("Available");
            builder.AppendLine();
            if (availableContracts.Count == 0)
            {
                builder.AppendLine("No new contracts posted.");
            }
            else
            {
                for (var i = 0; i < availableContracts.Count; i++)
                {
                    var marker = i == selectedAvailableIndex ? "> " : "  ";
                    builder.AppendLine($"{marker}{availableContracts[i].title}");
                    builder.AppendLine($"   {availableContracts[i].giverName} - {availableContracts[i].contractType}");
                }
            }

            availableText.text = builder.ToString();
        }

        private void RefreshJournalText()
        {
            if (journalText == null)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("Contract Journal");
            builder.AppendLine();
            if (playerContracts.Count == 0)
            {
                builder.AppendLine("No accepted contracts.");
            }
            else
            {
                for (var i = 0; i < playerContracts.Count; i++)
                {
                    var contract = playerContracts[i];
                    var marker = i == selectedPlayerIndex ? "> " : "  ";
                    builder.AppendLine($"{marker}{contract.title}");
                    builder.AppendLine($"   {contract.status} | {manager.GetObjectiveText(contract)}");
                }
            }

            journalText.text = builder.ToString();
        }

        private void RefreshSelectedText()
        {
            if (selectedText == null)
            {
                return;
            }

            var selectedAvailable = GetSelectedAvailableContract();
            var selectedPlayer = GetSelectedPlayerContract();
            var builder = new StringBuilder();

            if (selectedAvailable != null)
            {
                builder.AppendLine(selectedAvailable.title);
                builder.AppendLine(selectedAvailable.description);
                builder.AppendLine();
                builder.AppendLine($"Giver: {selectedAvailable.giverName}");
                builder.AppendLine($"Location: {selectedAvailable.locationName}");
                builder.AppendLine($"Objective: {manager.GetObjectiveText(selectedAvailable)}");
                builder.AppendLine($"Reward: {selectedAvailable.rewardGold} gold, {selectedAvailable.rewardProfessionXP} XP");
            }
            else
            {
                builder.AppendLine("No available contract selected.");
            }

            builder.AppendLine();
            if (selectedPlayer != null)
            {
                builder.AppendLine("Selected Journal Entry");
                builder.AppendLine(selectedPlayer.title);
                builder.AppendLine(manager.GetObjectiveText(selectedPlayer));
                builder.AppendLine($"Status: {selectedPlayer.status}");
            }

            selectedText.text = builder.ToString();

            if (acceptButton != null)
            {
                acceptButton.interactable = selectedAvailable != null;
            }

            if (completeButton != null)
            {
                completeButton.interactable = selectedPlayer != null && selectedPlayer.status == ContractStatus.ReadyToTurnIn;
            }
        }

        private ContractData GetSelectedAvailableContract()
        {
            if (availableContracts.Count == 0 || selectedAvailableIndex < 0 || selectedAvailableIndex >= availableContracts.Count)
            {
                return null;
            }

            return availableContracts[selectedAvailableIndex];
        }

        private ContractData GetSelectedPlayerContract()
        {
            var activeContracts = playerContracts.Where(contract => contract.status != ContractStatus.Abandoned && contract.status != ContractStatus.Failed).ToList();
            if (activeContracts.Count == 0 || selectedPlayerIndex < 0 || selectedPlayerIndex >= activeContracts.Count)
            {
                return null;
            }

            return activeContracts[selectedPlayerIndex];
        }

        private static int ClampIndex(int index, int count)
        {
            if (count <= 0)
            {
                return 0;
            }

            return Mathf.Clamp(index, 0, count - 1);
        }
    }
}
