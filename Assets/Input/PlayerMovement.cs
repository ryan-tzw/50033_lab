using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    
    private Rigidbody2D _rb;
    private PlayerInputActions _actions;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _actions = new PlayerInputActions();
        _actions.Combat.Enable();
        _actions.Combat.Attack.performed += Attack;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _actions.Combat.Move.ReadValue<Vector2>();
        _rb.linearVelocity = inputVector.normalized * moveSpeed;
    }
    
}
