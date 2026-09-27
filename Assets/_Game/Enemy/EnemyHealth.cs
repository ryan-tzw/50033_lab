using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private const float FlashDuration = 0.10f;
    private const float DissolveDuration = 1.0f;
    private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
    private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");
    private static readonly int DissolveAmountId = Shader.PropertyToID("_DissolveAmount");
    private static readonly int NoiseOffsetId = Shader.PropertyToID("_NoiseOffset");
    
    [SerializeField, Min(1)] private int maxHealth = 3;
    [SerializeField] private int currentHealth;

    [SerializeField] private Collider contactDmgCollider;
    
    private SpriteRenderer _sr;
    private MaterialPropertyBlock _mpb;
    
    private float _flashRemaining;
    private float _dissolveElapsed;
    private bool _isDying;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _mpb = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (_flashRemaining > 0f)
        {
            _flashRemaining -= Time.deltaTime;
            if (_flashRemaining <= 0f)
            {
                SetFlash(0f);
            }
        }

        if (!_isDying) return;
        
        _dissolveElapsed += Time.deltaTime;
        float progress = Mathf.Min(_dissolveElapsed / DissolveDuration, 1f);
        SetDissolve(progress);

        if (progress >= 1f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        _flashRemaining = 0f;
        _dissolveElapsed = 0f;
        _isDying = false;
        
        contactDmgCollider.enabled = true;

        _sr.GetPropertyBlock(_mpb);
        _mpb.SetFloat(FlashAmountId, 0f);
        _mpb.SetColor(FlashColorId, Color.white);
        _mpb.SetVector(NoiseOffsetId, new Vector4(Random.Range(0f, 100f), Random.Range(0f,100f), 0f, 0f));
        _sr.SetPropertyBlock(_mpb);
    }

    public void ReceiveHit(HitData hit)
    {
        if (_isDying) return;
        
        currentHealth -= hit.Damage;
        _flashRemaining = FlashDuration;
        SetFlash(1f);

        if (currentHealth <= 0)
        {
            _isDying = true;
            _dissolveElapsed = 0f;

            contactDmgCollider.enabled = false;
        }
    }

    private void SetFlash(float amount)
    {
        _sr.GetPropertyBlock(_mpb);
        _mpb.SetFloat(FlashAmountId, amount);
        _mpb.SetColor(FlashColorId, Color.white);
        _sr.SetPropertyBlock(_mpb);
    }

    private void SetDissolve(float amount)
    {
        _sr.GetPropertyBlock(_mpb);
        _mpb.SetFloat(DissolveAmountId, amount);
        _sr.SetPropertyBlock(_mpb);
    }
}