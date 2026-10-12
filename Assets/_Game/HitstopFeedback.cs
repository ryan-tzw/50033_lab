using UnityEngine;

[RequireComponent(typeof(LocalTime))]
public class HitstopFeedback : MonoBehaviour
{
    private LocalTime _localTime;

    private void Awake()
    {
        _localTime = GetComponent<LocalTime>();
    }

    public void ApplyHitstop(HitEventData data)
    {
        if (data.AttackerTime != _localTime && data.TargetTime != _localTime) return;
        _localTime.Pause(data.Hit.HitstopDuration);
    }
}

