using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyList;
    public List<Transform> spawnPointsList;
    private float _gameTimer;
    private float _spawnInterval;
    private float _timeSinceLastSpawn;

    private void Start()
    {
        _gameTimer = 0f;
        _spawnInterval = 7.5f;
        _timeSinceLastSpawn = 0f;
    }

    private void Update()
    {
        _gameTimer += Time.deltaTime;
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= _spawnInterval)
        {
            if (_gameTimer >= 120f) return;

            SpawnEnemy();
            _timeSinceLastSpawn = 0f;
            _spawnInterval -= (_spawnInterval * 3) / 100;

            // Debug.Log(_spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyToSpawn = enemyList[Random.Range(0, enemyList.Count)];
        Transform spawnPoint = spawnPointsList[Random.Range(0, spawnPointsList.Count)];
        Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
    }

    public void ScreamSpawnEnemies(int count)
    {
        while (count > 0)
        {
            GameObject enemyToSpawn = enemyList[Random.Range(0, enemyList.Count)];
            Transform spawnPoint = spawnPointsList[Random.Range(0, spawnPointsList.Count)];
            Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
            count--;
        }
    }
}