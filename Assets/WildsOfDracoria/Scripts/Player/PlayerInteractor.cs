using System.Collections.Generic;
using UnityEngine;
using WildsOfDracoria.Interaction;
using WildsOfDracoria.UI;

namespace WildsOfDracoria.Player
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private InteractionPromptUI promptUI;

        private readonly List<IInteractable> nearbyInteractables = new List<IInteractable>();
        private IInteractable focusedInteractable;

        private void Awake()
        {
            var sphere = GetComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = Mathf.Max(sphere.radius, 2.25f);
        }

        private void Update()
        {
            UpdateFocus();
        }

        public void InteractWithFocusedObject()
        {
            focusedInteractable?.Interact(this);
        }

        public void RegisterPromptUI(InteractionPromptUI ui)
        {
            promptUI = ui;
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null && !nearbyInteractables.Contains(interactable))
            {
                nearbyInteractables.Add(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                nearbyInteractables.Remove(interactable);
            }
        }

        private void UpdateFocus()
        {
            nearbyInteractables.RemoveAll(item => item == null);
            focusedInteractable = null;
            var nearestDistance = float.MaxValue;

            foreach (var interactable in nearbyInteractables)
            {
                var interactableTransform = ((MonoBehaviour)interactable).transform;
                var distance = Vector3.Distance(transform.position, interactableTransform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    focusedInteractable = interactable;
                }
            }

            if (focusedInteractable == null)
            {
                promptUI?.Hide();
            }
            else
            {
                promptUI?.Show(focusedInteractable.InteractionLabel);
            }
        }
    }
}
