using Lab50033;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private IntVariable killCount;
    [SerializeField] private int killCountLimit = 20;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private EnemySpawner3D enemySpawner;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private float musicFadeDuration = 1.5f;
    private bool _bossSpawned;
    private Boss _spawnedBoss;

    private void Update()
    {
        if (!_bossSpawned && killCount.Value >= killCountLimit)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        _bossSpawned = true;
        enemySpawner.StopAndClear();
        _spawnedBoss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.Play(bossMusic, musicFadeDuration);
        }
    }
}
