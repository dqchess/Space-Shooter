using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    [SerializeField] float crosshairMovementSpeed = 800f;

    [Header("Joystick Configurations")]
    [SerializeField] Joystick joystick;
    [Range(0f, 1f)] [SerializeField] float horizontalJoystickThreshold = 0.2f;
    [Range(0f, 1f)] [SerializeField] float verticalJoystickThreshold = 0.2f;

    private float x;
    private float y;
    private float sensitivity;
    private float multiplier;
    private void Start() {
        sensitivity = PlayerPrefs.GetFloat("AimingSensitivity", 0.30f);

        multiplier = Mathf.Clamp(sensitivity, 0.1f, 2f);

        crosshairMovementSpeed *= multiplier;
        
    }

    // Update is called once per frame
    void Update() {
        MoveCrosshair();
    }

    private void MoveCrosshair() {

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

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x + x, 0f, Screen.width);
        position.y = Mathf.Clamp(position.y + y, 0f, Screen.height);

        transform.position = position;
    }
}
