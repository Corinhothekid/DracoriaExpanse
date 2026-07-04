using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.Data;
using WildsOfDracoria.Items;
using WildsOfDracoria.Systems;

namespace WildsOfDracoria.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text inventoryText;

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void Start()
        {
            GameManager.Instance?.RegisterInventoryUI(this);
        }

        public void Toggle()
        {
            if (panel != null)
            {
                panel.SetActive(!panel.activeSelf);
            }
        }

        public void Refresh(CharacterData data)
        {
            if (inventoryText == null || data == null)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("Inventory");
            builder.AppendLine($"Gold: {data.gold}");
            builder.AppendLine();

            if (data.inventory.Count == 0)
            {
                builder.AppendLine("Empty");
            }
            else
            {
                foreach (var item in data.inventory)
                {
                    item.Normalize();
                    builder.AppendLine($"{ItemDatabase.GetDisplayName(item.itemId)} x{item.quantity}");
                }
            }

            inventoryText.text = builder.ToString();
        }
    }
}
