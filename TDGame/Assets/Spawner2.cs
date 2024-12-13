
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class Spawner2 : MonoBehaviour
{
    public static Spawner2 main;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private TextMeshProUGUI waveCounterText;

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

        if (currentWave > maxWaves)
        {
            EndGame();
            yield break;
        }

        isSpawning = true;
        isWaveActive = true;

        if (currentWave == 10 || currentWave == 20)
        {
            enemiesLeftToSpawn = 1; //ensures that only one boss spawns during these waves
        }
        else
        {
            enemiesLeftToSpawn = EnemiesPerWave();
            eps = EnemiesPerSecond();
        }

        UpdateWaveCounter();  //updates wave counter at the start of each wave
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
            EndWave(); //ends wave, to start next wave 
        }
    }

    //function to handle enemies per wave
    private int EnemiesPerWave()
    {
        if (currentWave % 5 == 0) //every 5 waves, a large wave of various enemies will spawn in 
        {
            Debug.Log("SUGAR RUSH!");
            return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScaling) * 1.5f);
        }

        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScaling)); //each wave has slightly more enemies than the last to increase difficulty
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(eps * Mathf.Pow(currentWave, difficultyScaling), 0f, enemiesPerSecondCap);
    }

    //function to spawn enemies
    private void spawnEnemy()
    {
        GameObject prefabToSpawn;

        if (currentWave == 10 || currentWave == 20 || currentWave == 30) //spawns boss at wave 10 and wave 20
        {
            prefabToSpawn = bossPrefabs[0];

        }
        else
        {
            if (currentWave < 5) //for first 5 waves, spawns only one type of enemy
            {
                prefabToSpawn = enemyPrefabs[0]; //index for devilish donut
            }
            else if (currentWave >= 5 && currentWave < 15)
            {
                int index = Random.Range(0, enemyPrefabs.Length - 6); //Adds Chocolate Charger to Spawner
                prefabToSpawn = enemyPrefabs[index];
            }
            else if (currentWave >= 20 && currentWave < 15)
            {
                int index = Random.Range(0, enemyPrefabs.Length - 3); //Adds Juking JellyBean
                prefabToSpawn = enemyPrefabs[index];
            }
            else
            {
                int index = Random.Range(0, enemyPrefabs.Length); //Adds SlowliPop to Spawner
                prefabToSpawn = enemyPrefabs[index];
            }
        }


        Instantiate(prefabToSpawn, Level1.main.startPoint.position, Quaternion.identity); //instantiates the enemy and spawns it at the start position to follow the path
    }

    private GameObject findEnemy(string name)
    {
        foreach (GameObject prefab in enemyPrefabs)
        {
            if (prefab.name == name)
            {
                return prefab;
            }
        }

        Debug.LogError($"Enemy ({name}) not found");
        return null;
    }


    private void EnemyDestroyed() //handles event where enemy is destroyed by decrementing enemies alive for wave behavior
    {
        enemiesAlive--;
    }

    private void UpdateWaveCounter()
    {
        if (waveCounterText != null)
        {
            waveCounterText.text = "Wave: " + currentWave; //updates wave counter every wave by adding to a text box
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over! You defeated the sweet treat army!");
    }
}
