using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Aiming : MonoBehaviour {
    [SerializeField] Image crosshair;

    private float x;
    private float y;
    private Plane plane;

    // Start is called before the first frame update
    void Start() {
        plane = new Plane(Vector3.back, 45f);
    }

    // Update is called once per frame
    void LateUpdate() {
        Rotate();
    }

    private void Rotate() {
        Ray cameraRay = Camera.main.ScreenPointToRay(crosshair.transform.position);
        float distance;
        plane.Raycast(cameraRay, out distance);
        Vector3 point = cameraRay.GetPoint(distance);
        Vector3 firingDirection = point - transform.position;

        transform.rotation = Quaternion.LookRotation(firingDirection.normalized);

    }

}
