using UnityEngine;

public class Magnet : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _worldCollider;
    private System.Action<Magnet> _releaseCallback;
    private System.Action _collectedCallback;
    
    private int _playerHitboxLayer;
    private bool _collected;

    [SerializeField] private CoinRuntimeSet activeCoins;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _worldCollider = GetComponent<Collider>();
        _playerHitboxLayer = LayerMask.NameToLayer("PlayerHitbox");
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
        _collected = false;
        _worldCollider.enabled = true;
        _rb.isKinematic = false;
        _rb.linearVelocity = Vector3.zero;
        
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
