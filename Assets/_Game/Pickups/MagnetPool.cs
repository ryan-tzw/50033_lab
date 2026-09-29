using UnityEngine;
using UnityEngine.Pool;

public class MagnetPool : MonoBehaviour
{
    [SerializeField] private Magnet magnetPrefab;
    [SerializeField] private CoinPool coinPool;

    [SerializeField, Min(0f)] private float spawnHeight = 0.25f;
    [SerializeField, Min(0f)] private float horizontalImpulse = 2f;
    [SerializeField, Min(0f)] private float upwardImpulse = 2.5f;

    [SerializeField, Min(1)] private int initialCapacity = 2;
    [SerializeField, Min(1)] private int maxPoolSize = 20;
    private ObjectPool<Magnet> _pool;
    
    public event System.Action MagnetCollected;

    private void Awake()
    {
        _pool = new ObjectPool<Magnet>(
            createFunc: () =>
            {
                var magnet = Instantiate(magnetPrefab, transform);
                magnet.SetReleaseCallback(ReleaseMagnet);
                magnet.SetCollectedCallback(HandleMagnetCollected);
                magnet.gameObject.SetActive(false);
                return magnet;
            },
            actionOnGet: null,
            actionOnRelease: magnet =>  magnet.gameObject.SetActive(false),
            actionOnDestroy: magnet => Destroy(magnet.gameObject),
            collectionCheck: true,
            defaultCapacity: initialCapacity,
            maxSize: maxPoolSize
        );
    }

    public void DropMagnet(Enemy enemy)
    {
        var angle = Random.Range(0f, Mathf.PI * 2f);
        var dirXZ = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        var impulse = dirXZ * horizontalImpulse + Vector3.up * upwardImpulse;
        var spawnPosition = enemy.transform.position + Vector3.up * spawnHeight;

        var magnet = _pool.Get();
        magnet.Spawn(spawnPosition, impulse, coinPool);
    }

    private void HandleMagnetCollected()
    {
        MagnetCollected?.Invoke();
    }

    private void ReleaseMagnet(Magnet magnet)
    {
        _pool.Release(magnet);
    }
}