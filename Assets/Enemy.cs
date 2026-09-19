using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;

    [SerializeField] private float hitstunDuration = 0.15f;
    [SerializeField] private float recoilSpeed = 5f;

    private EnemyState _state = EnemyState.Alive;
    private float _hitstunEndTime;
    private Rigidbody2D _rb;

    private enum EnemyState
    {
        Alive,
        Hitstunned,
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        _state = EnemyState.Hitstunned;
        _hitstunEndTime = Time.time + hitstunDuration;

        // todo: die when hp <= 0
        _currentHealth -= damage;
        
        Vector2 recoilDir = (_rb.position - attackerPosition).normalized;
        _rb.linearVelocity = recoilDir * recoilSpeed;
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Hitstunned:
            {
                if (Time.time > _hitstunEndTime)
                {
                    _state = EnemyState.Alive;
                    _rb.linearVelocity = Vector2.zero;
                }
                break;
            }
        }
        
    }
}
