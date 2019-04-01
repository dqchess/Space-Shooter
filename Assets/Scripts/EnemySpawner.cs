using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] WaypointConfig waypointConfig;
    [Range(0, 10, order = 1)] [SerializeField] int numberOfEnemies = 4;

    void Start() {

           StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies() {

        for (int enemyCount = 0; enemyCount < numberOfEnemies; enemyCount++) {
            var enemy = Instantiate(waypointConfig.GetEnemyPrefab(), waypointConfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetWaypointConfig(waypointConfig);
            enemy.GetComponent<Enemy>().SetEnemyPosition(enemyCount);
            yield return new WaitForSeconds(waypointConfig.GetTimeBetweenSpawns());
        }

        yield return null;
    }


}
