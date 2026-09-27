using UnityEngine;

public readonly struct HitData
{
    public int Damage { get; }
    public Vector3 Direction { get; }
    public float Knockback { get; }

    public HitData(int damage, Vector3 direction, float knockback)
    {
        Damage = damage;
        Direction = direction;
        Knockback = knockback;
    }
}
