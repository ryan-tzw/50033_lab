using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private float _lifetime = 0.2f;
    private float _finisherLifetime = 0.25f;

    public void Spawn(Vector3 direction, int comboIndex)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        Destroy(gameObject, comboIndex == 2 ? _finisherLifetime : _lifetime);
    }
}
