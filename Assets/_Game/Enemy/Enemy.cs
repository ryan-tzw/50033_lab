using System;
using UnityEngine;

[RequireComponent( typeof(EnemyHealth), typeof(EnemyMovement), typeof(DissolveEffect))]
public class Enemy : StateController
{
    [SerializeField] private Collider contactDmgCollider;
    [SerializeField] private AudioClip hurtSound;
    private AudioSource _audioSource;

    private EnemyHealth _health;
    private EnemyMovement _movement;
    private DissolveEffect _dissolveEffect;
    private Action<Enemy> _releaseCallback;
    
    private bool _recoveryRequested;
    private float _recoveryDuration;
    public bool RecoveryRequested => _recoveryRequested;
    public float RecoveryDuration => _recoveryDuration;

    private bool _deathRequested;
    public bool DeathRequested => _deathRequested;
    
    public event Action<Enemy> Killed;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _health = GetComponent<EnemyHealth>();
        _movement = GetComponent<EnemyMovement>();
        _dissolveEffect = GetComponent<DissolveEffect>();
        _health.Depleted += HandleHealthDepleted;
    }

    protected override void OnEnable()
    {
        _recoveryRequested = false;
        _recoveryDuration = 0f;
        _deathRequested = false;
        contactDmgCollider.enabled = true;
        base.OnEnable();
    }

    private void OnDestroy()
    {
        _health.Depleted -= HandleHealthDepleted;
    }
    
    //================================================== 

    public void RequestRecovery(float duration)
    {
        if (_deathRequested || duration <= 0f) return;
        _recoveryDuration = duration;
        _recoveryRequested = true;
    }

    public void BeginRecoveryState()
    {
        _recoveryRequested = false;
    }

    public void SetMovementEnabled(bool enabled)
    {
        _movement.SetMovementEnabled(enabled);
    }

    public void SetAnimationPaused(bool paused)
    {
        _movement.SetAnimationPaused(paused);
    }

    public void BeginDeathState()
    {
        _recoveryRequested = false;
        contactDmgCollider.enabled = false;
        Killed?.Invoke(this);
        _dissolveEffect.Begin();
    }

    public void TickDeathState()
    {
        if (_dissolveEffect.Tick(Time.deltaTime))
        {
            Release();
        }
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
        if (_deathRequested) return;
        
        _health.ReceiveHit(hit);
        if (hurtSound)
        {
            _audioSource.PlayOneShot(hurtSound);
        }
    }

    private void HandleHealthDepleted()
    {
        _deathRequested = true;
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