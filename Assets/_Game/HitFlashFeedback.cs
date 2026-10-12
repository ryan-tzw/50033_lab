using UnityEngine;

[RequireComponent(typeof(LocalTime))]
public class HitFlashFeedback : MonoBehaviour
{
    private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
    private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");

    [SerializeField] private Color flashColor;
    [SerializeField, Range(0f,1f)] private float duration;

    private SpriteRenderer _sr;
    private MaterialPropertyBlock _mpr;
    private LocalTime _localTime;
    private float _remainingTime;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _mpr = new MaterialPropertyBlock();
        _localTime = GetComponent<LocalTime>();
    }

    private void OnEnable()
    {
        _remainingTime = 0f;
        SetFlash(0f);
    }

    private void OnDisable()
    {
        SetFlash(0f);
    }

    private void Update()
    {
        if (_remainingTime <= 0f) return;
        
        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0f)
        {
            SetFlash(0f);
        }
    }

    public void ApplyHitFlash(HitEventData data)
    {
        if (data.TargetTime != _localTime) return;
        _remainingTime = duration;
        SetFlash(1f);
    }

    private void SetFlash(float amount)
    {
        _sr.GetPropertyBlock(_mpr);
        _mpr.SetFloat(FlashAmountId, amount);
        _mpr.SetColor(FlashColorId, flashColor);
        _sr.SetPropertyBlock(_mpr);
    }
}

