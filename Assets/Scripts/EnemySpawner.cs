﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] WaypointConfig waypointConfig;
    [Range(0, 10, order = 1)] [SerializeField] int numberOfEnemies = 4;

    private Player player;
    private bool spawningExecuted = false;

    void Start() {
        player = FindObjectOfType<Player>();
    }

    private void Update() {
        if (player.IsPlayerReady() && !spawningExecuted) {
            StartCoroutine(SpawnEnemies());
            spawningExecuted = true;
        }
    }

    IEnumerator SpawnEnemies() {

        for (int enemyCount = 0; enemyCount < numberOfEnemies; enemyCount++) {
            var enemy = Instantiate(waypointConfig.GetEnemyPrefab(), waypointConfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyOne>().SetWaypointConfig(waypointConfig);
            enemy.GetComponent<EnemyOne>().SetEnemyPosition(enemyCount);
            yield return new WaitForSeconds(waypointConfig.GetTimeBetweenSpawns());
        }

        yield return null;
    }


}
