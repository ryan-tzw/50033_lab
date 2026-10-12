using UnityEngine;

[RequireComponent(typeof(LocalTime))]
[RequireComponent(typeof(AudioSource))]
public class HurtAudioFeedback : MonoBehaviour
{
    [SerializeField] private AudioClip hurtSound;
    
    private AudioSource _audioSrc;
    private LocalTime _localTime;

    private void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
        _localTime = GetComponent<LocalTime>();
    }

    public void PlayHurtSound(HitEventData data)
    {
        if (data.TargetTime != _localTime) return;
        if (!hurtSound) return;
        
        _audioSrc.PlayOneShot(hurtSound);
    }
}
