using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private PlayerRuntimeSet activePlayers;
    
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField, Min(0f)] private float knockbackDecel = 12f;
    
    private static readonly int IsMovingId = Animator.StringToHash("IsMoving");

    private Rigidbody _rb;
    private SpriteRenderer _sr;
    private Animator _animator;
    private LocalTime _localTime;
    
    private Player _target;
    private bool _movementEnabled;
    private bool _animationPaused;
    private Vector3 _knockbackVel;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _localTime = GetComponent<LocalTime>();
    }

    private void OnEnable()
    {
        // reset everything since we're using a pool
        _target = null;
        _animationPaused = false;
        _knockbackVel = Vector3.zero;
        _rb.linearVelocity = Vector3.zero;
        _sr.flipX = false;
        _animator.speed = 1f;
        _animator.SetBool(IsMovingId, false);
    }

    private void Update()
    {
        _animator.speed = _animationPaused || _localTime.IsPaused ?  0f : 1f;
    }

    private void FixedUpdate()
    {
        if (_localTime.IsPaused)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            return;
        }

        if (_knockbackVel.sqrMagnitude > 0.0001f)
        {
            _rb.linearVelocity = new Vector3(_knockbackVel.x, _rb.linearVelocity.y, _knockbackVel.z);
            _knockbackVel = Vector3.MoveTowards(_knockbackVel, Vector3.zero, knockbackDecel * Time.fixedDeltaTime);
            _animator.SetBool(IsMovingId, false);
            return;
        }
        
        if (!_movementEnabled)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            _animator.SetBool(IsMovingId, false);
            return;
        }

        if (!_target || !activePlayers.Contains(_target))
        {
            _target = FindClosestPlayer();
        }
        if (!_target)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            _animator.SetBool(IsMovingId, false);
            return;
        }

        var moveDir = _target.transform.position - _rb.position;
        moveDir.y = 0f;

        if (moveDir.sqrMagnitude < 0.001f)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            _animator.SetBool(IsMovingId, false);
            return;
        }
        
        moveDir.Normalize();

        _rb.linearVelocity = new Vector3( moveDir.x * moveSpeed, _rb.linearVelocity.y, moveDir.z * moveSpeed);
        
        _animator.SetBool(IsMovingId, true);
        
        if (Mathf.Abs(moveDir.x) > 0.01f)
        {
            _sr.flipX = moveDir.x < 0f;
        }
    }

    public void SetMovementEnabled(bool movementEnabled)
    {
        _movementEnabled = movementEnabled;
    }

    public void SetAnimationPaused(bool animationPaused)
    {
        _animationPaused = animationPaused;
        _animator.speed = _animationPaused || _localTime.IsPaused ? 0f : 1f;
    }

    public void ApplyKnockback(Vector3 dir, float str)
    {
        dir.y = 0f;
        _knockbackVel += dir.normalized * str;
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
}
