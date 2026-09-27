using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private readonly HashSet<EnemyHealth> _hitTargets = new();
    private HitData _hitData;

    public void Init(HitData hitData)
    {
        _hitData = hitData;
        _hitTargets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.name);
        EnemyHealth target = other.GetComponentInParent<EnemyHealth>();

        if (target is null || !_hitTargets.Add(target)) return;
        
        target.ReceiveHit(_hitData);
    }
    
}