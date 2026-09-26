using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private float _lifetime = 0.2f;
    private float _finisherLifetime = 0.25f;

    public void Spawn(Vector3 direction, int comboIndex)
    {
        // this is really dumb but i have to make the sprite stand upright (aligned on the XY plane) so that 
        // it will actually display in Unity's Project preview because they just assume all sprites are aligned to XY
        // which means i have to rotate it here to align it back to the XZ plane for our actual use case
        // i could ignore the issue but it was bugging me that the preview was empty and its not that hard a fix so...
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
        Destroy(gameObject, comboIndex == 2 ? _finisherLifetime : _lifetime);
    }
}
