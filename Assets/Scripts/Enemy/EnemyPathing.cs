using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {

    private WaypointConfig waveConfig;
    private List<Transform> waypoints;
    private int waypointIndex = 0;
    private Vector3 movementDirection;
    private float speed;

    // Use this for initialization
    void Start() {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update() {
        Move();
    }

    public void SetWaveConfig(WaypointConfig waveConfig) {
        this.waveConfig = waveConfig;
    }

    private void Move() {
        if (waypointIndex <= waypoints.Count - 1) {
            var targetPosition = waypoints[waypointIndex].transform.position;
            speed = waveConfig.GetMoveSpeed();
            var movementThisFrame = speed * Time.deltaTime;

            if (waypointIndex > 0) {
                movementDirection = waypoints[waypointIndex].position - waypoints[waypointIndex - 1].position;
            }

            transform.position = Vector3.MoveTowards
                (transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition) {
                waypointIndex++;
            }
        } else {
            Destroy(gameObject);
        }
    }

    public Vector3 GetMovementDirection() {
        return movementDirection;
    }

    public float GetSpeed() {
        return speed;
    }

}