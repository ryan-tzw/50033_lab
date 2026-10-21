using UnityEngine;
using System.Collections;

namespace Lab50033
{
    public class Boss : MonoBehaviour, IHittable
    {
        private Rigidbody _rb;
        private Animator _animator;
        private BoxCollider HitBoxCollider;
        private SpriteRenderer _sr;
        private SphereCollider AttackRange;
        private Player _target;
        private static readonly int IsMovingId = Animator.StringToHash("IsMoving");
        private static readonly int IsAttackingId = Animator.StringToHash("IsAttacking");
        public LocalTime LocalTime { get; private set; }
        private bool _isDead;
        [SerializeField] private BoxCollider swingHitbox;

        [SerializeField] private Color flashColor = Color.red;
        [SerializeField] private float flashDuration = 0.1f;


        [SerializeField] private int BossPhase = 0;
        [SerializeField] private int BossMaxHealth = 50;
        [SerializeField] private int BossCurrentHealth;
        [SerializeField] private float BossMovementSpeed = 10.0f;
        [SerializeField] private PlayerRuntimeSet activePlayers;

        [SerializeField] private float chaseDelay = 2f;
        [SerializeField] private float arriveThreshold = 0.6f;

        [SerializeField] private float attackDelay = 0.8f;

        [SerializeField] private float stopDistance = 2f;   


        private bool InRange = false;
        private float chaseTimer = 0.0f;
        private float attackDelayTimer = 0.0f;
        private bool IsAttacking = false;
        private bool _isSpawning; 
        public bool IsDepleted { get; private set; }
        private Coroutine _flashRoutine;
        [SerializeField] private GameEvent bossDefeatedEvent;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _sr = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            HitBoxCollider = GetComponentInChildren<BoxCollider>();
            BossCurrentHealth = BossMaxHealth;
            LocalTime = GetComponent<LocalTime>();

        }

        private void OnEnable()
        {
            _isSpawning = true;
            IsDepleted = false;
            _animator.Play("Spawn", 0, 0f); 
        }

        private void FixedUpdate()
        {
            if (_isDead) return;
            if (_isSpawning)
            {
                StopMoving();
                return;     
            }

            if (!_target || !activePlayers.Contains(_target))
                _target = FindClosestPlayer();

            if (!_target)
            {
                StopMoving();
                return;
            }

            

            FollowPlayer();
        }

        private void FollowPlayer()
        {
            if (IsAttacking)
            {
                StopMoving();
                return;
            }

            var playerPos = _target.transform.position;

            float side = _rb.position.x < playerPos.x ? -1f : 1f;
            var standPoint = new Vector3(playerPos.x + side * stopDistance, _rb.position.y, playerPos.z);

            var toStand = standPoint - _rb.position;
            toStand.y = 0f;

            InRange = toStand.sqrMagnitude <= arriveThreshold * arriveThreshold;

            FaceDirection(playerPos.x - _rb.position.x);
            
            if (InRange)
            {
                chaseTimer = chaseDelay;
                StopMoving();
                TryAttack();
                return;
            }

            if (chaseTimer > 0f)
            {
                chaseTimer -= Time.fixedDeltaTime;
                StopMoving();
                return;
            }

            var moveDir = toStand.normalized;
            _rb.linearVelocity = new Vector3(moveDir.x * BossMovementSpeed, _rb.linearVelocity.y, moveDir.z * BossMovementSpeed);
            _animator.SetBool(IsMovingId, true);
        }

        private void StopMoving()
        {
            if (!_rb.isKinematic)
            {
                _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            }
            _animator.SetBool(IsMovingId, false);
        }

        private void FaceDirection(float x)
        {
            if (IsAttacking) return;
            if (Mathf.Abs(x) < 0.01f) return;

            var t = _sr.transform;
            var scale = t.localScale;
            scale.x = Mathf.Abs(scale.x) * (x < 0f ? -1f : 1f);
            t.localScale = scale;

            var center = swingHitbox.center;
            center.x = Mathf.Abs(center.x) * (x < 0f ? -1f : 1f);
            swingHitbox.center = center;
        }

        private Player FindClosestPlayer()
        {
            Player closestPlayer = null;
            var closestDistanceSqr = Mathf.Infinity;

            foreach (var player in activePlayers.Items)
            {
                if (!player) continue;
                var diff = player.transform.position - transform.position;
                diff.y = 0f;
                var distanceSqr = diff.sqrMagnitude;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestPlayer = player;
                }
            }

            return closestPlayer;
        }

        private void TryAttack()
        {
            if (IsAttacking) return;

            attackDelayTimer -= Time.fixedDeltaTime;

            if (attackDelayTimer <= 0f)
            {
                IsAttacking = true;
                attackDelayTimer = attackDelay;  
                SwingAttack();
            }
        }

        public void EndAttack()
        {
            IsAttacking = false;
            _animator.SetBool(IsAttackingId, false);
        }

        public void EndSpawn()
        {
            Debug.Log("EndSpawn called");
            _isSpawning = false;
        }

        private void SwingAttack()
        {
            _animator.SetBool(IsMovingId, false);
            _animator.SetBool(IsAttackingId, true);

        }
        public bool ReceiveHit(HitData hit)
        {
            if (_isSpawning || _isDead) return false;   

            BossCurrentHealth = Mathf.Max(0, BossCurrentHealth - hit.Damage);

            Debug.Log($"Boss hit! HP: {BossCurrentHealth}/{BossMaxHealth}");

            if (_flashRoutine != null) StopCoroutine(_flashRoutine);
            _flashRoutine = StartCoroutine(Flash());

            if (BossCurrentHealth == 0)
            {
                Die();
            }
            return true;
        }

        private void Die()
        {
            _isDead = true;
            StopMoving();
            IsAttacking = false;
            swingHitbox.enabled = false;
            HitBoxCollider.enabled = false;

            _rb.isKinematic = true;

            _animator.Play("Death", 0, 0f);
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
            bossDefeatedEvent.Raise();
        }

        private IEnumerator Flash()
        {
            _sr.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            _sr.color = Color.white;
            _flashRoutine = null;
        }
    }
}
