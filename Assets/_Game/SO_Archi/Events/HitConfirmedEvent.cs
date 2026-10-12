using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HitConfirmedEvent", menuName = "SO/Events/HitConfirmed", order = -1)]
public class HitConfirmedEvent : GameEvent<HitEventData>
{
    public int RaiseCount { get; private set; }
    public HitEventData LastEvent { get; private set; }

    public override void Raise(HitEventData data)
    {
        RaiseCount++;
        LastEvent = data;
        base.Raise(data);
    }
}