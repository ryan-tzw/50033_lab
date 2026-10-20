using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private readonly HashSet<IHittable> _hitTargets = new();
    private HitData _hitData;
    private LocalTime _attackerTime;
    private HitConfirmedEvent _hitConfirmed;

    public void Activate(HitData hitData, LocalTime attackerTime, HitConfirmedEvent hitConfirmed)
    {
        _hitData = hitData;
        _attackerTime = attackerTime;
        _hitConfirmed = hitConfirmed;
        _hitTargets.Clear();
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Enemy target = other.GetComponentInParent<Enemy>();
        IHittable target = other.GetComponentInParent<IHittable>();

        if (target is null || !_hitTargets.Add(target)) return;

        if (!target.ReceiveHit(_hitData)) return;
        
        var hitPos = other.ClosestPoint(transform.position);
        _hitConfirmed.Raise(new HitEventData(_attackerTime, target.LocalTime, _hitData, hitPos));
    }
    
}