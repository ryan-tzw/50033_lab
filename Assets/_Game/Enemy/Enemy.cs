using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    private EnemyHealth _health;
    private System.Action<Enemy> _despawnCallback;
    public bool IsDying => _health.IsDying;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
        _health.SetDeathCompletedCallback(Despawn);
    }

    public void Spawn(Vector3 position)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
        gameObject.SetActive(true);
    }

    public void SetDespawnCallback(System.Action<Enemy> despawnCallback)
    {
        _despawnCallback = despawnCallback;
    }

    public void ReceiveHit(HitData hit)
    {
        _health.ReceiveHit(hit);
    }

    private void Despawn()
    {
        if (_despawnCallback is null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _despawnCallback.Invoke(this);
        }
    }
}
