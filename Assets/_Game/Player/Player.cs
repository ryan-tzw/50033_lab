using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerAttack))]
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
    private PlayerAttack _attack;

    public LocalTime LocalTime { get; private set; }
    public bool IsHealthDepleted => _health.IsDepleted;
    public bool IsDissolveFinished => _dissolve.IsFinished;
    public bool IsAttacking => _attack.IsAttacking;

    [SerializeField] private GameEvent playerDied;
    [SerializeField] private PlayerRuntimeSet activePlayers;

    private void Awake()
    {
        _health  = GetComponent<PlayerHealth>();
        _movement = GetComponent<PlayerMovement>();
        _attack = GetComponent<PlayerAttack>();
        _input = GetComponent<PlayerInput>();
        _dissolve = GetComponent<DissolveEffect>();
        LocalTime = GetComponent<LocalTime>();
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
        _movement.SetMovementEnabled(true);
    }

    public void BeginAttackState()
    {
        _movement.SetMovementEnabled(false);
        _movement.ApplyAttackMomentum(_attack.AttackDirection, _attack.AttackForwardSpeed, _attack.AttackDuration);
    }

    public void BeginDeathState()
    {
        _input.DeactivateInput();
        _movement.SetMovementEnabled(false);
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