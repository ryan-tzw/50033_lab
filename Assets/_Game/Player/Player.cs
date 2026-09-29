using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private enum PlayerState
    {
        Alive,
        Dying,
        Dead
    }
    
    private PlayerState _state;
    private PlayerHealth _health;
    private PlayerMovement _movement;
    private PlayerInput _input;
    private DissolveEffect _dissolve;

    public event System.Action Died;

    private void Awake()
    {
        _health  = GetComponent<PlayerHealth>();
        _movement = GetComponent<PlayerMovement>();
        _input = GetComponent<PlayerInput>();
        _dissolve = GetComponent<DissolveEffect>();
        _state = PlayerState.Alive;
        _health.Depleted += HandleHealthDepleted;
    }

    private void Update()
    {
        if (_state == PlayerState.Dying && _dissolve.Tick(Time.deltaTime))
        {
            _state = PlayerState.Dead;
            Died?.Invoke();
        }
    }

    private void OnDestroy()
    {
        _health.Depleted -= HandleHealthDepleted;
    }

    public bool ReceiveHit(HitData hit)
    {
        if (_state != PlayerState.Alive || !_health.ReceiveHit(hit)) return false;

        _movement.ApplyHitReaction(hit.Direction, hit.Knockback, hit.HitstunDuration);
        return true;
    }

    private void HandleHealthDepleted()
    {
        _state =  PlayerState.Dying;
        _input.DeactivateInput();
        _dissolve.Begin();
    }
}