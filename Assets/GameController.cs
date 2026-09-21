using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameOverScreen gameOverScreen;

    private Player _player;

    private void Start()
    {
        Player spawnPlayer = Instantiate(
            playerPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        InitializePlayerReference(spawnPlayer);
    }

    public void InitializePlayerReference(Player player)
    {
        if (_player != null)
        {
            _player.OnDeath -= GameOver;
        }

        _player = player;
        _player.OnDeath += GameOver;
    }

    private void GameOver()
    {
        gameOverScreen.Setup(0);
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