using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 3;
    [SerializeField] private int currentHealth;

    public bool IsDepleted { get; private set; }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        IsDepleted = false;
    }

    public void ReceiveHit(HitData hit)
    {
        if (IsDepleted) return;

        currentHealth = Mathf.Max(0, currentHealth - hit.Damage);
        if (currentHealth == 0)
        {
            IsDepleted = true;
        }
    }
}