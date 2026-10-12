using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private IntVariable runScore;

    private void Awake()
    {
        Time.timeScale = 1;
        runScore.ResetValue();
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        
        healthDisplay.ReferencePlayer(player.GetComponent<PlayerHealth>());
    }

    public void HandleCoinCollected()
    {
        runScore.Add(1);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        runScore.ResetValue();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}