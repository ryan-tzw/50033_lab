using UnityEngine;
using UnityEngine.InputSystem;

public class GameSession : MonoBehaviour
{
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private Transform spawnPoint;
    
    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        
        healthDisplay.ReferencePlayer(player.GetComponent<PlayerHealth>());
    }
}