using UnityEngine;

namespace Lab50033
{
    public class BossAnimationEvents : MonoBehaviour
    {
        [SerializeField] private SwingAttack swingAttack;
        private Boss _boss;

        private void Awake()
        {
            _boss = GetComponentInParent<Boss>();
        }

        public void EndAttack()
        {
            _boss.EndAttack();
        }

        // Animation Event: frame the blade starts swinging
        public void EnableHitbox()
        {
            swingAttack.EnableHitbox();
        }

        // Animation Event: frame the swing finishes
        public void DisableHitbox()
        {
            swingAttack.DisableHitbox();
        }
        public void EndSpawn()
        {
            _boss.EndSpawn();
        }

        public void DestroySelf()
        {
            _boss.DestroySelf();
        }

    }
}