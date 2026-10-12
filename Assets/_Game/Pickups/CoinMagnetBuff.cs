using UnityEngine;

public class CoinMagnetBuff : MonoBehaviour
{
    [SerializeField] private CoinRuntimeSet activeCoins;

    private float _remainingDuration;
    private bool _isActive;

    private void Update()
    {
        if (!_isActive) return;

        foreach (var coin in activeCoins.Items)
        {
            if (coin) coin.AttractTo(transform);
        }
        
        _remainingDuration -= Time.deltaTime;

        if (!(_remainingDuration <= 0f)) return;
        
        _isActive = false;
            
        foreach (var coin in activeCoins.Items)
        {
            if (coin) coin.StopAttracting(transform);
        }
    }

    private void OnDisable()
    {
        if (!_isActive) return;
        
        _isActive = false;
        _remainingDuration  = 0f;
        foreach (var coin in activeCoins.Items)
        {
            if (coin) coin.StopAttracting(transform);
        }
    }

    public void Activate(float duration)
    {
        _remainingDuration = duration;
        _isActive = duration > 0f;

        foreach (var coin in activeCoins.Items)
        {
            if (coin) coin.AttractTo(transform);
        }
    }
}