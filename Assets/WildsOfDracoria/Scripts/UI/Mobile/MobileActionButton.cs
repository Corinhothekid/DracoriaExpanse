using UnityEngine;
using UnityEngine.EventSystems;

namespace WildsOfDracoria.UI.Mobile
{
    public class MobileActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private MobileControlAction action;
        [SerializeField] private MobileControlsRouter router;

        public void Configure(MobileControlsRouter newRouter, MobileControlAction newAction)
        {
            router = newRouter;
            action = newAction;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            router?.HandleButtonDown(action);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            router?.HandleButtonUp(action);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            router?.HandleButtonClick(action);
        }
    }
}
