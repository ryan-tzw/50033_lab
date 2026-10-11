using UnityEngine;

public class LocalTime : MonoBehaviour
{
    private float _pauseRemaining;
    public bool IsPaused => _pauseRemaining > 0f;
    public float DeltaTime => IsPaused ? 0f : Time.deltaTime;

    private void OnEnable()
    {
        _pauseRemaining = 0f;
    }

    private void Update()
    {
        if (IsPaused)
        {
            _pauseRemaining -= Time.deltaTime;
        }
    }

    public void Pause(float duration)
    {
        _pauseRemaining = Mathf.Max(_pauseRemaining, duration);
    }
}

