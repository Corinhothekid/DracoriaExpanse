using UnityEngine;

namespace WildsOfDracoria.UI.Mobile
{
    public class MobileControlsBootstrap : MonoBehaviour
    {
        [SerializeField] private MobileControlsRouter router;

        private void Start()
        {
            if (router == null)
            {
                router = Object.FindAnyObjectByType<MobileControlsRouter>();
            }

            router?.ShowPrototypeNotice("Mobile controls ready");
        }
    }
}
