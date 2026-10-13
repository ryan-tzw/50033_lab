using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PotionPickupAudio : MonoBehaviour
{
    [SerializeField] private AudioClip pickupClip;


    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayPickupSound()
    {
        _audioSource.PlayOneShot(pickupClip);
    }
}