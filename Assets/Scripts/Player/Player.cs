using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    // Configurations
    [Header("Player Configurations")]
    [Range(0f, 50f)] [SerializeField] float playerMovementSpeed = 20f;

    [Header("Joystick Configurations")]
    [SerializeField] Joystick joystick;
    [Range(0f, 1f)] [SerializeField] float horizontalJoystickThreshold = 0.2f;
    [Range(0f, 1f)] [SerializeField] float verticalJoystickThreshold = 0.2f;

    [Header("Projectile Configurations")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float velocity = 550f;
    [Range(0f, 5f)] [SerializeField] float delayBetweenFiring = 0.3f;
	[Range(0f, 5f)] [SerializeField] float powerUpDelayBetweenFiring = 0.9f;
    [SerializeField] LayerMask projectileLayerMask;


    [Header("Power Up Gun")]
    [SerializeField] GameObject powerUpGun;

    [Header("Bots Position")]
    //[SerializeField] Transform botPosition1;
    //[SerializeField] Transform botPosition2;
    //[SerializeField] Transform botPosition3;
    //[SerializeField] Transform botPosition4;

    [SerializeField] Vector3 botPosition1;
    [SerializeField] Vector3 botPosition2;
    [SerializeField] Vector3 botPosition3;
    [SerializeField] Vector3 botPosition4;

    // Cached references
    private Rigidbody projectileRigidbody;
    private LineRenderer aimAssistLine;
    private float currentFiringTime = 0f;
    private Animator animator;

    // Player rotation animation hashes
    //private int leftRotationHash = Animator.StringToHash("Left rotation");
    //private int rightRotationHash = Animator.StringToHash("Right rotation");
    //private int idleHash = Animator.StringToHash("Idle");

    private float x;
    private float y;
    private bool aimAssistLineIsVisible = true;
    private bool isPlayerReadyForCombat = false;
    private bool canFire = true;
    private bool isPlayerReady = false;
    private bool isPowerUpActive = false;

    private PowerUpSpawner powerUpSpawner;
    private Coins coins;

    private Transform gun;
    private GameObject bot1;
    private GameObject bot2;
    private GameObject bot3;
    private GameObject bot4;

    
    [SerializeField] GameObject bot1Prefab;
    [SerializeField] GameObject bot2Prefab;
    [SerializeField] int drone1Coins = 3;
    [SerializeField] int drone2Coins = 4;

    void Start() {
        //aimAssistLine = gameObject.GetComponent<LineRenderer>();
        //animator = gameObject.GetComponent<Animator>();
        gun = transform.Find("Gun").transform;
        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        coins = FindObjectOfType<Coins>();

    }

    void Update() {
        // Fire and move only after the spaceship reaches it's position
        if (!isPlayerReadyForCombat) {
            if (transform.position.z >= 0) {
                isPlayerReadyForCombat = true;
            }

        } else {

            Move();

            if (isPowerUpActive) {
                FirePowerUp();
            } else {
                Fire();
            }

            /*
            // Aim assist line
            if (aimAssistLineIsVisible) {
                AimAssist();
            } else {
                aimAssistLine.positionCount = 0; // Delete the line which was drawn previously.
            }*/
        }


        if (!isPlayerReady) {
            StartCoroutine(PlayerEntrance());
        }

        if (!isPlayerReadyForCombat && isPlayerReady) {
            // Move the player ship to the stating position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, 0f), playerMovementSpeed * Time.deltaTime);
        }
    }

    IEnumerator PlayerEntrance() {
        // Wait till camera shake is completed
        yield return new WaitForSeconds(2f);

        isPlayerReady = true;
    }

    public void StartFire() {
        canFire = true;
    }

    public void StopFire() {
        canFire = false;
    }

    private void Fire() {
        if (canFire && currentFiringTime >= delayBetweenFiring) {
            currentFiringTime = 0;
            GameObject projectile = Instantiate(projectilePrefab, gun.position, gun.rotation);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * velocity;
        }
        currentFiringTime += Time.deltaTime;
    }

    private void Move() {

        Vector3 currentPosition = transform.position;

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

        // Restrict player movement to viewport
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

        //TiltPlayerSpaceShip(x);

        // Move the player spaceship
        transform.Translate(x, y, 0f, Space.World);

    }

    private void TiltPlayerSpaceShip(float xValue) {
        //Debug.Log("X value = " + xValue);
        float rotationSpeed = 50f;
        //Debug.Log("Rotating..");
        
        if (xValue < 0) {
            //Debug.Log("Left rotation");
            //animator.Play(leftRotationHash);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(30, Vector3.forward), rotationSpeed);
        } else if (xValue > 0) {
            //animator.Play(rightRotationHash);
            //Debug.Log("Right rotation");
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(-30, Vector3.forward), rotationSpeed);

        } else {
            //animator.Play(idleHash);
            //Debug.Log("Idle");
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), rotationSpeed);
        }
    }

    public void SetAimAssistLineIsVisible(bool isVisible) {
        Debug.Log("SetAimAssistLine");
        aimAssistLineIsVisible = isVisible;
    }


    private void AimAssist() {
        Vector3 from = gun.position;
        Vector3 to = new Vector3(gun.position.x, gun.position.y, gun.forward.z + 250);

        RaycastHit hitInfo;
        // If the line hits any object
        if (Physics.Linecast(from, to, out hitInfo, ~projectileLayerMask)) {
            to = hitInfo.point;
        }

        // Line renderer
        Vector3[] newPositions = new Vector3[2];
        newPositions[0] = from;
        newPositions[1] = to;

        aimAssistLine.positionCount = newPositions.Length;
        aimAssistLine.SetPositions(newPositions);
    }

    public bool IsPlayerReady() {
        return isPlayerReadyForCombat;
    }

    public float GetPlayerSpeed() {
        return playerMovementSpeed;
    }

    public void SetIdleAnimation() {
        //animator.Play(idleHash);
        

    }

    public void ResetForNextLevel() {
        Vector3 initialPosition = new Vector3(0f, 0f, -150);
        transform.position = initialPosition;

        isPlayerReadyForCombat = false;
        canFire = false;
        isPlayerReady = false;

        gameObject.GetComponent<Health>().SetHealth(100f);
    }

    public void SetPowerUpGunActive(float seconds) {
        isPowerUpActive = true;
        Debug.Log("PowerUp Gun active");
        StartCoroutine(FireForSeconds(seconds));
    }

    private IEnumerator FireForSeconds(float seconds) {

        yield return new WaitForSeconds(seconds);
        isPowerUpActive = false;
        powerUpSpawner.CanSpawnPowerUp(true);

    }

    private void FirePowerUp() {
        if (canFire && currentFiringTime >= powerUpDelayBetweenFiring) {
            currentFiringTime = 0;

            // Fire missile from all the guns
            foreach (Transform child in powerUpGun.transform) {
                GameObject projectile = Instantiate(projectilePrefab, child.position, child.rotation);
                projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * velocity;
            }

        }
        currentFiringTime += Time.deltaTime;
    }

    public void DestroyPowerUp(GameObject powerUp, float seconds) {
        StartCoroutine(DestroyPowerUpCoroutine(powerUp, seconds));
    }

    private IEnumerator DestroyPowerUpCoroutine(GameObject powerUp,float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(powerUp);
    }


    public void PurchaseBot1() {
        Debug.Log("Purchasing Bot 1");
        if (bot1 == null) {
            bot1 = Spawn(bot1Prefab, botPosition1, drone1Coins);
        } else if (bot2 == null) {
            bot2 = Spawn(bot1Prefab, botPosition2, drone1Coins);
        } else if (bot3 == null) {
            bot3 = Spawn(bot1Prefab, botPosition3, drone1Coins);
        } else if (bot4 == null) {
            bot4 = Spawn(bot1Prefab, botPosition4, drone1Coins);
        } 
    }

    public void PurchaseBot2() {
        Debug.Log("Purchasing Bot 2");
        if (bot1 == null) {
            bot1 = Spawn(bot2Prefab, botPosition1, drone2Coins);
        } else if (bot2 == null) {
            bot2 = Spawn(bot2Prefab, botPosition2, drone2Coins);
        } else if (bot3 == null) {
            bot3 = Spawn(bot2Prefab, botPosition3, drone2Coins);
        } else if (bot4 == null) {
            bot4 = Spawn(bot2Prefab, botPosition4, drone2Coins);
        } 
    }

    private GameObject Spawn(GameObject bot, Vector3 position, int price) {
        GameObject botSpawned = Instantiate(bot, transform.position, transform.rotation, transform);
        botSpawned.GetComponent<PlayerBot>().SetTargetPosition(position);
        coins.SubtractCoins(price);
        return botSpawned;
    }
}
