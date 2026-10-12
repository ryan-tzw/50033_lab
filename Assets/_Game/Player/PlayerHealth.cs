using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Min(0f)] private float invulnDuration = 0.8f;
    [SerializeField, Min(1)] private int maxHealth = 3;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDepleted { get; private set; }
    
    private float _invulnRemaining;

    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        // reset
        CurrentHealth = maxHealth;
        _invulnRemaining = 0f;
        IsDepleted = false;
        
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    private void Update()
    {
        if (_invulnRemaining > 0f)
        {
            _invulnRemaining -= Time.deltaTime;
        }
    }

    // returns true when hit accepted false otherwise
    // to use later for sfx etc
    public bool ReceiveHit(HitData hit)
    {
        if (IsDepleted || _invulnRemaining > 0f) { return false; }

        CurrentHealth = Mathf.Max(0, CurrentHealth - hit.Damage);
        OnHealthChanged?.Invoke(CurrentHealth);

        _invulnRemaining = invulnDuration;

        if (CurrentHealth == 0)
        {
            IsDepleted = true;
        }

        return true;
    }

    public void RestoreHealth(int health)
    {
        CurrentHealth += health;
        OnHealthChanged?.Invoke(CurrentHealth);
    }
}