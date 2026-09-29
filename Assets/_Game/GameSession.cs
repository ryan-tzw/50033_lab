using UnityEngine;
using UnityEngine.InputSystem;

public class GameSession : MonoBehaviour
{
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CoinPool coinPool;
    [SerializeField] private GameOverScreen gameOverScreen;

    private Player _player;
    private int _score;

    private void OnEnable()
    {
        coinPool.CoinCollected += HandleCoinCollected;
        if (_player is not null) _player.Died += HandlePlayerDied;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        
        healthDisplay.ReferencePlayer(player.GetComponent<PlayerHealth>());

        if (_player is not null) _player.Died -= HandlePlayerDied;
        
        _player = player.GetComponent<Player>();
        _player.Died += HandlePlayerDied;
    }

    private void HandleCoinCollected()
    {
        _score++;
    }

    private void HandlePlayerDied()
    {
        gameOverScreen.Show(_score);
        Time.timeScale = 0;
    }
    
}