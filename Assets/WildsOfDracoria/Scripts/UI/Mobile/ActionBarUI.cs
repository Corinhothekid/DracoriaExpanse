using UnityEngine;
using UnityEngine.UI;

namespace WildsOfDracoria.UI.Mobile
{
    public class ActionBarUI : MonoBehaviour
    {
        [SerializeField] private Text[] slotLabels = new Text[6];

        public int SlotCount => slotLabels != null ? slotLabels.Length : 0;

        private void Awake()
        {
            RefreshEmptySlots();
        }

        public void SetSlotLabel(int index, string label)
        {
            if (slotLabels == null || index < 0 || index >= slotLabels.Length || slotLabels[index] == null)
            {
                return;
            }

            slotLabels[index].text = string.IsNullOrWhiteSpace(label) ? "Empty" : label;
        }

        public void RefreshEmptySlots()
        {
            if (slotLabels == null)
            {
                return;
            }

            for (var i = 0; i < slotLabels.Length; i++)
            {
                if (slotLabels[i] != null && string.IsNullOrWhiteSpace(slotLabels[i].text))
                {
                    slotLabels[i].text = "Empty";
                }
            }
        }
    }
}
