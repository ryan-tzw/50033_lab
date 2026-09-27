using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    
    private static readonly int IsMovingId = Animator.StringToHash("IsMoving");

    private Rigidbody _rb;
    private SpriteRenderer _sr;
    private Animator _animator;
    
    private EnemyHealth _health;
    private Transform _target;
    private float _pauseRemaining;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _health = GetComponent<EnemyHealth>();
    }

    private void FixedUpdate()
    {
        if (_health.IsDying)
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

        if (_target is null) { _target = FindClosestPlayer(); }
        if (_target is null)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            _animator.SetBool(IsMovingId, false);
            return;
        }

        var moveDir = _target.position - _rb.position;
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

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private Transform FindClosestPlayer()
    {
        Transform closestPlayer = null;
        var closestDistanceSqr = Mathf.Infinity;

        foreach (var player in PlayerInput.all)
        {
            var diff = player.transform.position - transform.position;
            diff.y = 0f;
            var distanceSqr = diff.sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestPlayer = player.transform;
            }
        }

        return closestPlayer;
    }
}