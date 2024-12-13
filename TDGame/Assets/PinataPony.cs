using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinataPony : MonoBehaviour
{
    [Header("Spawning Attributes")]
    [SerializeField] private GameObject[] enemyPrefabs; 
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 1f; 
    [SerializeField] private int enemiesToSpawnOnDestroy = 5;
    [SerializeField] private int enemiesToSpawnOverTime = 3; 

    private int passiveSpawns = 0;
    private bool isSpawning = false;
    private bool isDestroyed = false;

    private Health health;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }

        health = GetComponent<Health>();
    }

    private void Update()
    {

        if(!isDestroyed && health.currentHealth > 0)
        {
            if(health.currentHealth <= 0)
            {
                onPinataDeath();
            }
        }
    }

    public void onDamageTaken()
    {
        if (!isSpawning && passiveSpawns < enemiesToSpawnOverTime)
        {
            StartCoroutine(passiveSpawn());
        }
    }

    private IEnumerator passiveSpawn()
    {
        isSpawning = true;

        while (passiveSpawns < enemiesToSpawnOverTime)
        {
            spawnEnemy();
            passiveSpawns++;
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    private void spawnEnemy()
    {
        if (enemyPrefabs.Length > 0)
        {
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
        }
    }

    private IEnumerator spawnFinalWave()
    {
        for(int i = 0; i < enemiesToSpawnOnDestroy; i++)
        {
            spawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void onPinataDeath()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            onDestroy();
            Destroy(gameObject);
        }
    }

    public void onDestroy()
    {
        StartCoroutine(spawnFinalWave());
    }
}