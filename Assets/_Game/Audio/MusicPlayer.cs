using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [SerializeField] private AudioSource firstSource;
    [SerializeField] private AudioSource secondSource;
    [SerializeField] private AudioClip startingClip;
    [SerializeField] private float crossfadeDuration = 1f;

    private AudioSource _currentSource;
    private Coroutine _crossfadeCoroutine;
    private bool _isPrimaryInstance;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Instance.Play(startingClip, crossfadeDuration);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _isPrimaryInstance = true;
        DontDestroyOnLoad(gameObject);

        firstSource.playOnAwake = false;
        firstSource.loop = true;
        firstSource.volume = 0;

        secondSource.playOnAwake = false;
        secondSource.loop = true;
        secondSource.volume = 0;

        _currentSource = firstSource;
    }

    private void Start()
    {
        if (_isPrimaryInstance)
        {
            Play(startingClip, crossfadeDuration);
        }
    }

    public void Play(AudioClip clip, float duration)
    {
        if (!clip)
        {
            return;
        }

        if (_currentSource.clip == clip && _currentSource.isPlaying)
        {
            return;
        }

        if (_crossfadeCoroutine != null)
        {
            StopCoroutine(_crossfadeCoroutine);
        }

        AudioSource nextSource;
        if (firstSource.clip == clip && firstSource.isPlaying)
        {
            nextSource = firstSource;
        }
        else if (secondSource.clip == clip && secondSource.isPlaying)
        {
            nextSource = secondSource;
        }
        else
        {
            AudioSource louderSource = firstSource.volume >= secondSource.volume ? firstSource : secondSource;
            nextSource = louderSource == firstSource ? secondSource : firstSource;
            
            nextSource.Stop();
            nextSource.clip = clip;
            nextSource.volume = 0;
            nextSource.Play();
        }
        
        AudioSource prevSource = nextSource == firstSource ? secondSource : firstSource;
        _currentSource = nextSource;

        
        _crossfadeCoroutine = StartCoroutine(Crossfade(prevSource, nextSource, duration));
    }

    private IEnumerator Crossfade(AudioSource previousSource, AudioSource nextSource, float duration)
    {
        float previousStartVolume = previousSource.volume;
        float nextStartVolume = nextSource.volume;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            previousSource.volume = Mathf.Lerp(previousStartVolume, 0, progress);
            nextSource.volume = Mathf.Lerp(nextStartVolume, 1, progress);

            yield return null;
        }

        previousSource.Stop();
        previousSource.clip = null;
        previousSource.volume = 0;
        nextSource.volume = 1;

        _crossfadeCoroutine = null;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}