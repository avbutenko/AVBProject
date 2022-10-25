using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AVBProject.Components
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
