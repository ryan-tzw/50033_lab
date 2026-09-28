using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float pickupDuration = 0.4f;
    [SerializeField, Min(0f)] private float pickupHeight = 0.75f;

    [SerializeField]
    private AnimationCurve pickupHeightCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.7f, 1f),
        new Keyframe(1f, 0.8f)
    );

    private Rigidbody _rb;
    private Collider _worldCollider;
    private Collider _pickupCollider;
    private System.Action<Coin> _despawnCallback;

    private Vector3 _pickupStartPosition;
    private float _pickupElapsed;
    private bool _isCollecting;
    private int _playerHitboxLayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _worldCollider = GetComponent<Collider>();
        _playerHitboxLayer = LayerMask.NameToLayer("PlayerHitbox");

        foreach (var childCollider in GetComponentsInChildren<Collider>())
        {
            if (childCollider.isTrigger)
            {
                _pickupCollider = childCollider;
                break;
            }
        }
    }

    private void Update()
    {
        if (!_isCollecting) return;
        
        // pickup animation
        _pickupElapsed += Time.deltaTime;
        var progress = Mathf.Min(_pickupElapsed / pickupDuration, 1f);
        var height = pickupHeightCurve.Evaluate(progress) * pickupHeight;

        transform.position = _pickupStartPosition + Vector3.up * height;

        if (progress >= 1f)
        {
            Despawn();
        }
    }

    public void Spawn(Vector3 position, Vector3 impulse)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);

        // reset stuff
        _isCollecting = false;
        _pickupElapsed = 0f;
        _worldCollider.enabled = true;
        _pickupCollider.enabled = true;
        _rb.isKinematic = false;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        gameObject.SetActive(true);
        _rb.AddForce(impulse, ForceMode.Impulse);
    }

    public void SetDespawnCallback(System.Action<Coin> despawnCallback)
    {
        _despawnCallback = despawnCallback;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision with "  + other.gameObject.name);
        if (_isCollecting) return;
        if (other.gameObject.layer != _playerHitboxLayer) return;

        BeginPickup();
    }

    private void BeginPickup()
    {
        _isCollecting = true;
        _pickupElapsed = 0f;
        _pickupStartPosition = transform.position;

        // stop physics from interfering with the animation
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;

        _worldCollider.enabled = false;
        _pickupCollider.enabled = false;
    }

    private void Despawn()
    {
        if (_despawnCallback is null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _despawnCallback.Invoke(this);
        }
    }
}
