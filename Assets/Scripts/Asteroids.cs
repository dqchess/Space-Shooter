using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour {

    [Header("Asteroids Prefabs")]
    [SerializeField] GameObject[] asteroids;

    [Header("Asteroids Spawning Coordinate Values")]
    [SerializeField] float xMin = -38.5f;
    [SerializeField] float xMax = 38.5f;
    [SerializeField] float yMin = -16f;
    [SerializeField] float yMax = 25f;
    [SerializeField] float zMin = -50f;
    [SerializeField] float zMax = 250f;

    [SerializeField] float asteroidSpeed = 20;
    [SerializeField] float timeBetweenSpawn = 3.5f;
    private float currentSpawnTime = 0f;

    // Update is called once per frame
    void Update() {
        if (currentSpawnTime >= timeBetweenSpawn) {
            currentSpawnTime = 0;
            SpawnAsteroids();
        }
        currentSpawnTime += Time.deltaTime;
    }

    private void SpawnAsteroids() {
        int asteroidIndex = Random.Range(0, asteroids.Length);
        Vector3 position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), zMax);
        Debug.Log("X: " + position.x + " Y: " + position.y + " Z: " + position.z);
        Instantiate(asteroids[asteroidIndex], position, Quaternion.identity);
    }
}
