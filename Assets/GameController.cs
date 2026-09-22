using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private PlayerInputManager playerInputManager;
    private int score;

    private Player _player;

    public void InitializePlayerReference(PlayerInput playerInput)
    {
        Debug.Log("PLAYER JOINED");
        Player player = playerInput.GetComponent<Player>();
        if (_player != null)
        {
            _player.OnDeath -= GameOver;
        }

        _player = player;
        _player.OnDeath += GameOver;
        healthDisplay.ReferencePlayer(player);
    }

    public void ResetValues()
    {
        score = 0;
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        enemySpawner.OnEnemySpawned += SubscribeToEnemy;
        playerInputManager.onPlayerJoined += InitializePlayerReference;
    }

    private void OnDisable()
    {
        enemySpawner.OnEnemySpawned -= SubscribeToEnemy;
        playerInputManager.onPlayerJoined -= InitializePlayerReference;
    }

    private void SubscribeToEnemy(Enemy enemy)
    {
        enemy.OnDeath += addPoints;
    }

    private void addPoints(Enemy enemy, int points)
    {
        score += points;
    }

   

    private void GameOver()
    {
        gameOverScreen.Setup(score);
        Time.timeScale = 0f;
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnDeath -= GameOver;
        }
    }
}