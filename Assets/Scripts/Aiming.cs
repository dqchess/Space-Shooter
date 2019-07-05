using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiming : MonoBehaviour {
    [SerializeField] Image crosshair;

    private float x;
    private float y;
    private Plane plane;
    private PlayerBot playerBot;
    private string parentObjectName;

    // Start is called before the first frame update
    void Start() {
        plane = new Plane(Vector3.back, 80f);

        if (crosshair == null) {
            crosshair = FindObjectOfType<Crosshair>().GetComponent<Image>();
        }

        parentObjectName = transform.parent.gameObject.name;
        playerBot = transform.parent.GetComponent<PlayerBot>();
    }

    // Update is called once per frame
    void LateUpdate() {
        Rotate();

        Debug.DrawRay(transform.position, transform.forward * 250, Color.green);
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
