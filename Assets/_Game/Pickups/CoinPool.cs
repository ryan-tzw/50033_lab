using UnityEngine;
using UnityEngine.Pool;

public class CoinPool : MonoBehaviour
{
    [SerializeField] private Coin coinPrefab;

    [SerializeField, Min(0f)] private float spawnHeight = 0.25f;
    [SerializeField, Min(0f)] private float horizontalImpulse = 2f;
    [SerializeField, Min(0f)] private float upwardImpulse = 2.5f;

    [SerializeField, Min(1)] private int initialCapacity = 10;
    [SerializeField, Min(1)] private int maxPoolSize = 500;

    private ObjectPool<Coin> _pool;

    public event System.Action CoinCollected;

    private void Awake()
    {
        _pool = new ObjectPool<Coin>(
            createFunc: () =>
            {
                var coin = Instantiate(coinPrefab, transform);
                coin.SetReleaseCallback(ReleaseCoin);
                coin.SetCollectedCallback(HandleCoinCollected);
                coin.gameObject.SetActive(false);
                return coin; 
            },
            actionOnGet: null,
            actionOnRelease: coin => coin.gameObject.SetActive(false),
            actionOnDestroy: coin => Destroy(coin.gameObject),
            collectionCheck: true,
            defaultCapacity: initialCapacity,
            maxSize: maxPoolSize
        );
    }

    public void DropCoin(Enemy enemy)
    {
        var angle = Random.Range(0f, Mathf.PI * 2f);
        var horizontalDirection = new Vector3( Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        var impulse = horizontalDirection * horizontalImpulse + Vector3.up * upwardImpulse;
        var spawnPosition = enemy.transform.position + Vector3.up * spawnHeight;
        
        var coin = _pool.Get();
        coin.Spawn(spawnPosition, impulse);
    }

    private void HandleCoinCollected()
    {
        CoinCollected?.Invoke();
    }

    private void ReleaseCoin(Coin coin)
    {
        _pool.Release(coin);
    }
}