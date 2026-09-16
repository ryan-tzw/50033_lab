using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    
    private Rigidbody2D _rb;
    private PlayerInput _playerInput;
    private InputActionMap _combatActions;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _combatActions = _playerInput.actions.FindActionMap("Combat");
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _combatActions["Move"].ReadValue<Vector2>();
        _rb.linearVelocity = inputVector.normalized * moveSpeed;
    }
    
}
