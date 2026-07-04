using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.Data;
using WildsOfDracoria.Professions;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.UI
{
    public class CharacterSheetUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text characterText;

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void Start()
        {
            GameManager.Instance?.RegisterCharacterSheetUI(this);
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
                Refresh(GameManager.Instance?.CharacterData);
            }
        }

        public void Refresh(CharacterData data)
        {
            if (characterText == null || data == null)
            {
                return;
            }

            var profession = data.GetProfession(data.currentProfessionFocus);
            var professionName = profession != null ? profession.displayName : ProfessionIds.Normalize(data.currentProfessionFocus);

            var builder = new StringBuilder();
            builder.AppendLine("Character");
            builder.AppendLine($"Name: {data.characterName} {data.familyName}");
            builder.AppendLine($"House: House {data.familyName}");
            builder.AppendLine($"Race: {data.race}");
            builder.AppendLine($"Homeland: {data.startingHomeland}");
            builder.AppendLine($"Gold: {data.gold}");
            builder.AppendLine($"Profession Focus: {professionName}");
            builder.AppendLine();
            builder.AppendLine("Appearance");
            builder.AppendLine($"Body: {data.bodyType}");
            builder.AppendLine($"Skin: {data.skinTone}");
            builder.AppendLine($"Hair: {data.hairStyle}, {data.hairColor}");
            builder.AppendLine($"Facial Hair: {data.facialHairStyle}");
            builder.AppendLine($"Eyes: {data.eyeColor}");

            characterText.text = builder.ToString();
        }
    }
}