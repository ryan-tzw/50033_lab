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
    
    public bool RecoveryRequested { get; private set; }
    public float RecoveryDuration { get; private set; }
    public bool IsHealthDepleted => _health.IsDepleted;
    
    public event Action<Enemy> Killed;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _health = GetComponent<EnemyHealth>();
        _movement = GetComponent<EnemyMovement>();
        _dissolveEffect = GetComponent<DissolveEffect>();
    }

    protected override void OnEnable()
    {
        RecoveryRequested = false;
        RecoveryDuration = 0f;
        contactDmgCollider.enabled = true;
        base.OnEnable();
    }

    //================================================== 

    public void RequestRecovery(float duration)
    {
        if (_health.IsDepleted || duration <= 0f) return;
        RecoveryDuration = duration;
        RecoveryRequested = true;
    }

    public void BeginRecoveryState()
    {
        RecoveryRequested = false;
    }

    public void SetMovementEnabled(bool movementEnabled)
    {
        _movement.SetMovementEnabled(movementEnabled);
    }

    public void SetAnimationPaused(bool paused)
    {
        _movement.SetAnimationPaused(paused);
    }

    public void BeginDeathState()
    {
        RecoveryRequested = false;
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
        if (_health.IsDepleted) return;
        
        _health.ReceiveHit(hit);
        if (hurtSound)
        {
            _audioSource.PlayOneShot(hurtSound);
        }
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