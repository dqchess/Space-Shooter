using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Configurations
    [Header("Player Configurations")]
    [Range(0f, 50f)] [SerializeField] float playerMovementSpeed = 20f;
    [Range(0f, 90f)] [SerializeField] float playerMovementRotation = 30f;
    [Range(0f, 10f)] [SerializeField] float rotationSpeed = 3f;

    [Header("Joystick Configurations")]
    [SerializeField] Joystick joystick;
    [Range(0f, 1f)] [SerializeField] float horizontalJoystickThreshold = 0.2f;
    [Range(0f, 1f)] [SerializeField] float verticalJoystickThreshold = 0.2f;

    [Header("Projectile Configurations")]
    [SerializeField] GameObject projectilePrefab;
    [Range(1000f, 5000f)] [SerializeField] float projectileSpeed = 2000f;
    [Range(0f, 5f)] [SerializeField] float delayBetweenFiring = 0.1f;

    // Cached references
    private Rigidbody projectileRigidbody;
    private LineRenderer aimAssistLine;
    private float currentFiringTime = 0f;
    private Animator animator;

    // Player rotation animation hashes
    private int leftRotationHash = Animator.StringToHash("Left rotation");
    private int rightRotationHash = Animator.StringToHash("Right rotation");
    private int idleHash = Animator.StringToHash("Idle");



    private float x;
    private float y;
    private bool aimAssistLineIsVisible = false;
    public bool isCombatStarted = false;

    void Start() {
        aimAssistLine = gameObject.GetComponent<LineRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update() {
        // Fire and move only after the spaceship reaches it's position
        if (!isCombatStarted) {
            Debug.Log("Combat not started!");
            if (transform.position.z >= 0) {
                isCombatStarted = true;
            }
        } else {
            Debug.Log("Combat started!");

            Move();
            Fire();

            Debug.Log("aimAssistLineIsVisible: " + aimAssistLineIsVisible);
            if (aimAssistLineIsVisible) {
                AimAssist();
            } else {
                aimAssistLine.positionCount = 0; // Delete the line which was drawn previously.
            }
        }

        if (!isCombatStarted) {
            // Move the player ship to the stating position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, 0f), playerMovementSpeed * Time.deltaTime);
        }

    }

    private void Fire() {
        if (Input.GetButton("Fire1")) {
            FireContinueously();
        }
    }

    private void FireContinueously() {
        if (currentFiringTime >= delayBetweenFiring) {
            currentFiringTime = 0;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * projectileSpeed);
        }
        currentFiringTime += Time.deltaTime;
    }

    private void Move() {

        Vector3 currentPosition = transform.position;

#if UNITY_ANDROID
        if (joystick.Horizontal >= horizontalJoystickThreshold) {
            x = joystick.Horizontal * playerMovementSpeed * Time.deltaTime;
        } else if (joystick.Horizontal <= horizontalJoystickThreshold) {
            x = joystick.Horizontal * playerMovementSpeed * Time.deltaTime;
        } else {
            x = 0;
        }

        if (joystick.Vertical >= verticalJoystickThreshold) {
            y = joystick.Vertical * playerMovementSpeed * Time.deltaTime;
        } else if (joystick.Vertical <= verticalJoystickThreshold) {
            y = joystick.Vertical * playerMovementSpeed * Time.deltaTime;
        } else {
            y = 0;
        }

        // Debug.Log("\nX: " + x + "Y: " + y);
#else
        x = Input.GetAxis("Horizontal") * playerMovementSpeed * Time.deltaTime;
        y = Input.GetAxis("Vertical") * playerMovementSpeed * Time.deltaTime;
#endif

        // Restrict player movement to viewport
        if (currentPosition.x + x >= 29) {
            x = 0;
        }

        if (currentPosition.x + x <= -29) {
            x = 0;
        }

        if (currentPosition.y + y <= -9) {
            y = 0;
        }

        if (currentPosition.y + y >= 29) {
            y = 0;
        }

        TiltPlayerSpaceShip(x);

        // Move the player spaceship
        transform.Translate(x, y, 0f, Space.World);

    }

    private void TiltPlayerSpaceShip(float xValue) {
        if (xValue < 0) {
            animator.Play(leftRotationHash);
        } else if (xValue > 0) {
            animator.Play(rightRotationHash);
        } else {
            animator.Play(idleHash);
        }
    }

    public void SetAimAssistLineIsVisible(bool isVisible) {
        aimAssistLineIsVisible = isVisible;
    }


    private void AimAssist() {
        Vector3 from = transform.position;
        Vector3 to = new Vector3(transform.position.x, transform.position.y, transform.forward.z + 250);

        RaycastHit hitInfo;
        // If the line hits any object
        if (Physics.Linecast(from, to, out hitInfo)) {
            to = hitInfo.point;
        }

        Vector3[] newPositions = new Vector3[2];
        newPositions[0] = from;
        newPositions[1] = to;

        aimAssistLine.positionCount = newPositions.Length;
        aimAssistLine.SetPositions(newPositions);
    }
}
