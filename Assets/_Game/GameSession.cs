using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSession : MonoBehaviour
{
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CoinPool coinPool;
    [SerializeField] private IntVariable runScore;

    private void OnEnable()
    {
        coinPool.CoinCollected += HandleCoinCollected;
    }

    private void OnDisable()
    {
        coinPool.CoinCollected -= HandleCoinCollected;
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        
        healthDisplay.ReferencePlayer(player.GetComponent<PlayerHealth>());
    }

    private void HandleCoinCollected()
    {
        runScore.Add(1);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    
}