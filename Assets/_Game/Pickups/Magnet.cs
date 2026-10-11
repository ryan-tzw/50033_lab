using UnityEngine;

public class Magnet : MonoBehaviour
{
    private Rigidbody _rb;
    private SpriteRenderer _sr;
    private Collider _worldCollider;
    private System.Action<Magnet> _releaseCallback;
    private System.Action _collectedCallback;
    
    private int _playerHitboxLayer;
    private bool _collected;

    [SerializeField] private CoinRuntimeSet activeCoins;

    [SerializeField, Min(0f)] private float lifetime = 15f;
    private float _remainingLifetime;
    
    [SerializeField, Min(0f)] private float blinkStartTime = 15f;
    [SerializeField, Range(0.01f, 0.5f)] private float blinkInterval = 0.1f;
    private float _blinkTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _worldCollider = GetComponent<Collider>();
        _playerHitboxLayer = LayerMask.NameToLayer("PlayerHitbox");
    }

    private void Update()
    {
        if (_collected) return;
        
        _remainingLifetime -= Time.deltaTime;

        if (_remainingLifetime <= 0f)
        {
            _collected = true;
            Release();
            return;
        }

        if (_remainingLifetime > blinkStartTime) return;
        
        _blinkTimer -= Time.deltaTime;
        
        if (_blinkTimer > 0f) return;

        _blinkTimer = blinkInterval;
        _sr.enabled = !_sr.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collected) return;
        if (other.gameObject.layer != _playerHitboxLayer) return;
        
        _collected = true;

        foreach (var coin in activeCoins.Items)
        {
            if (coin) coin.AttractTo(other.transform);
        }
        
        _collectedCallback.Invoke();
        
        Release();
    }

    public void Spawn(Vector3 position, Vector3 impulse)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
        _worldCollider.enabled = true;
        _rb.isKinematic = false;
        _rb.linearVelocity = Vector3.zero;
        _collected = false;
        _remainingLifetime = lifetime;
        _blinkTimer = blinkInterval;
        _sr.enabled = true;
        
        gameObject.SetActive(true);
        _rb.AddForce(impulse, ForceMode.Impulse);
    }

    public void SetReleaseCallback(System.Action<Magnet> callback)
    {
        _releaseCallback = callback;
    }

    public void SetCollectedCallback(System.Action callback)
    {
        _collectedCallback = callback;
    }

    private void Release()
    {
        if (_releaseCallback is null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _releaseCallback.Invoke(this);
        }
    }
}
