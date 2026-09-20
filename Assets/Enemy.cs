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
    private float _hitstunStartTime;
    private float _hitstunEndTime;
    private Vector2 _recoilDirection;

    private EnemyState _state = EnemyState.Alive;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private enum EnemyState
    {
        Alive,
        Hitstunned,
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        // todo: die when hp <= 0
        _currentHealth -= damage;
        
        // hitstun
        _state = EnemyState.Hitstunned;
        _hitstunStartTime = Time.time;
        _hitstunEndTime = Time.time + hitstunDuration;
        _spriteRenderer.color = Color.white;
        
        _recoilDirection = (_rb.position - attackerPosition).normalized;
        _rb.linearVelocity = _recoilDirection * recoilSpeed;
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Hitstunned:
            {
                float progress = Mathf.InverseLerp(_hitstunStartTime,  _hitstunEndTime, Time.time);
                float speedMultiplier = Mathf.Pow((1 - progress), recoilFalloffPower);

                _rb.linearVelocity = recoilSpeed * speedMultiplier * _recoilDirection;
                if (progress >= 1f)
                {
                    _rb.linearVelocity = Vector2.zero;
                    _state = EnemyState.Alive;
                    _spriteRenderer.color = new Color32(243, 86, 86, 255);
                }
                
                break;
            }
        }
        
    }
}
