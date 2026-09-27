using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 3;
    [SerializeField] private int currentHealth;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void ReceiveHit(HitData hit)
    {
        currentHealth -= hit.Damage;

        if (currentHealth <= 0)
        {
            // todo: temporary for now before we add death animation and wtv
            gameObject.SetActive(false);
        }
    }
}