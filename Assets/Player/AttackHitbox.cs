using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
           Debug.Log($"Attack hit: {other.name}"); 
    }
}
