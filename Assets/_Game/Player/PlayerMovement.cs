using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private static readonly int IsMovingId = Animator.StringToHash("IsMoving");
    
    private PlayerInput _playerInput;
    private InputActionMap _combatActions;
    private InputAction _moveAction;

    private Rigidbody _rb;
        
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _combatActions = _playerInput.actions.FindActionMap("Combat");
        _moveAction = _combatActions.FindAction("Move");
    }

    private void Update()
    {
        var move = _moveAction.ReadValue<Vector2>();
        _animator.SetBool(IsMovingId, move.sqrMagnitude > 0.01f);
        if (move.x != 0)
        {
            _spriteRenderer.flipX = move.x < 0;
        }
    }

    private void FixedUpdate()
    {
        var moveDir = _moveAction.ReadValue<Vector2>();
        var hVelocity = Vector3.ClampMagnitude(new Vector3(moveDir.x, 0, moveDir.y), 1f) * moveSpeed;
        _rb.linearVelocity = new Vector3(hVelocity.x, _rb.linearVelocity.y, hVelocity.z);
    }
}