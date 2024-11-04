using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScaling = 0.75f;
    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    protected int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;


    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(StartWave());
    }

    // Update is called once per frame

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
    void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && (enemiesLeftToSpawn > 0)) //still spawning enemies
        {
            spawnEnemy();

            //makes sure this doesnt spawn infinite enemies
            enemiesLeftToSpawn--;
            enemiesAlive++;

            timeSinceLastSpawn = 0f;
        }
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            //ends wave
            EndWave();
        }
    }
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScaling));
    }
    private void spawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[0];
        Instantiate(prefabToSpawn, Level1.main.startPoint.position, Quaternion.identity);
    }

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }
}
