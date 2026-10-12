using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private bool _movementEnabled = true;
    
    // knockback
    private Vector3 _knockbackVelocity;
    private float _hitstunDuration;
    private float _hitstunRemaining;

    private Vector3 _attackVelocity;
    private float _attackMomentumDuration;
    private float _attackMomentumRemaining;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private static readonly int IsMovingId = Animator.StringToHash("IsMoving");
    
    private PlayerInput _playerInput;
    private InputActionMap _combatActions;
    private InputAction _moveAction;

    private Rigidbody _rb;
    private LocalTime _localTime;
        
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _combatActions = _playerInput.actions.FindActionMap("Combat");
        _moveAction = _combatActions.FindAction("Move");
        _localTime = GetComponent<LocalTime>();
    }

    private void Update()
    {
        _animator.speed = _localTime.IsPaused ? 0f : 1f;
        if (_localTime.IsPaused) return;

        if (!_movementEnabled)
        {
            _animator.SetBool(IsMovingId, false);
            return;
        }
        
        var move = _moveAction.ReadValue<Vector2>();
        _animator.SetBool(IsMovingId, move.sqrMagnitude > 0.01f);
        if (move.x != 0)
        {
            _spriteRenderer.flipX = move.x < 0;
        }
    }

    private void FixedUpdate()
    {
        if (_localTime.IsPaused)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            return;
        }
        
        _attackMomentumRemaining -= Time.fixedDeltaTime;
        
        if (_hitstunRemaining > 0f)
        {
            var remainingFraction = _hitstunRemaining / _hitstunDuration;
            var velocity = _knockbackVelocity * remainingFraction;
            
            _rb.linearVelocity = new  Vector3(velocity.x, _rb.linearVelocity.y, velocity.z);
            _hitstunRemaining -= Time.fixedDeltaTime;
            
            return;
        }

        if (_attackMomentumRemaining > 0f)
        {
            var remainingFraction = _attackMomentumRemaining / _attackMomentumDuration;
            var vel = _attackVelocity * remainingFraction;
            _rb.linearVelocity = new Vector3(vel.x, _rb.linearVelocity.y, vel.z);
            return;
        }

        if (!_movementEnabled)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            return;
        }
        
        var moveDir = _moveAction.ReadValue<Vector2>();
        var hVelocity = Vector3.ClampMagnitude(new Vector3(moveDir.x, 0, moveDir.y), 1f) * moveSpeed;
        _rb.linearVelocity = new Vector3(hVelocity.x, _rb.linearVelocity.y, hVelocity.z);
    }

    public void ApplyHitReaction(Vector3 dir, float knockback, float hitstunDuration)
    {
        dir.y = 0f;
        
        _knockbackVelocity = dir.normalized * knockback;
        _hitstunDuration = hitstunDuration;
        _hitstunRemaining = hitstunDuration;
    }

    public void ApplyAttackMomentum(Vector3 dir, float spd, float dur)
    {
        dir.y = 0f;
        _attackVelocity = dir.normalized * spd;
        _attackMomentumDuration = dur;
        _attackMomentumRemaining = dur;
    }
    
    public void SetMovementEnabled(bool moveEnabled)
    {
        _movementEnabled = moveEnabled;
        if (moveEnabled) return;
        _animator.SetBool(IsMovingId, false);
        _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
    }
}