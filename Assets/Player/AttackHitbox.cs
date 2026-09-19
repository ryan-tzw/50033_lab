using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        enemy?.TakeDamage(damage, transform.root.position); // transform.root.position gets the Player's position rather than the position of the attack itself
   }
}
