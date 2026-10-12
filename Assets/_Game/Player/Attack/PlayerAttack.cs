using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private MeleeAttack attackPrefab;
    [SerializeField] private AttackSchedule attackSchedule;

    public bool IsAttacking => _attackScheduler.IsAttackActive;
    public Vector3 AttackDirection { get; private set; }
    public float AttackForwardSpeed { get; private set; }
    public float AttackDuration { get; private set; }
    
    private Vector3 _facingDirection = Vector3.right;
    
    // raycast from camera to compute the direction to attack
    [SerializeField] private Camera worldCamera;

    private AttackScheduler _attackScheduler;
    private LocalTime _localTime;
    private AudioSource _audioSource;
    private PlayerInput _playerInput;
    private InputAction _attackAction;
    private InputAction _aimAction;
    private InputAction _moveAction;

    private void Awake()
    {
        _attackScheduler = new AttackScheduler(attackSchedule);
        _localTime = GetComponent<LocalTime>();
        _audioSource = GetComponent<AudioSource>();
        _playerInput = GetComponent<PlayerInput>();
        var combatActions = _playerInput.actions.FindActionMap("Combat");

        _attackAction = combatActions.FindAction("Attack");
        _aimAction = combatActions.FindAction("Aim");
        _moveAction = combatActions.FindAction("Move");

        if (worldCamera == null)
        {
            worldCamera = _playerInput.camera != null ? _playerInput.camera : Camera.main;
        }
    }

    private void Update()
    {
        var aimInput = _aimAction.ReadValue<Vector2>();

        // aiming
        if (_playerInput.currentControlScheme == "KBM")
        {
            var mouseViewport = new Vector2(aimInput.x / Screen.width, aimInput.y / Screen.height);
            var mouseRay = worldCamera.ViewportPointToRay(mouseViewport);
            // note: assuming NO elevation changes. rewrite this if we make terrain with variation in elevation
            var groundPlane = new Plane(Vector3.up, transform.position);

            if (groundPlane.Raycast(mouseRay, out float distance))
            {
                var direction = mouseRay.GetPoint(distance) - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.01f)
                {
                    _facingDirection = direction.normalized;
                }
            }
        }
        else
        {
            var directionInput = aimInput.sqrMagnitude > 0.01f ? aimInput : _moveAction.ReadValue<Vector2>();

            if (directionInput.sqrMagnitude > 0.01f)
            {
                var cameraRight = Vector3.ProjectOnPlane(worldCamera.transform.right, Vector3.up).normalized;
                var cameraForward = Vector3.ProjectOnPlane(worldCamera.transform.forward, Vector3.up).normalized;
                _facingDirection = (cameraRight * directionInput.x + cameraForward * directionInput.y).normalized;
            }
        }
        
        // attack
        if (_attackAction.WasPressedThisFrame())
        {
            _attackScheduler.RequestAttack();
        }
        
        _attackScheduler.SetContinuousRequest(_attackAction.IsPressed());

        if (_localTime.IsPaused) return;
        if (_attackScheduler.Tick(_localTime.DeltaTime, out ScheduledAttack scheduledAttack))
        {
            AttackDirection = _facingDirection;
            AttackForwardSpeed = scheduledAttack.Step.ForwardSpeed;
            AttackDuration = scheduledAttack.Step.AnimDuration;
            
            var attackPosition = transform.position + new Vector3(0f, 0.25f, 0f);
            var attack = Instantiate(attackPrefab, attackPosition, Quaternion.identity, transform);
            attack.Spawn(_facingDirection, scheduledAttack.Index, scheduledAttack.Step, _localTime);

            if (scheduledAttack.Step.Sound)
            {
                _audioSource.PlayOneShot(scheduledAttack.Step.Sound);
            }
        }

    }

    private void OnDisable()
    {
        _attackScheduler?.Reset();
    }
}
