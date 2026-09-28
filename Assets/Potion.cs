using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if(player && collision == player.PlayerCollider && player.CanHeal())
        {
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
