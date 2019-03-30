using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour {

    [Header("Asteroids Prefabs")]
    [SerializeField] GameObject[] asteroids;

    [Header("Asteroids Spawning Coordinate Values")]
    [SerializeField] float x1Min = -45f;
    [SerializeField] float x1Max = -30f;
    [SerializeField] float x2Min = 29f;
    [SerializeField] float x2Max = 45f;

    [SerializeField] float yMin = -16f;
    [SerializeField] float yMax = 25f;
    [SerializeField] float zMin = -50f;
    [SerializeField] float zMax = 250f;

    [SerializeField] float asteroidSpeed = 20;
    [SerializeField] float timeBetweenSpawn = 3.5f;
    private float currentSpawnTime = 0f;

    // Cached References
    private float x;

    void Update() {
        if (currentSpawnTime >= timeBetweenSpawn) {
            currentSpawnTime = 0;
            SpawnAsteroids();
        }
        currentSpawnTime += Time.deltaTime;
    }

    private void SpawnAsteroids() {
        int asteroidIndex = Random.Range(0, asteroids.Length);
        
        if (Random.Range(0, 100) % 2 == 0) {
            x = Random.Range(x1Min, x1Max);
        } else {
            x = Random.Range(x2Min, x2Max);
        }

        Vector3 position = new Vector3(x, Random.Range(yMin, yMax), zMax);
        //Debug.Log("X: " + position.x + " Y: " + position.y + " Z: " + position.z);
        Instantiate(asteroids[asteroidIndex], position, Quaternion.identity);
    }
}
