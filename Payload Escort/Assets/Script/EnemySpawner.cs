using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform _payload;
    [SerializeField] GameObject[] _enemies;
    [SerializeField] Transform[] _spawnPoints;

    private int _enemyCount = 0;
    public int startSpawnTime = 10;
    public int spawnTime = 5;


    void Start()
    {
        InvokeRepeating("Spawn", startSpawnTime, spawnTime);
    }

    void Spawn()
    {
        int spawnPoints = Random.Range(0, _spawnPoints.Length);
        int randomEnemy = Random.Range(0, _enemies.Length);
        float randomPos = Random.Range(-2, 2);
        
        if (_enemyCount == 5) return;

        Instantiate(_enemies[randomEnemy], _spawnPoints[spawnPoints].position, Quaternion.identity);
        _enemyCount++;
    }
}
