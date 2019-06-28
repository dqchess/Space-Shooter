using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aiming : MonoBehaviour {

    [Header("Joystick Configurations")]
    [SerializeField] Joystick joystick;
    [Range(0f, 1f)] [SerializeField] float horizontalJoystickThreshold = 0.2f;
    [Range(0f, 1f)] [SerializeField] float verticalJoystickThreshold = 0.2f;

    [Header("Rotation Settings")]
    [SerializeField] float rotationSpeed = 10;
    [SerializeField] float xLimitMin = -45f;
    [SerializeField] float xLimitMax = 45f;
    [SerializeField] float yLimitMin = -60f;
    [SerializeField] float yLimitMax = 60f;

    [SerializeField] LayerMask projectileLayerMask;
    [SerializeField] Image crosshair;
    private float x;
    private float y;
    private LineRenderer aimAssistLine;
    private Vector3 previousPosition;
    // Start is called before the first frame update
    void Start() {
        aimAssistLine = GetComponent<LineRenderer>();
        previousPosition = new Vector3(0f, 0f, 100f);
        Debug.Log("Aiming.cs");
    }

    // Update is called once per frame
    void Update() {
        Rotate();
    }

    private void Rotate() {
        Debug.Log("Rotating Gun");
        //if (joystick.Horizontal >= horizontalJoystickThreshold) {
        //    x = joystick.Horizontal * rotationSpeed * Time.deltaTime;
        //} else if (joystick.Horizontal <= horizontalJoystickThreshold) {
        //    x = joystick.Horizontal * rotationSpeed * Time.deltaTime;
        //} else {
        //    x = 0;
        //}

        //if (joystick.Vertical >= verticalJoystickThreshold) {
        //    y = joystick.Vertical * rotationSpeed * Time.deltaTime;
        //} else if (joystick.Vertical <= verticalJoystickThreshold) {
        //    y = joystick.Vertical * rotationSpeed * Time.deltaTime;
        //} else {
        //    y = 0;
        //}

        //crosshair.transform.position = Camera.main.WorldToScreenPoint(transform.forward);

        Vector3 end = GetWorldPositionOnPlane(crosshair.transform.position, 75f);
        //transform.rotation = Quaternion.LookRotation();
        Ray rayOrigin = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, out hitInfo)) {
            if (hitInfo.collider.gameObject.CompareTag("Plane")) {
                end = hitInfo.point;
            }
        }
        //Vector3 crosshairPosition = GetWorldPositionOnPlane(crosshair.transform.position, 55f);
        Vector3 gunPosition = transform.position;

        Vector3 firingDirection = end - gunPosition;

        transform.rotation = Quaternion.LookRotation(firingDirection);

        //transform.Rotate(-y, x, 0f);
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

}
