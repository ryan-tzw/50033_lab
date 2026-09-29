using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MagnetPickupAudio : MonoBehaviour
{
    [SerializeField] private MagnetPool magnetPool;
    [SerializeField] private AudioClip pickupClip;

    [SerializeField, Min(0.01f)] private float basePitch = 1f;
    [SerializeField, Min(0f)] private float randomPitchVariation = 0.03f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        magnetPool.MagnetCollected += PlayPickupSound;
    }

    private void OnDisable()
    {
        magnetPool.MagnetCollected -= PlayPickupSound;
    }

    private void PlayPickupSound()
    {
        var randomOffset = Random.Range(-randomPitchVariation, randomPitchVariation);

        _audioSource.pitch = basePitch + randomOffset;

        _audioSource.PlayOneShot(pickupClip);
    }
}
