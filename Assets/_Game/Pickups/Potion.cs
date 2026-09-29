using UnityEngine;

public class Potion : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _worldCollider;
    private Collider _pickupCollider;
    [SerializeField] private int healAmount = 1;

    //private void Awake()
    //{
    //    _rb = GetComponent<Rigidbody>();
    //    _worldCollider = GetComponent<Collider>();
    //    _playerHitboxLayer = LayerMask.NameToLayer("PlayerHitbox");

    //    foreach (var childCollider in GetComponentsInChildren<Collider>())
    //    {
    //        if (childCollider.isTrigger)
    //        {
    //            _pickupCollider = childCollider;
    //            break;
    //        }
    //    }
    //}
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
            Destroy(gameObject);
        }
    }

}
