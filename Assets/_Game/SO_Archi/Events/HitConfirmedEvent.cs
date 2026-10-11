using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HitConfirmedEvent", menuName = "SO/Events/HitConfirmed", order = -1)]
public class HitConfirmedEvent : ScriptableObject
{
    public event Action<HitEventData> Raised;
    
    public int RaiseCount { get; private set; }
    public HitEventData LastEvent { get; private set; }

    public void Raise(HitEventData data)
    {
        RaiseCount++;
        LastEvent = data;
        Raised?.Invoke(data);
    }
}