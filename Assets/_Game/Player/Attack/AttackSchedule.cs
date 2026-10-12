using System;
using UnityEngine;

[Serializable]
public struct AttackStep
{
    [SerializeField, Min(0f)] private float animDuration;
    [SerializeField, Min(0f)] private float hitboxDuration;
    [SerializeField, Min(0f)] private float recovery;
    [SerializeField, Min(0)]  private int   damage;
    [SerializeField, Min(0f)] private float knockback;
    [SerializeField, Range(0f, 1f)] private float hitstopDuration;
    [SerializeField, Min(0f)] private float forwardSpeed;
    [SerializeField] private AudioClip sound;
    
    public float AnimDuration => animDuration;
    public float HitboxDuration => hitboxDuration;
    public float Recovery => recovery;
    public int Damage => damage;
    public float Knockback => knockback;
    public float HitstopDuration => hitstopDuration;
    public  float ForwardSpeed => forwardSpeed;
    public AudioClip Sound => sound;

    public AttackStep
    (
        float animDuration,
        float hitboxDuration,
        float recovery,
        int   damage,
        float knockback,
        float hitstopDuration = 0f,
        float forwardSpeed = 0f
    )
    {
        this.animDuration = animDuration;
        this.hitboxDuration = hitboxDuration;
        this.recovery = recovery;
        this.damage = damage;
        this.knockback = knockback;
        this.hitstopDuration = hitstopDuration;
        this.forwardSpeed = forwardSpeed;
        sound = null;
    }
}

[CreateAssetMenu(fileName = "AttackSchedule", menuName = "SO/Combat/AttackSchedule", order = -1)]
public class AttackSchedule : ScriptableObject
{
    [SerializeField] private AttackStep[] attackString =
    {
        new AttackStep(0.20f, 0.15f, 0.20f, 1, 1f),
        new AttackStep(0.20f, 0.15f, 0.20f, 1, 1f),
        new AttackStep(0.25f, 0.15f, 0.25f, 1, 5f)
    };
    
    [SerializeField]         private bool  repeatWhileHeld         = true;
    [SerializeField, Min(0)] private float inputBufferDuration     = 0.15f;
    [SerializeField, Min(0)] private float comboContinuationWindow = 0.30f;
    
    public int SequenceLength => attackString.Length;
    public bool RepeatWhileHeld => repeatWhileHeld;
    public float InputBufferDuration => inputBufferDuration;
    public float ComboContinuationWindow => comboContinuationWindow;

    public AttackStep GetAttack(int attackIndex)
    {
        return attackString[attackIndex];
    }
}
