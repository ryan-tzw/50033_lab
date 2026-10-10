using UnityEngine;
using UnityEngine.InputSystem;

public class GameSession : MonoBehaviour
{
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CoinPool coinPool;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private IntVariable runScore;

    private Player _player;

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
        runScore.Add(1);
    }

    private void HandlePlayerDied()
    {
        gameOverScreen.Show();
        Time.timeScale = 0;
    }
    
}