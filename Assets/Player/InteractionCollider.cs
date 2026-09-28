using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private Interactable interactableInRange = null;

    public void OnInteract()
    {
        if (interactableInRange != null)
        {
            
            interactableInRange.Interact();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null && interactable.CanInteract())
        {
            interactableInRange = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if(interactable == interactableInRange)
        {
            interactableInRange = null;
        }
    }
}
