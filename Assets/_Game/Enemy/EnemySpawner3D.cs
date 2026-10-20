using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;  

public class EnemySpawner3D : MonoBehaviour
{
    [SerializeField] private EnemyDropSpawner dropSpawner;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Camera worldCamera;

    [SerializeField, Min(0f)] private float minSpawnInterval = 0.8f;
    [SerializeField, Min(0f)] private float maxSpawnInterval = 1.4f;
    [SerializeField, Min(0f)] private float spawnMargin = 1f;

    [SerializeField, Min(1)] private int initialCap = 10;
    [SerializeField, Min(1)] private int maxPoolSize = 100;

    [SerializeField] private GameEvent enemyKilledEvent;

    private readonly List<Enemy> _activeEnemies = new();
    private float _nextSpawnTime;
    private Plane _groundPlane;
    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        worldCamera ??= Camera.main;
        _groundPlane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));

        _pool = new ObjectPool<Enemy>(
            createFunc: () =>
            {
                var enemy = Instantiate(enemyPrefab, transform);
                enemy.SetReleaseCallback(ReleaseEnemy);
                if (dropSpawner is not null)
                {
                    enemy.Killed += dropSpawner.DropPickup;
                }
                enemy.Killed += OnEnemyKilled;
                enemy.gameObject.SetActive(false);
                return enemy;
            },
            actionOnGet: enemy =>
            {
                _activeEnemies.Add(enemy);
                enemy.Spawn(GetSpawnPosition());
            },
            actionOnRelease: enemy =>
            {
                _activeEnemies.Remove(enemy);
                enemy.gameObject.SetActive(false);
            },
            actionOnDestroy: enemy =>
            {
                if (dropSpawner is not null)
                {
                    enemy.Killed -= dropSpawner.DropPickup;
                }
                enemy.Killed -= OnEnemyKilled;
                Destroy(enemy.gameObject);
            },
            collectionCheck: true,
            defaultCapacity: initialCap,
            maxSize: maxPoolSize
        );
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
    }

    private void OnEnable()
    {
        _nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void OnEnemyKilled(Enemy enemy)   
    {
        enemyKilledEvent.Raise();   
    }

    private void Update()
    {
        if (Time.time < _nextSpawnTime) return;

        _pool.Get();
        _nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private Vector3 GetSpawnPosition()
    {
        const float horizontalViewportPadding = 0.1f;
        var viewportX = Random.Range( horizontalViewportPadding, 1f - horizontalViewportPadding);
        var spawnNorth = Random.value < 0.5f;
        var viewportY = spawnNorth ? 1f : 0f;
        
        var edgePosition = ViewportToGround(viewportX, viewportY);
        var screenCentre = ViewportToGround(0.5f, 0.5f);
        var edgeCentre   = ViewportToGround(0.5f, viewportY);

        var outwardDirection = (edgeCentre - screenCentre).normalized;

        return edgePosition + outwardDirection * spawnMargin;
    }

    private Vector3 ViewportToGround(float viewportX, float viewportY)
    {
        var ray = worldCamera.ViewportPointToRay( new Vector3(viewportX, viewportY));
        _groundPlane.Raycast(ray, out var distance);
        return ray.GetPoint(distance);
    }

    public void StopAndClear()
    {
        enabled = false;          
        StopAllCoroutines();

        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = _activeEnemies[i];
            if (enemy == null)                    
            {
                _activeEnemies.RemoveAt(i);
                continue;
            }
            _pool.Release(enemy);
        }
    }
}
