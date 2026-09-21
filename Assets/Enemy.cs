using UnityEngine;

public class Enemy : MonoBehaviour
{
    // health
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;

    // hitstun/knockback
    [SerializeField] private float hitstunDuration = 0.5f;
    [SerializeField] private float recoilSpeed = 25f;
    [SerializeField] private float recoilFalloffPower = 3f;
    [SerializeField] private float collisionDamage = 1;
    private float _hitstunStartTime;
    private float _hitstunEndTime;
    private Vector2 _recoilDirection;
    
    // hitstop
    // todo: unify this between the player and the enemy.
    //  hitstop should probably be part of the attack's data since different attacks want different hitstop (light/heavy)
    [SerializeField] private float hitstopDuration = 0.05f;
    private float _hitstopEndTime;

    private EnemyState _state = EnemyState.Alive;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Color _normalColor; // save the initial color to reset back to

    private enum EnemyState
    {
        Alive,
        Hitstopped,
        Hitstunned,
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _normalColor =  _spriteRenderer.color;
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        // todo: die when hp <= 0
        _currentHealth -= damage;
        _spriteRenderer.color = Color.white;
        
        // save the recoil direction for later during hitstun phase
        _recoilDirection = (_rb.position - attackerPosition).normalized;

        // start with hitstop first
        _state = EnemyState.Hitstopped;
        _hitstopEndTime =  Time.time + hitstopDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (player != null && collision == player.PlayerCollider)
        {
            player.TakeDamage(collisionDamage,_rb.position);
        }
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Hitstopped:
            {
                _rb.linearVelocity = Vector2.zero;
                if (Time.time > _hitstopEndTime)
                {
                    // when hitstop ends, begin hitstun duration
                    _state = EnemyState.Hitstunned;
                    _hitstunStartTime =  Time.time;
                    _hitstunEndTime = Time.time + hitstunDuration;
                    _rb.linearVelocity = _recoilDirection * recoilSpeed;
                }
                break;
            }
            
            case EnemyState.Hitstunned:
            {
                float progress = Mathf.InverseLerp(_hitstunStartTime,  _hitstunEndTime, Time.time);
                float speedMultiplier = Mathf.Pow((1 - progress), recoilFalloffPower);

                _rb.linearVelocity = recoilSpeed * speedMultiplier * _recoilDirection;
                if (progress >= 1f)
                {
                    _rb.linearVelocity = Vector2.zero;
                    _state = EnemyState.Alive;
                    _spriteRenderer.color = _normalColor;
                }
                
                break;
            }
        }
    }
}
