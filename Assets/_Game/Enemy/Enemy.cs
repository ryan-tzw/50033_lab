using System;
using UnityEngine;

[RequireComponent( typeof(EnemyHealth), typeof(EnemyMovement), typeof(DissolveEffect))]
public class Enemy : StateController
{
    [SerializeField] private Collider contactDmgCollider;
    [SerializeField] private AudioClip hurtSound;
    private AudioSource _audioSource;

    private EnemyHealth _health;
    private DissolveEffect _dissolveEffect;
    private Action<Enemy> _releaseCallback;
    private bool _isDying;
    public bool IsDying => _isDying;
    private bool _recoveryRequested;
    private float _recoveryDuration;
    public bool RecoveryRequested => _recoveryRequested;
    public float RecoveryDuration => _recoveryDuration;
    
    public event Action<Enemy> Killed;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _health = GetComponent<EnemyHealth>();
        _dissolveEffect = GetComponent<DissolveEffect>();
        _health.Depleted += HandleHealthDepleted;
    }

    protected override void OnEnable()
    {
        _isDying = false;
        _recoveryRequested = false;
        _recoveryDuration = 0f;
        contactDmgCollider.enabled = true;
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
        
        if (!_isDying) return;

        if (_dissolveEffect.Tick(Time.deltaTime))
        {
            Release();
        }
    }

    public void RequestRecovery(float duration)
    {
        if (_isDying || duration <= 0f) return;
        _recoveryDuration = duration;
        _recoveryRequested = true;
    }

    public void BeginRecoveryState()
    {
        _recoveryRequested = false;
    }

    private void OnDestroy()
    {
        _health.Depleted -= HandleHealthDepleted;
    }

    public void Spawn(Vector3 position)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
        gameObject.SetActive(true);
    }

    public void SetReleaseCallback(Action<Enemy> releaseCallback)
    {
        _releaseCallback = releaseCallback;
    }

    public void ReceiveHit(HitData hit)
    {
        _health.ReceiveHit(hit);
        if (hurtSound)
        {
            _audioSource.PlayOneShot(hurtSound);
        }
    }

    private void HandleHealthDepleted()
    {
        _isDying = true;
        contactDmgCollider.enabled = false;

        Killed?.Invoke(this);
        _dissolveEffect.Begin();
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