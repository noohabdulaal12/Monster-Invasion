using UnityEngine;
using TMPro;

public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public TextMeshProUGUI roundText;

    [Header("Round Settings")]
    public int currentRound = 1;
    public int zombiesToSpawn = 5;
    public int zombiesAlive = 0;
    public float spawnInterval = 2f;

    [Header("Difficulty Scaling")]
    public int healthIncreasePerRound = 20;
    public float speedIncreasePerRound = 0.2f;
    public int damageIncreasePerRound = 1;

    private int zombiesSpawnedThisRound = 0;
    private float nextSpawnTime;
    private bool roundInProgress = true;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (!roundInProgress) return;

        if (zombiesSpawnedThisRound < zombiesToSpawn && Time.time >= nextSpawnTime)
        {
            SpawnZombie();
            nextSpawnTime = Time.time + spawnInterval;
        }

        if (zombiesSpawnedThisRound >= zombiesToSpawn && zombiesAlive <= 0)
        {
            NextRound();
        }
    }

    void StartRound()
    {
        zombiesSpawnedThisRound = 0;
        zombiesAlive = 0;
        roundInProgress = true;
        UpdateRoundUI();
    }

    void SpawnZombie()
    {
        if (zombiePrefab == null || spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        GameObject zombieObj = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

        Zombie zombie = zombieObj.GetComponent<Zombie>();
        if (zombie != null)
        {
            int health = 100 + ((currentRound - 1) * healthIncreasePerRound);
            float speed = 2.5f + ((currentRound - 1) * speedIncreasePerRound);
            int damage = 2 + ((currentRound - 1) * damageIncreasePerRound);

            zombie.Setup(health, speed, damage);
        }

        zombiesSpawnedThisRound++;
        zombiesAlive++;
    }

    public void ZombieKilled()
    {
        zombiesAlive--;
    }

    void NextRound()
    {
        currentRound++;
        zombiesToSpawn += 2;
        StartRound();
    }

    void UpdateRoundUI()
    {
        if (roundText != null)
            roundText.text = "Round: " + currentRound;
    }
}