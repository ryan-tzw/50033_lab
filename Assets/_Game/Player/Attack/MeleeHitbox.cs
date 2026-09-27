using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private readonly HashSet<EnemyHealth> _hitTargets = new();
    private HitData _hitData;

    public void Activate(HitData hitData, float duration)
    {
        _hitData = hitData;
        _hitTargets.Clear();
        gameObject.SetActive(true);
        StartCoroutine(DisableAfter(duration));
    }

    private IEnumerator DisableAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.name);
        EnemyHealth target = other.GetComponentInParent<EnemyHealth>();

        if (target is null || !_hitTargets.Add(target)) return;
        
        target.ReceiveHit(_hitData);
    }
    
}