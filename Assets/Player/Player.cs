using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private Transform spriteTransform;
    private PolygonCollider2D _playerCollider;
    public PolygonCollider2D PlayerCollider => _playerCollider;
    [SerializeField] private float _staggerSpeed = 8;
    [SerializeField] private float hitstunDuration = 0.5f;

    // melee swing animation
    [SerializeField] private GameObject swingPrefab;
    [SerializeField] private float swingLifetime;
    [SerializeField] private float attackCooldown = 0.4f;
    [SerializeField] private int healthPoints = 3;
    [SerializeField] private float immunityDuration = 0.8f;
    private float _nextAttackTime;

    // lock the player rotation when attacking
    private float _rotationLockedTime;

    private float _hitstopEndTime;

    private float _immunityEndTime;

    private float _hitstunStartTime;
    private float _hitstunEndTime;

    private Rigidbody2D _rb;

    private SpriteRenderer _spriteRenderer;
    
    // player inputs
    private PlayerInput _playerInput;
    private InputActionMap _combatActions;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _aimAction;
    private bool _immune;
    
    private Vector2 facingDir = Vector2.right;
    private Vector2 _moveDir;
    private Vector2 _staggerDirection;
    private PlayerState _state = PlayerState.Alive;

    [SerializeField] private float recoilFalloffPower = 2f;

    public event System.Action OnDeath;
    public event System.Action<int> OnHealthChanged;



    private enum PlayerState
    {
        Alive,
        Hitstopped,
        Hitstunned,
        Stunned,
        Dead,
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _playerCollider = GetComponentInChildren<PolygonCollider2D>();
        _combatActions = _playerInput.actions.FindActionMap("Combat");
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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

    //public fn so that enemy can callback
    public void TakeDamage(int damage, Vector2 enemyPosition)
    {
        if (_immune)
        {
            return;
        } else
        {
            healthPoints -= damage;
            _spriteRenderer.color = Color.red;
            DamageImmunity(immunityDuration);
            OnHealthChanged?.Invoke(healthPoints);
            Debug.Log("HP: " + healthPoints);
            if (healthPoints > 0)
            {
                _state = PlayerState.Hitstopped;
                _staggerDirection = ((Vector2)_rb.transform.position - enemyPosition).normalized;

            }
            else
            {
                _state = PlayerState.Dead;
            }
        }
        
    }

    private void DamageImmunity(float duration)
    {
        _immune = true;
        //_playerCollider.enabled = false;
        _immunityEndTime = Time.time + duration;
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

        //if (!_playerCollider.enabled && Time.time >= _immunityEndTime)
        if (_immune && Time.time >= _immunityEndTime)
        {
            //_playerCollider.enabled = true;
            _immune = false;
            _spriteRenderer.color = Color.white;
        }
    }

    private void FixedUpdate()
    {
        //if (Time.time < _hitstopEndTime)
        //{
        //    _rb.linearVelocity = Vector2.zero;
        //    return;
        //}

        switch (_state)
        {
            case PlayerState.Hitstopped:
                {
               
                    _rb.linearVelocity = Vector2.zero;
                    if (Time.time > _hitstopEndTime)
                    {
                        // when hitstop ends, begin hitstun duration
                        _state = PlayerState.Hitstunned;
                        _hitstunStartTime = Time.time;
                        _hitstunEndTime = Time.time + hitstunDuration;
                        _rb.linearVelocity = _staggerSpeed * _staggerDirection;

                    }
                    return;
                }

            case PlayerState.Hitstunned:
                {
                    float progress = Mathf.InverseLerp(_hitstunStartTime, _hitstunEndTime, Time.time);
                    float speedMultiplier = Mathf.Pow((1 - progress), recoilFalloffPower);

                    _rb.linearVelocity = _staggerSpeed * speedMultiplier * _staggerDirection;
                    if (progress >= 1f)
                    {
                        _rb.linearVelocity = Vector2.zero;
                        _state = PlayerState.Alive;
                        //_spriteRenderer.color = _normalColor;
                    }

                    return;
                }

            case PlayerState.Dead:
                {
                    _rb.linearVelocity = Vector2.zero;
                    _playerInput.DeactivateInput();
                    
                    if(OnDeath != null)
                    {
                        OnDeath.Invoke();
                    }
                    
                    
                    return;
                }
        }

        Vector2 moveDir = _moveAction.ReadValue<Vector2>();
        _rb.linearVelocity = Vector2.ClampMagnitude(moveDir, 1f) * moveSpeed;
    }
    
}
