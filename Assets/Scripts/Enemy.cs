using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    // Configurations
    [Header("Firing Configurations")]
    [Range(0f, 5f)] [SerializeField] float delayBetweenFiring = 0.75f;
    [SerializeField] GameObject missilePrefab;
    [Range(1000f, 5000f)] [SerializeField] float projectileSpeed = 3000f;

    [Header("Movement Configurations")]
    [SerializeField] float enemyMovementSpeed = 20f;

    // Cached references
    private float currentFiringTime = 0f;
    private int waypointIndex = 0;
    private List<Transform> waypoints;
    private WaypointConfig waypointConfig;
    private int enemyPosition = 0;
    private float distanceBetweenEnemies = 15f;

    private void Start() {
        waypoints = waypointConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    public void SetEnemyPosition(int position) {
        enemyPosition = position;
    }

    public void SetWaypointConfig(WaypointConfig waypointConfig) {
        this.waypointConfig = waypointConfig;
    }

    // Update is called once per frame
    void Update() {
        MoveEnemy();
    }

    public void MoveEnemy() {
        if (waypointIndex <= waypoints.Count - 1) {

            var movementThisFrame = waypointConfig.GetMoveSpeed() * Time.deltaTime;

            // Target positions where the enemy has to move
            float x = waypoints[waypointIndex].transform.position.x + (enemyPosition * distanceBetweenEnemies); // Added offset value to maintain distance between them,
            float y = waypoints[waypointIndex].transform.position.y;
            float z = waypoints[waypointIndex].transform.position.z;

            var targetPosition = new Vector3(x, y, z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (Vector3.Distance(transform.position, targetPosition) <= 0.05f) {
                waypointIndex++;
            }
        }
    }
}
