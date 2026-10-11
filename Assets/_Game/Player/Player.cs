using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(DissolveEffect))]
[RequireComponent(typeof(LocalTime))]
public class Player : StateController
{
    private PlayerHealth _health;
    private PlayerMovement _movement;
    private PlayerInput _input;
    private DissolveEffect _dissolve;

    public bool IsHealthDepleted => _health.IsDepleted;
    public bool IsDissolveFinished => _dissolve.IsFinished;

    [SerializeField] private GameEvent playerDied;
    [SerializeField] private PlayerRuntimeSet activePlayers;

    private void Awake()
    {
        _health  = GetComponent<PlayerHealth>();
        _movement = GetComponent<PlayerMovement>();
        _input = GetComponent<PlayerInput>();
        _dissolve = GetComponent<DissolveEffect>();
    }

    protected override void OnEnable()
    {
        activePlayers.Add(this);
        base.OnEnable();
    }

    private void OnDisable()
    {
        activePlayers.Remove(this);
    }

    public bool ReceiveHit(HitData hit)
    {
        if (_health.IsDepleted || !_health.ReceiveHit(hit)) return false;

        _movement.ApplyHitReaction(hit.Direction, hit.Knockback, hit.HitstunDuration);
        return true;
    }
    
    public bool ReceiveHeal()
    {
        return !_health.IsDepleted && _health.CurrentHealth < _health.MaxHealth;
    }

    public void BeginAliveState()
    {
        _input.ActivateInput();
    }

    public void BeginDeathState()
    {
        _input.DeactivateInput();
        _dissolve.Begin();
    }

    public void TickDeathState()
    {
        _dissolve.Tick(Time.deltaTime);
    }

    public void RaisePlayerDied()
    {
        playerDied.Raise();
    }
}