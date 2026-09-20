using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float hitstopDuration = 0.05f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        Player player = GetComponentInParent<Player>();

        if (enemy == null || player == null) return;
        
        enemy.TakeDamage(damage, transform.root.position); // transform.root.position gets the Player's position rather than the position of the attack itself
        player.ApplyHitstop(hitstopDuration);
    }
}
