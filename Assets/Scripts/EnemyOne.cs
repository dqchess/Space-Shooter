using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : MonoBehaviour {

    // Configurations
    [Header("Firing Configurations")]
    [Range(0f, 5f)] [SerializeField] float delayBetweenFiring = 0.75f;
    [SerializeField] GameObject missilePrefab;
    [Range(1, 200f)] [SerializeField] float projectileSpeed = 100f;

    [Header("Movement Configurations")]
    [SerializeField] float enemyMovementSpeed = 20f;

    // Cached references
    private float currentFiringTime = 0f;
    private int waypointIndex = 0;
    private List<Transform> waypoints;
    private WaypointConfig waypointConfig;
    private int enemyPosition = 0;
    private float distanceBetweenEnemies = 15f;
    private bool canShoot = false;

    private void Start() {
        waypoints = waypointConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;

        transform.rotation = Quaternion.Euler(0f, 90f, 0f);

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

        if (canShoot) {
            StartShooting();
        }
    }

  
    private void StartShooting() {
        if (currentFiringTime >= delayBetweenFiring) {
            // Instantiate bullet
            currentFiringTime = 0;
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            missile.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, missile.transform.forward.z * -1) * projectileSpeed;
        }
        currentFiringTime += Time.deltaTime;
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
            float rotationSpeed = 3f;
            // Rotaton part
            if (waypointIndex == 3) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 150f, -30f), Time.deltaTime * rotationSpeed);
            }

            if (waypointIndex > 3) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 180f, 0f), Time.deltaTime * rotationSpeed);
            }
        } else {
            canShoot = true;
        }
    }
}
