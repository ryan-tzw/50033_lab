using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private IntVariable runScore;
    [SerializeField] private IntVariable killCount;
    [SerializeField] private PlayerInputManager pim;

    private void Awake()
    {
        Time.timeScale = 1;
        runScore.ResetValue();
        killCount.ResetValue();
    }

    private void Start()
    {
        pim.JoinPlayer(controlScheme: "KBM", pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current });
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

    public void HandleEnemyKilled()
    {
        killCount.Add(1);
        Debug.Log(killCount.Value);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        runScore.ResetValue();
        killCount.ResetValue();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }
    
}