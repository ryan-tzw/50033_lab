using System;
using UnityEngine;

[Serializable]
public struct AttackTiming
{
    [SerializeField, Min(0f)] private float duration;
    [SerializeField, Min(0f)] private float recovery;
    
    public float Duration => duration;
    public float Recovery => recovery;

    public AttackTiming(float duration, float recovery)
    {
        this.duration = duration;
        this.recovery = recovery;
    }
}

[CreateAssetMenu(fileName = "AttackSchedule", menuName = "Combat/AttackSchedule")]
public class AttackSchedule : ScriptableObject
{
    [SerializeField] private AttackTiming[] attackString =
    {
        new AttackTiming(0.20f, 0.08f),
        new AttackTiming(0.20f, 0.08f),
        new AttackTiming(0.25f, 0.25f)
    };
    [SerializeField]         private bool  repeatWhileHeld         = true;
    [SerializeField, Min(0)] private float inputBufferDuration     = 0.15f;
    [SerializeField, Min(0)] private float comboContinuationWindow = 0.30f;
    
    public int SequenceLength => attackString.Length;
    public bool RepeatWhileHeld => repeatWhileHeld;
    public float InputBufferDuration => inputBufferDuration;
    public float ComboContinuationWindow => comboContinuationWindow;

    public AttackTiming GetTiming(int attackIndex)
    {
        return attackString[attackIndex];
    }
}
