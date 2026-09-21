using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    // health
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;
    
    // death/dissolve shader
    [SerializeField] private float dissolveDuration = 1f;
    private float _dissolveStartTime;
    private  float _dissolveEndTime;
    
    // note:
    // all instances use the same material so we can't modify the material itself or it will affect other instances
    // MaterialPropertyBlock lets us override properties for individual instances
    // (in this case i want to randomise the noise offset so the animation doesn't look the same every time)
    private MaterialPropertyBlock _materialProperties;
    private static readonly int DissolveAmountId = Shader.PropertyToID("_DissolveAmount");
    private static readonly int NoiseOffsetId = Shader.PropertyToID("_NoiseOffset");
    private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");

    // hitstun/knockback
    [SerializeField] private float hitstunDuration = 0.25f;
    [SerializeField] private float recoilSpeed = 25f;
    [SerializeField] private float recoilFalloffPower = 3f;
    private float _hitstunStartTime;
    private float _hitstunEndTime;
    private Vector2 _recoilDirection;
    
    // hitstop
    // todo: unify this between the player and the enemy.
    //  hitstop should probably be part of the attack's data since different attacks want different hitstop (light/heavy)
    [SerializeField] private float hitstopDuration = 0.1f;
    private float _hitstopEndTime;

    private EnemyState _state = EnemyState.Alive;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private enum EnemyState
    {
        Alive,
        Hitstopped,
        Hitstunned,
        Dying,
        Dead
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;
        _collider = GetComponent<Collider2D>();
        
        // copy the renderer's existing properties first 
        _materialProperties = new MaterialPropertyBlock();
        _spriteRenderer.GetPropertyBlock(_materialProperties);
        
        // modify the properties with our random values, then update it 
        _materialProperties.SetFloat(DissolveAmountId, 0f);
        _materialProperties.SetVector(NoiseOffsetId, new Vector4(Random.Range(0f, 100f), Random.Range(0f, 100f), 0f, 0f));
        _spriteRenderer.SetPropertyBlock(_materialProperties);
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (_state is EnemyState.Dying or EnemyState.Dead) return;
        
        _currentHealth -= damage;
        _materialProperties.SetFloat(FlashAmountId, 1f);
        _spriteRenderer.SetPropertyBlock(_materialProperties);
        
        // save the recoil direction for later during hitstun phase
        _recoilDirection = (_rb.position - attackerPosition).normalized;

        // start with hitstop first
        _state = EnemyState.Hitstopped;
        _hitstopEndTime =  Time.time + hitstopDuration;
    }

    private void Update()
    {
        switch (_state)
        {
            case EnemyState.Dying:
            {
                float progress = Mathf.InverseLerp(_dissolveStartTime, _dissolveEndTime, Time.time);
                
                _spriteRenderer.GetPropertyBlock(_materialProperties);
                _materialProperties.SetFloat(DissolveAmountId, progress);
                _spriteRenderer.SetPropertyBlock(_materialProperties);

                if (progress >= 1f)
                {
                    _state = EnemyState.Dead;
                    gameObject.SetActive(false);
                }
                
                break;
            }
            
        }
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Hitstopped:
            {
                _rb.linearVelocity = Vector2.zero;
                
                // when hitstop ends
                if (Time.time > _hitstopEndTime)
                {
                    _materialProperties.SetFloat(FlashAmountId, 0f);
                    _spriteRenderer.SetPropertyBlock(_materialProperties);
                    
                    _hitstunStartTime = Time.time;
                    _hitstunEndTime = Time.time + hitstunDuration;
                    _rb.linearVelocity = _recoilDirection * recoilSpeed;
                    
                    if (_currentHealth <= 0)
                    {
                        _state = EnemyState.Dying;
                        _dissolveStartTime =  Time.time;
                        _dissolveEndTime = Time.time + dissolveDuration;
                        _collider.enabled = false;
                    }
                    else
                    {
                        _state = EnemyState.Hitstunned;
                    }
                    
                }
                break;
            }
            
            // clean code is a myth anyway
            case EnemyState.Dying:
            case EnemyState.Hitstunned:
            {
                float progress = Mathf.InverseLerp(_hitstunStartTime,  _hitstunEndTime, Time.time);
                float speedMultiplier = Mathf.Pow(1f - progress, recoilFalloffPower);

                _rb.linearVelocity = recoilSpeed * speedMultiplier * _recoilDirection;
                if (progress >= 1f)
                {
                    _rb.linearVelocity = Vector2.zero;
                    
                    if (_state == EnemyState.Hitstunned)
                    {
                        _state = EnemyState.Alive;
                        _materialProperties.SetFloat(FlashAmountId, 0f);
                        _spriteRenderer.SetPropertyBlock(_materialProperties);
                    }
                }
                
                break;
            }
        }
    }
}
