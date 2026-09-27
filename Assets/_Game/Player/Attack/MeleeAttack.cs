using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [SerializeField] private MeleeHitbox normalHitbox;
    [SerializeField] private MeleeHitbox finisherHitbox;
    
    private float _lifetime = 0.2f;
    private float _finisherLifetime = 0.25f;

    private static readonly int[] AttackStateIds =
    {
        Animator.StringToHash("Base Layer.Attack1"),
        Animator.StringToHash("Base Layer.Attack2"),
        Animator.StringToHash("Base Layer.Attack3")
    };

    public void Spawn(Vector3 direction, int comboIndex, AttackStep attack)
    {
        // this is really dumb but i have to make the sprite stand upright (aligned on the XY plane) so that 
        // it will actually display in Unity's Project preview because they just assume all sprites are aligned to XY
        // which means i have to rotate it here to align it back to the XZ plane for our actual use case
        // i could ignore the issue but it was bugging me that the preview was empty and its not that hard a fix so...
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
        
        normalHitbox.gameObject.SetActive(false);
        finisherHitbox.gameObject.SetActive(false);
        
        bool isFinisher = comboIndex == 2;
        var activeHitbox = isFinisher ? finisherHitbox : normalHitbox;
        activeHitbox.Init(new HitData(attack.Damage, direction, attack.Knockback));
        activeHitbox.gameObject.SetActive(true);
        
        animator.Play(AttackStateIds[comboIndex], 0, 0f);

        Destroy(gameObject, attack.Duration);
    }
}
