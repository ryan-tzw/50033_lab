using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    private static readonly int DissolveAmountId = Shader.PropertyToID("_DissolveAmount");
    private static readonly int NoiseOffsetId = Shader.PropertyToID("_NoiseOffset");

    [SerializeField, Min(0.01f)] private float dissolveDuration = 1f;

    private SpriteRenderer _sr;
    private MaterialPropertyBlock _mpb;

    private float _elapsed;
    private bool _isPlaying;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        _elapsed = 0f;
        _isPlaying = false;

        _sr.GetPropertyBlock(_mpb);
        _mpb.SetFloat(DissolveAmountId, 0f);
        _mpb.SetVector( NoiseOffsetId, new Vector4( Random.Range(0f, 100f), Random.Range(0f, 100f), 0f, 0f));
        _sr.SetPropertyBlock(_mpb);
    }

    public void Begin()
    {
        _elapsed = 0f;
        _isPlaying = true;
    }

    // returns true when finished
    public bool Tick(float deltaTime)
    {
        if (!_isPlaying) return false;

        _elapsed += deltaTime;
        var progress = Mathf.Min(_elapsed / dissolveDuration, 1f);

        _sr.GetPropertyBlock(_mpb);
        _mpb.SetFloat(DissolveAmountId, progress);
        _sr.SetPropertyBlock(_mpb);

        if (progress < 1f) return false;
        
        _isPlaying = false;
        return true;
    }
}