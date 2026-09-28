using UnityEngine;

public class EnemyDropSpawner : MonoBehaviour
{
    [SerializeField] private CoinPool coins;
    [SerializeField] private MagnetPool magnets;
    [SerializeField, Range(0f, 1f)] private float magnetChance = 0.01f;

    public void DropPickup(Enemy enemy)
    {
        if (Random.value < magnetChance)
        {
            magnets.DropMagnet(enemy);
        }
        else
        {
            coins.DropCoin(enemy);
        }
    }

}