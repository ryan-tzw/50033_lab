using System;
using UnityEngine;

public readonly struct ScheduledAttack
{
    public int Index { get; }
    public float Duration { get; }

    public ScheduledAttack(int index, float duration)
    {
        Index = index;
        Duration = duration;
    }
}

/*
 * I'm trying to make this generic enough that in theory several different types of attacks could be added
 * and all use this to schedule their attacks. "Scheduling" includes: input buffering, hold to continually attack,
 * per-attack recovery and resetting a combo when dropped
 */
public class AttackScheduler
{
    private readonly AttackSchedule _schedule;

    private int _nextAttackIndex;
    
    // timers
    private float _bufferRemaining; // if attack becomes possible during this time we immediately execute it
    private float _attackLockRemaining;
    private float _comboWindowRemaining;

    // different states
    private bool _attackBuffered;
    private bool _continuousRequest;   // while input button is held continually request to attack
    private bool _comboActive;

    public AttackScheduler(AttackSchedule schedule)
    {
        _schedule = schedule;
    }

    public void RequestAttack()
    {
        _attackBuffered = true;
        _bufferRemaining = _schedule.InputBufferDuration;
    }

    public void SetContinuousRequest(bool active)
    {
        _continuousRequest = active;
    }

    public bool Tick(float deltaTime, out ScheduledAttack scheduledAttack)
    {
        scheduledAttack = default;

        _attackLockRemaining -= deltaTime;
        
        if (_comboActive && _attackLockRemaining <= 0f)
        {
            _comboWindowRemaining -= deltaTime;
            
            if (_comboWindowRemaining <= 0f)
            {
                _comboActive = false;
                _nextAttackIndex = 0;
            }
        }

        bool attackRequested = _attackBuffered || (_schedule.RepeatWhileHeld && _continuousRequest);

        if (_attackLockRemaining <= 0f && attackRequested)
        {
            _attackBuffered = false;
            _bufferRemaining = 0f;

            AttackTiming timing = _schedule.GetTiming(_nextAttackIndex);
            scheduledAttack = new ScheduledAttack(_nextAttackIndex, timing.Duration);
            _attackLockRemaining = timing.Duration + timing.Recovery;

            if (_nextAttackIndex >= _schedule.SequenceLength - 1)
            {
                // end of combo
                _nextAttackIndex = 0;
                _comboActive = false;
                _comboWindowRemaining = 0f;
            }
            else
            {
                // advance the combo
                _nextAttackIndex++;
                _comboActive = true;
                _comboWindowRemaining = _schedule.ComboContinuationWindow;
            }

            return true;
        }

        if (_attackBuffered)
        {
            _bufferRemaining -= deltaTime;
            if (_bufferRemaining <= 0f)
            {
                _attackBuffered = false;
            }
        }

        return false;
    }

    public void Reset()
    {
        _nextAttackIndex = 0;
        _bufferRemaining = 0f;
        _attackLockRemaining = 0f;
        _comboWindowRemaining = 0f;
        _attackBuffered = false;
        _continuousRequest = false;
        _comboActive = false;
    }
}
