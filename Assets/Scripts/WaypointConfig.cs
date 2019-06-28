using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Path Config")]
public class WaypointConfig : ScriptableObject {
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] int numberOfEnemy = 4;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float timeDelayAfterSpawn = 1f;

    public GameObject GetEnemyPrefab() {
        return enemyPrefab;
    }

    public List<Transform> GetWaypoints() {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform) {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }

    public float GetTimeBetweenSpawns() {
        return timeBetweenSpawns;
    }

    public int GetNumberOfEnemy() {
        return numberOfEnemy;
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public float GetTimeDelayAfterSpawn() {
        return timeDelayAfterSpawn;
    }
}
