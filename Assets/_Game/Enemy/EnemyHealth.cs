using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private const float FlashDuration = 0.15f;

    private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
    private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");

    [SerializeField, Min(1)] private int maxHealth = 3;
    [SerializeField] private int currentHealth;

    private SpriteRenderer _sr;
    private MaterialPropertyBlock _mpb;

    private float _flashRemaining;
    private bool _isDepleted;
    public event Action Depleted;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        _flashRemaining = 0f;
        _isDepleted = false;
        SetFlash(0f);
    }

    private void Update()
    {
        if (_flashRemaining <= 0f) return;

        _flashRemaining -= Time.deltaTime;
        if (_flashRemaining <= 0f)
        {
            SetFlash(0f);
        }
    }

    public void ReceiveHit(HitData hit)
    {
        if (_isDepleted) return;

        _flashRemaining = FlashDuration;
        SetFlash(1f);

        currentHealth = Mathf.Max(0, currentHealth - hit.Damage);
        if (currentHealth == 0)
        {
            _isDepleted = true;
            Depleted?.Invoke();
        }
    }

    private void SetFlash(float amount)
    {
        _sr.GetPropertyBlock(_mpb);
        _mpb.SetFloat(FlashAmountId, amount);
        _mpb.SetColor(FlashColorId, Color.white);
        _sr.SetPropertyBlock(_mpb);
    }
}