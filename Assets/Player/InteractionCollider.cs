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
            Debug.Log("Activate Interaction");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(interactableInRange);
        Interactable interactable = collision.GetComponent<Interactable>();
        Debug.Log(interactable);
        if (interactable != null && interactable.CanInteract())
        {
            Debug.Log("Interactable in range");
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
