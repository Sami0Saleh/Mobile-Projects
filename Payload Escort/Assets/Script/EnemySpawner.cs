using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform _payload;
    [SerializeField] GameObject[] _enemies;
    private int _enemyCount = 0;
    void Update()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        float distance = Vector3.Distance(transform.position, _payload.position);
        while (distance == 5)
        {
            int randomEnemy = Random.Range(0, _enemies.Length);
            float randomPos = Random.Range(-2, 2);
            Instantiate(_enemies[randomEnemy], new Vector3(transform.position.x + randomPos, 
                transform.position.y,transform.position.z + randomPos), Quaternion.identity);
            _enemyCount++;
            distance = Vector3.Distance(transform.position, _payload.position);
            yield return new WaitForSecondsRealtime(10);
        }

    }
}
