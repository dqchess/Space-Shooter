using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    [SerializeField] float crosshairMovementSpeed = 500f;

    [Header("Joystick Configurations")]
    [SerializeField] Joystick joystick;
    [Range(0f, 1f)] [SerializeField] float horizontalJoystickThreshold = 0.2f;
    [Range(0f, 1f)] [SerializeField] float verticalJoystickThreshold = 0.2f;

    private float x;
    private float y;

    // Update is called once per frame
    void Update() {
        MoveCrosshair();
    }

    private void MoveCrosshair() {

        Vector3 currentPosition = transform.position;


        if (joystick.Horizontal >= horizontalJoystickThreshold) {
            x = joystick.Horizontal * crosshairMovementSpeed * Time.deltaTime;
        } else if (joystick.Horizontal <= horizontalJoystickThreshold) {
            x = joystick.Horizontal * crosshairMovementSpeed * Time.deltaTime;
        } else {
            x = 0;
        }

        if (joystick.Vertical >= verticalJoystickThreshold) {
            y = joystick.Vertical * crosshairMovementSpeed * Time.deltaTime;
        } else if (joystick.Vertical <= verticalJoystickThreshold) {
            y = joystick.Vertical * crosshairMovementSpeed * Time.deltaTime;
        } else {
            y = 0;
        }
        /*
       // Restrict crosshair movement to viewport
        if (currentPosition.x + x >= 40) {
            x = 0;
        }

        if (currentPosition.x + x <= -40) {
            x = 0;
        }

        if (currentPosition.y + y <= -9) {
            y = 0;
        }

        if (currentPosition.y + y >= 29) {
            y = 0;
        }
        */
        transform.Translate(x, y, 0, Space.World);
    }

}
