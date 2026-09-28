using UnityEngine;

public class Magnet : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _worldCollider;
    private CoinPool _coinPool;
    private System.Action<Magnet> _releaseCallback;
    private int _playerHitboxLayer;
    private bool _collected;

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
        _coinPool.AttractAll(other.transform);
        Release();
    }

    public void Spawn(Vector3 position, Vector3 impulse, CoinPool pool)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
        _coinPool = pool;
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
