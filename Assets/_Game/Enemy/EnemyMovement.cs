using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private PlayerRuntimeSet activePlayers;
    
    [SerializeField] private float moveSpeed = 2f;
    
    private static readonly int IsMovingId = Animator.StringToHash("IsMoving");

    private Rigidbody _rb;
    private SpriteRenderer _sr;
    private Animator _animator;
    
    private Enemy _enemy;
    private Player _target;
    private float _pauseRemaining;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        // reset everything since we're using a pool
        _target = null;
        _pauseRemaining = 0f;
        _rb.linearVelocity = Vector3.zero;
        _sr.flipX = false;
        _animator.speed = 1f;
        _animator.SetBool(IsMovingId, false);
    }

    private void FixedUpdate()
    {
        if (_enemy.IsDying)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            _animator.SetBool(IsMovingId, false);
            _animator.speed = 0f;
            return;
        }

        if (_pauseRemaining > 0f)
        {
            _pauseRemaining -= Time.fixedDeltaTime;
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            // _animator.SetBool(IsMovingId, false);
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

    public void Pause(float duration)
    {
        if (duration > _pauseRemaining)
        {
            _pauseRemaining = duration;
        }
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
