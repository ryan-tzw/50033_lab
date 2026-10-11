using System;
using UnityEngine;

public class HitstopFeedback : MonoBehaviour
{
    [SerializeField] private HitConfirmedEvent hitConfirmed;

    private void OnEnable()
    {
        hitConfirmed.Raised += HandleHit;
    }

    private void OnDisable()
    {
        hitConfirmed.Raised -= HandleHit;
    }

    private void HandleHit(HitEventData data)
    {
        data.AttackerTime.Pause(data.Hit.HitstopDuration);
        data.TargetTime.Pause(data.Hit.HitstopDuration);
    }
}

