public interface IHittable
{
    // Returns true if the hit landed (false if dead, spawning, invincible, etc.)
    bool ReceiveHit(HitData hit);

    // Used by the HitConfirmed event (hitstop / effects)
    LocalTime LocalTime { get; }
}