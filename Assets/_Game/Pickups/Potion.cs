using UnityEngine;

public class Potion : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _worldCollider;
    private Collider _pickupCollider;
    [SerializeField] private int healAmount = 1;
    [SerializeField] private GameEvent potionCollectedEvent;

    private void OnTriggerEnter(Collider collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if(player == null)
        {
            return;
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (player && player.ReceiveHeal())
        {
            playerHealth.RestoreHealth(healAmount);
            potionCollectedEvent.Raise();
            Destroy(gameObject);
        }
    }

}
