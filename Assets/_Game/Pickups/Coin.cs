using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private CoinRuntimeSet activeCoins;
    
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
    private System.Action<Coin> _releaseCallback;
    private System.Action _collectedCallback;

    private Vector3 _pickupStartPosition;
    private float _pickupElapsed;
    private bool _isCollecting;
    private int _playerHitboxLayer;
    
    // magnet stuff
    [SerializeField, Min(0f)] private float magnetAccel = 20f;
    [SerializeField, Min(0f)] private float magnetMaxSpd = 10f;
    private Transform _magnetTarget;
    private float _magnetSpeed;
    private bool _isMagnetised;

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

    private void OnEnable()
    {
        activeCoins.Add(this);
    }

    private void OnDisable()
    {
        activeCoins.Remove(this);
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

    private void FixedUpdate()
    {
        // move the coin towards the player if magnetised
        if (_isCollecting || _magnetTarget is null) return;
        
        _magnetSpeed = Mathf.MoveTowards(_magnetSpeed, magnetMaxSpd, magnetAccel * Time.fixedDeltaTime);
        var nextPosition = Vector3.MoveTowards(_rb.position, _magnetTarget.position, Time.fixedDeltaTime * _magnetSpeed);
        
        _rb.MovePosition(nextPosition);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isCollecting) return;
        if (other.gameObject.layer != _playerHitboxLayer) return;
        
        // once it collides with the player begin the pickup animation
        _isCollecting = true;
        _pickupElapsed = 0f;
        _pickupStartPosition = transform.position;
        _magnetTarget = null;

        // stop physics from interfering with the animation
        _rb.isKinematic = true;
        _worldCollider.enabled = false;
        _pickupCollider.enabled = false;
        
        _collectedCallback.Invoke();
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
        _magnetTarget = null;
        _magnetSpeed = 0f;
        _isMagnetised = false;

        gameObject.SetActive(true);
        _rb.AddForce(impulse, ForceMode.Impulse);
    }

    // only sets the target and some settings. actual movement in FixedUpdate
    public void AttractTo(Transform target)
    {
        if (_isCollecting) return;
        if (_isMagnetised) return;

        // disable physics and prevent collision with the world
        _magnetTarget = target;
        _magnetSpeed = 0f;
        _rb.isKinematic = true;
        _worldCollider.enabled = false;

        // keep the magnetised state so that subsequent magnets dont reset the speed
        _isMagnetised = true;
    }

    public void SetReleaseCallback(System.Action<Coin> releaseCallback)
    {
        _releaseCallback = releaseCallback;
    }

    public void SetCollectedCallback(System.Action collectedCallback)
    {
        _collectedCallback = collectedCallback;
    }

    private void Despawn()
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
