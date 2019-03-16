using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    // Camera configurations
    [SerializeField] Transform[] cameraWaypoints;
    [Range(0f, 30f)] [SerializeField] float cameraMovementSpeed = 16f;
    private int pointIndex = 0;

    private Vector3 cameraFinalPosition;
    private Quaternion cameraRotation;
    private Player player;

    // Start is called before the first frame update
    void Start() {
        cameraFinalPosition = new Vector3(0f, 18.1f, -35f);
        cameraRotation = Quaternion.Euler(10f, 0f, 0f);

        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update() {
        if (!player.isCombatStarted) {
            transform.LookAt(player.transform);

            if (Vector3.Distance(cameraWaypoints[pointIndex].transform.position, transform.position) < 0.2f) {
                if (pointIndex < cameraWaypoints.Length - 1) {
                    pointIndex++;
                }
            }

        } else {
            transform.position = Vector3.Lerp(transform.position, cameraFinalPosition, Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, cameraRotation, Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, cameraWaypoints[pointIndex].transform.position, Time.deltaTime * cameraMovementSpeed);

    }
}
