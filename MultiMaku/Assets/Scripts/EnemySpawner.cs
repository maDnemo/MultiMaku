using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public int numberOfEnemiesOnSpawn;
    public int maxNumberOfEnemies;
    public double enemySpawnDelay;
    public double nextEnemySpawn;

    public override void OnStartServer()
    {
        nextEnemySpawn = Time.time;
        for (int i = 0; i < numberOfEnemiesOnSpawn; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range(-3.5f, 3.5f),
                Random.Range(0f, 5.0f),
                0.0f);

            var spawnRotation = Quaternion.Euler(
                0.0f,
                0.0f,
                0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }

    public void Update()
    {
        if (Time.time > nextEnemySpawn)
        {
            nextEnemySpawn = Time.time + enemySpawnDelay;
            var spawnPosition = new Vector3(
                Random.Range(-3.5f, 3.5f),
                Random.Range(0f, 5.0f),
                0.0f);

            var spawnRotation = Quaternion.Euler(
                0.0f,
                0.0f,
                0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }
}
