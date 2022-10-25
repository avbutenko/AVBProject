using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.PixelCrew.Components.Interactions
{

    public class DoInteractionComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
            var interactable = go.GetComponent<InteractableComponent>();
            interactable?.Interact();
        }

    }
}
