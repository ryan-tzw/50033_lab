using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField, Min(1)]  private int   damage = 1;
    [SerializeField, Min(0f)] private float knockback = 3f;
    [SerializeField, Min(0f)] private float hitstunDuration = 0.2f;
    [SerializeField, Min(0f)] private float recoveryDuration = 0.35f;

    private EnemyMovement _movement;
    private int _playerHitboxLayer;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _playerHitboxLayer = LayerMask.NameToLayer("PlayerHitbox");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != _playerHitboxLayer) return;
        
        var player = other.GetComponentInParent<Player>();
        if (player == null) return;
        var dir = player.transform.position - transform.position;
        dir.y = 0f;

        var hitAccepted = player.ReceiveHit( new HitData( damage, dir.normalized, knockback, hitstunDuration));

        if (hitAccepted)
        {
            _movement.Pause(recoveryDuration);
        }
    }
}