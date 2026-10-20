using System;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(DissolveEffect))]
[RequireComponent(typeof(LocalTime))]
public class Enemy : StateController, IHittable
{
    [SerializeField] private Collider contactDmgCollider;

    private EnemyHealth _health;
    private EnemyMovement _movement;
    private DissolveEffect _dissolveEffect;
    private Action<Enemy> _releaseCallback;
    
    public LocalTime LocalTime { get; private set; }
    public bool RecoveryRequested { get; private set; }
    public float RecoveryDuration { get; private set; }
    public bool IsHealthDepleted => _health.IsDepleted;
    
    public event Action<Enemy> Killed;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
        _movement = GetComponent<EnemyMovement>();
        _dissolveEffect = GetComponent<DissolveEffect>();
        LocalTime = GetComponent<LocalTime>();
    }

    protected override void OnEnable()
    {
        RecoveryRequested = false;
        RecoveryDuration = 0f;
        contactDmgCollider.enabled = true;
        base.OnEnable();
    }

    protected override void Update()
    {
        if (LocalTime.IsPaused) return;
        base.Update();
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

    public bool ReceiveHit(HitData hit)
    {
        if (_health.IsDepleted) return false;
        
        _health.ReceiveHit(hit);
        _movement.ApplyKnockback(hit.Direction, hit.Knockback);
        return true;
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