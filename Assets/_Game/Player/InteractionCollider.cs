using UnityEngine;

public class InteractionCollider : MonoBehaviour
{
    private IInteractable interactableInRange = null;

    public void OnInteract()
    {
        if (interactableInRange != null)
        {

            interactableInRange.Interact();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null && interactable.CanInteract())
        {
            interactableInRange = interactable;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable == interactableInRange)
        {
            interactableInRange = null;
        }
    }
}