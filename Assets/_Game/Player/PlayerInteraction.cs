using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputActionMap _interactions;
    private InputAction _interactAction;
    [SerializeField] private InteractionCollider _interactionCollider;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _interactionCollider = GetComponentInChildren<InteractionCollider>();
        _interactions = _playerInput.actions.FindActionMap("Interaction");
        _interactAction = _interactions.FindAction("Interact");
        _interactions.Enable();
    }

    private void Interact()
    {
        _interactionCollider.OnInteract();
    }

    private void Update()
    {

        if (_interactAction.WasPressedThisFrame())
        {
            Interact();
        }
    }
}
