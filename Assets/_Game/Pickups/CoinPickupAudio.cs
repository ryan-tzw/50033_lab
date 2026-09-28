using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinPickupAudio : MonoBehaviour
{
    [SerializeField] private CoinPool coinPool;
    [SerializeField] private AudioClip pickupClip;

    [SerializeField, Min(0.01f)] private float basePitch = 1f;
    [SerializeField, Min(0f)] private float pitchStep = 0.08f;
    [SerializeField, Min(0.01f)] private float maximumPitch = 1.5f;
    [SerializeField, Min(0f)] private float randomPitchVariation = 0.03f;
    [SerializeField, Min(0f)] private float chainWindow = 0.3f;

    private AudioSource _audioSource;
    private float _lastPickupTime = float.NegativeInfinity;
    private int _chainIndex;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        coinPool.CoinCollected += PlayPickupSound;
    }

    private void OnDisable()
    {
        coinPool.CoinCollected -= PlayPickupSound;
    }

    private void PlayPickupSound()
    {
        if (Time.time - _lastPickupTime > chainWindow)
        {
            _chainIndex = 0;
        }
        else
        {
            _chainIndex++;
        }

        var risingPitch = basePitch + _chainIndex * pitchStep;
        var randomOffset = Random.Range(-randomPitchVariation, randomPitchVariation);

        _audioSource.pitch = Mathf.Clamp(
            risingPitch + randomOffset,
            0.01f,
            maximumPitch
        );

        _audioSource.PlayOneShot(pickupClip);
        _lastPickupTime = Time.time;
    }
}