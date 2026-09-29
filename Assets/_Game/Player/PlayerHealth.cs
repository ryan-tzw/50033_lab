using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
    private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");

    [SerializeField, Min(0f)] private float invulnDuration = 0.8f;
    
    [SerializeField, Min(1)] private int maxHealth = 3;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    
    private float _invulnRemaining;
    private bool _isDepleted;

    private SpriteRenderer _sr;
    private MaterialPropertyBlock _mpb;

    public event Action<int> OnHealthChanged;
    public event Action Depleted;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _mpb = new MaterialPropertyBlock();
        
        // reset
        CurrentHealth = maxHealth;
        _invulnRemaining = 0f;
        _isDepleted = false;
        SetFlash(0f);
        
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    private void Update()
    {
        if (_invulnRemaining > 0f)
        {
            _invulnRemaining -= Time.deltaTime;
            if (_invulnRemaining <= 0f) { SetFlash(0f); }
        }
    }

    // returns true when hit accepted false otherwise
    // to use later for sfx etc
    public bool ReceiveHit(HitData hit)
    {
        if (_isDepleted || _invulnRemaining > 0f) { return false; }

        CurrentHealth = Mathf.Max(0, CurrentHealth - hit.Damage);
        OnHealthChanged?.Invoke(CurrentHealth);

        _invulnRemaining = invulnDuration;
        SetFlash(1f);

        if (CurrentHealth == 0)
        {
            _isDepleted = true;
            Depleted?.Invoke();
        }

        return true;
    }

    public void RestoreHealth(int health)
    {
        CurrentHealth += health;
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    private void SetFlash(float amount)
    {
        _sr.GetPropertyBlock(_mpb);
        _mpb.SetColor(FlashColorId, Color.HSVToRGB(0f, 0.7f, 0.8f));
        _mpb.SetFloat(FlashAmountId, amount);
        _sr.SetPropertyBlock(_mpb);
    }
}