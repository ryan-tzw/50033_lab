using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private Transform spriteTransform;
    
    // melee swing animation
    [SerializeField] private GameObject swingPrefab;
    [SerializeField] private float swingLifetime;
    [SerializeField] private float attackCooldown = 0.4f;
    private float _nextAttackTime;

    // lock the player rotation when attacking
    private float _rotationLockedTime;

    private float _hitstopEndTime;

    private Rigidbody2D _rb;
    
    // player inputs
    private PlayerInput _playerInput;
    private InputActionMap _combatActions;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _aimAction;
    
    private Vector2 facingDir = Vector2.right;
    private Vector2 _moveDir;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _combatActions = _playerInput.actions.FindActionMap("Combat");

        _moveAction = _combatActions.FindAction("Move");
        _aimAction = _combatActions.FindAction("Aim");
        _attackAction = _combatActions.FindAction("Attack");
    }

    private void Attack()
    {
        if (Time.time < _nextAttackTime) return;

        _nextAttackTime = Time.time + attackCooldown;
        _rotationLockedTime = Time.time + swingLifetime;
        
        GameObject swing =  Instantiate(swingPrefab, transform.position, spriteTransform.rotation, transform);
        Destroy(swing, swingLifetime);
    }
    
    // provide a public fn so that the attack hitbox can callback
    public void ApplyHitstop(float duration)
    {
        _hitstopEndTime = Time.time + duration;
        _rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        // dont do anything while hitstopped
        // todo: probably later might want to do something that will allow the player to buffer inputs
        // but not unique to hitstop, would apply even e.g. when casting one spell -> next spell or smth
        if (Time.time < _hitstopEndTime) return;
        
        if (_attackAction.WasPressedThisFrame())
        {
            Attack();
        }
        
        // only turn the player when not locked in an animation
        if (Time.time >= _rotationLockedTime)
        {
            Vector2 aim = _aimAction.ReadValue<Vector2>();

            // note: for now im just putting everything in a single file since it's still quite small but refactor later if it gets too large
            // keyboard and mouse aiming
            if (_playerInput.currentControlScheme == "KBM")
            {
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(aim);
                facingDir = ((Vector2)mouseWorld - (Vector2)transform.position).normalized;
            }
            // controller aiming
            else
            {
                // right stick aiming
                if (aim.sqrMagnitude > 0.1f)
                {
                    facingDir = aim.normalized;
                }
                // use move direction if right stick not in use
                else
                {
                    Vector2 move = _moveAction.ReadValue<Vector2>();
                    if (move.sqrMagnitude > 0.1f)
                    {
                        facingDir = move.normalized;
                    }
                }
            }

            float angle = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;
            spriteTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }

    private void FixedUpdate()
    {
        if (Time.time < _hitstopEndTime)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        
        Vector2 moveDir = _moveAction.ReadValue<Vector2>();
        _rb.linearVelocity = Vector2.ClampMagnitude(moveDir, 1f) * moveSpeed;
    }
    
}
