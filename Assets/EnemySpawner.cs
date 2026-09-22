using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Camera spawnCamera;  // spawn enemies outside the field of view of the camera
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnMargin = 1f;
    
    private float _nextSpawnTime;

    private void Awake()
    {
        spawnCamera ??= Camera.main;
        _nextSpawnTime = Time.time + spawnInterval;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time < _nextSpawnTime) return;

        Vector2 spawnPosition = GetSpawnPosition();
        Player closestPlayer = null;
        float closestDistSqr = Mathf.Infinity;

        foreach (PlayerInput playerInput in PlayerInput.all)
        {
            // NOTE:
            // i think generally we shouldn't call GetComponent in an update loop like this but here it's probably fine
            // in the final game players wouldn't be able to join in the middle of the game,
            // so instead we'd save references to each player
            // this would apply for example if the boss has an attack that targets players individually
            var player = playerInput.GetComponent<Player>();
            if (player is null) continue;
            
            float distSqr = ((Vector2)player.transform.position - spawnPosition).sqrMagnitude;
            if (distSqr < closestDistSqr)
            {
                closestPlayer = player;
                closestDistSqr = distSqr;
            }
        }

        if (closestPlayer is null) return;
        
        // todo: probably should use an object pool instead
        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.SetTarget(closestPlayer);
        _nextSpawnTime = Time.time + spawnInterval;
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 cameraCenter = spawnCamera.transform.position;
        float halfHeight = spawnCamera.orthographicSize;
        float halfWidth = spawnCamera.aspect * halfHeight;

        float innerRad = Mathf.Sqrt(halfWidth * halfWidth + halfHeight * halfHeight);
        float outerRad = innerRad + spawnMargin;
        float radius = Random.Range(innerRad, outerRad);
        
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        
        return cameraCenter + direction * radius;
    }
}
