using UnityEngine;

public readonly struct HitEventData
{
    public LocalTime AttackerTime { get; }
    public LocalTime TargetTime { get; }
    public HitData Hit { get; }
    public Vector3 Position { get; }

    public HitEventData(LocalTime attackerTime, LocalTime targetTime, HitData hit, Vector3 position)
    {
        AttackerTime = attackerTime;
        TargetTime = targetTime;
        Hit = hit;  
        Position = position;
    }
}