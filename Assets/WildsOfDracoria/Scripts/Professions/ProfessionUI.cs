using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WildsOfDracoria.Professions
{
    public class ProfessionUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text professionText;

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void Start()
        {
            ProfessionManager.Instance?.RegisterUI(this);
            Refresh();
        }

        public void Toggle()
        {
            if (panel != null)
            {
                panel.SetActive(!panel.activeSelf);
                if (panel.activeSelf)
                {
                    Refresh();
                }
            }
        }

        public void Show()
        {
            if (panel != null)
            {
                panel.SetActive(true);
            }
            Refresh();
        }

        public void Refresh()
        {
            if (professionText == null || ProfessionManager.Instance == null)
            {
                return;
            }

            ProfessionManager.Instance.InitializeProfessions();
            var builder = new StringBuilder();
            builder.AppendLine("Professions");
            builder.AppendLine();

            foreach (var definition in ProfessionRegistry.All)
            {
                var profession = ProfessionManager.Instance.GetProfession(definition.professionName);
                var locked = profession == null || !profession.isUnlocked;
                var state = locked ? "Locked" : $"Level {profession.level} - {profession.masteryRank}";
                var xp = locked ? "XP: --" : $"XP: {profession.currentXP}/{profession.xpRequired}";
                var reputation = locked ? "Rep: --" : $"Rep: {profession.reputation}";
                builder.AppendLine($"{definition.professionName} ({definition.family})");
                builder.AppendLine($"  {state} | {xp} | {reputation}");
            }

            professionText.text = builder.ToString();
        }
    }
}
