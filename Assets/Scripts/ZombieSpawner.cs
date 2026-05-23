using System.Collections;
using UnityEngine;
using TMPro;

public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public TextMeshProUGUI roundText;
    public BreakManager breakManager;

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
    private bool endingRound = false;

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

        if (!endingRound && zombiesSpawnedThisRound >= zombiesToSpawn && zombiesAlive <= 0)
        {
            Debug.Log("ROUND ENDED");

            StartCoroutine(EndRound());
        }
    }

    void StartRound()
    {
        zombiesSpawnedThisRound = 0;
        zombiesAlive = 0;
        roundInProgress = true;
        endingRound = false;
        nextSpawnTime = Time.time + spawnInterval;
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

        if (zombiesAlive < 0)
            zombiesAlive = 0;
    }

    IEnumerator EndRound()
    {
        endingRound = true;
        roundInProgress = false;

        if (breakManager != null)
        {
            yield return StartCoroutine(breakManager.StartBreak());
        }

        NextRound();
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