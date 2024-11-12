using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Spawner : MonoBehaviour
{
    public static Spawner main;  // Singleton instance

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private TextMeshProUGUI waveCounterText;  // Reference to the wave counter text

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScaling = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private int maxWaves = 20;

    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; //enemies per second
    private bool isSpawning = false;
    private bool isWaveActive = false;

    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    public void StartWaves()
    {
        if (!isWaveActive)
        {
            StartCoroutine(StartWave());
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        if(currentWave > maxWaves)
        {
            EndGame();
            yield break;
        }

        isSpawning = true;
        isWaveActive = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
        UpdateWaveCounter();  // Update wave counter at the start of each wave
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        isWaveActive = false;
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && (enemiesLeftToSpawn > 0))
        {
            spawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private int EnemiesPerWave()
    {
        if (currentWave % 5 == 0)
        {
            Debug.Log("SUGAR RUSH!");
            return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScaling) * 1.5f);
        }

        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScaling));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(eps * Mathf.Pow(currentWave, difficultyScaling), 0f, enemiesPerSecondCap);

    }

    private void spawnEnemy()
    {
        //int index = Random.Range(0, enemyPrefabs.Length);
        //GameObject prefabToSpawn = enemyPrefabs[index];
        GameObject prefabToSpawn;
        if(currentWave < 5)
        {
            prefabToSpawn = enemyPrefabs[0];
        }
        else
        {
            int index = Random.Range(0, enemyPrefabs.Length);
            prefabToSpawn = enemyPrefabs[index];
        }

        Instantiate(prefabToSpawn, Level1.main.startPoint.position, Quaternion.identity);
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void UpdateWaveCounter()
    {
        if (waveCounterText != null)
        {
            waveCounterText.text = "Wave: " + currentWave;
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over! You defeated the sweet treat army!");
    }
}
