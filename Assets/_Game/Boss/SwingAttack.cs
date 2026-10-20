using UnityEngine;

namespace Lab50033
{
    public class SwingAttack : MonoBehaviour
    {
        [SerializeField, Min(1)] private int damage = 1;
        [SerializeField, Min(0f)] private float knockback = 10f;
        [SerializeField, Min(0f)] private float hitstunDuration = 0.2f;
        [SerializeField, Min(0f)] private float recoveryDuration = 0.35f;
        [SerializeField] private HitConfirmedEvent hitConfirmedEvent;
        private BoxCollider SwingHitBox;
        private Boss _boss;
        private bool _hasHit;

        private void Awake()
        {
            SwingHitBox = GetComponent<BoxCollider>();
            SwingHitBox.enabled = false;
            _boss = GetComponentInParent<Boss>();
        }
        public void EnableHitbox()
        {
            _hasHit = false;
            SwingHitBox.enabled = true;
        }

        public void DisableHitbox()
        {
            SwingHitBox.enabled = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_hasHit) return;

            Player player = other.GetComponentInParent<Player>();
            if (player == null) return;

            var dir = player.transform.position - transform.position;
            dir.y = 0f;

            var hit = new HitData(damage, dir.normalized, knockback, hitstunDuration);
            if (!player.ReceiveHit(hit)) return;

            _hasHit = true;

            var hitPos = other.ClosestPoint(transform.position);
            hitConfirmedEvent.Raise(new HitEventData(_boss.LocalTime, player.LocalTime, hit, hitPos));

        }
    }
}
