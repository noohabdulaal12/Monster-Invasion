using UnityEngine;
using System.Collections;

public class ZombieRandomSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;

    public float minSpawnTime = 2f;
    public float maxSpawnTime = 4f;
    public int maxZombies = 12;

    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

            if (zombies.Length >= maxZombies)
                continue;

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = 0f;

            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            zombie.SetActive(true);
        }
    }
}