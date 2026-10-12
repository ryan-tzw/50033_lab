using System;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [SerializeField] private MeleeHitbox normalHitbox;
    [SerializeField] private MeleeHitbox finisherHitbox;
    [SerializeField] private HitConfirmedEvent hitConfirmedEvent;

    private LocalTime _ownerTime;
    private MeleeHitbox _hitbox;
    private float _elapsed;
    private float _hitboxDuration;
    private float _animationDuration;
    private bool _hitboxDisabled;

    private static readonly int[] AttackStateIds =
    {
        Animator.StringToHash("Base Layer.Attack1"),
        Animator.StringToHash("Base Layer.Attack2"),
        Animator.StringToHash("Base Layer.Attack3")
    };

    private void Update()
    {
        animator.speed = _ownerTime.IsPaused ? 0f : 1f;
        _elapsed += _ownerTime.DeltaTime;

        if (!_hitboxDisabled && _elapsed >= _hitboxDuration)
        {
            _hitbox.gameObject.SetActive(false);
            _hitboxDisabled = true;
        }

        if (_elapsed >= _animationDuration)
        {
            Destroy(gameObject);
        }
    }

    public void Spawn(Vector3 direction, int comboIndex, AttackStep attack, LocalTime ownerTime)
    {
        _ownerTime = ownerTime;
        _elapsed = 0f;
        _hitboxDuration = attack.HitboxDuration;
        _animationDuration = attack.AnimDuration;
        _hitboxDisabled = false;
        
        // this is really dumb but i have to make the sprite stand upright (aligned on the XY plane) so that 
        // it will actually display in Unity's Project preview because they just assume all sprites are aligned to XY
        // which means i have to rotate it here to align it back to the XZ plane for our actual use case
        // i could ignore the issue but it was bugging me that the preview was empty and its not that hard a fix so...
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
        
        normalHitbox.gameObject.SetActive(false);
        finisherHitbox.gameObject.SetActive(false);
        
        bool isFinisher = comboIndex == 2;
        _hitbox = isFinisher ? finisherHitbox : normalHitbox;

        var hitData = new HitData(attack.Damage, direction, attack.Knockback, hitstopDuration: attack.HitstopDuration);
        _hitbox.Activate(hitData, _ownerTime, hitConfirmedEvent);
        
        animator.Play(AttackStateIds[comboIndex], 0, 0f);
    }
}
