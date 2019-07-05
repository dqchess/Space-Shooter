using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    // Configurations
    [Header("Player Configurations")]
    [Range(0f, 2000f)] [SerializeField] float movementSpeed = 800f;

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
    [SerializeField] Vector3 botPosition1;
    [SerializeField] Vector3 botPosition2;
    [SerializeField] Vector3 botPosition3;
    [SerializeField] Vector3 botPosition4;

    // Cached references
    private Rigidbody projectileRigidbody;
    private LineRenderer aimAssistLine;
    private float currentFiringTime = 0f;

    private float x;
    private float y;
    private bool isPlayerReadyForCombat = false;
    private bool canFire = true;
    private bool isPlayerReady = false;
    private bool isPowerUpActive = false;
    private bool disableJoystickInput = false;
    private PowerUpSpawner powerUpSpawner;
    private Coins coins;

    private Transform gun;
    private GameObject bot1;
    private GameObject bot2;
    private GameObject bot3;
    private GameObject bot4;
    private float playerMovementSpeed;
    private Vector3 movementDirection;
    private Vector3 velocityVector;
    private float playerDeadZone;

    [SerializeField] GameObject bot1Prefab;
    [SerializeField] GameObject bot2Prefab;
    [SerializeField] int drone1Coins = 3;
    [SerializeField] int drone2Coins = 4;

    void Start() {
        playerMovementSpeed = 28f;
        gun = transform.Find("Gun").transform;
        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        coins = FindObjectOfType<Coins>();

        playerDeadZone = 20f    ;
    }

    void Update() {
        // Fire and move only after the spaceship reaches it's position
        if (!isPlayerReadyForCombat) {
            if (transform.position.z >= 0) {
                isPlayerReadyForCombat = true;
                playerMovementSpeed = movementSpeed;
            }

        } else {

            Move();

            FireContinueously();

        }


        if (!isPlayerReady) {
            StartCoroutine(PlayerEntrance());
        }

        if (!isPlayerReadyForCombat && isPlayerReady) {
            // Move the player ship to the stating position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, 0f), playerMovementSpeed * Time.deltaTime);
        }

    }

    private void FireContinueously() {
        if (Input.GetMouseButton(0)) {
            if (isPowerUpActive) {
                FirePowerUp();
            } else {
                Fire();
            }
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

        if (!disableJoystickInput) {
            if (joystick.Horizontal >= horizontalJoystickThreshold) {
                x = joystick.Horizontal * playerMovementSpeed;
            } else if (joystick.Horizontal <= horizontalJoystickThreshold) {
                x = joystick.Horizontal * playerMovementSpeed;
            } else {
                x = 0;
            }

            if (joystick.Vertical >= verticalJoystickThreshold) {
                y = joystick.Vertical * playerMovementSpeed;
            } else if (joystick.Vertical <= verticalJoystickThreshold) {
                y = joystick.Vertical * playerMovementSpeed;
            } else {
                y = 0;
            }

            ClampPlayer();
        }
    }

    private void ClampPlayer() {
        movementDirection = new Vector3(x, y, 0);

        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        position.x = Mathf.Clamp(position.x + x, playerDeadZone, Screen.width - playerDeadZone);
        position.y = Mathf.Clamp(position.y + y, playerDeadZone, Screen.height - playerDeadZone);


        velocityVector = movementDirection * playerMovementSpeed;
        transform.position = Camera.main.ScreenToWorldPoint(position);

    }

    public void DisableJoystickInput() {
        disableJoystickInput = true;
    }

    public Vector3 GetVelocityVector() {
        return velocityVector;
    }

    public bool IsPlayerReady() {
        return isPlayerReadyForCombat;
    }

    public float GetPlayerSpeed() {
        return playerMovementSpeed;
    }

    public void SetPlayerSpeed(float speed) {
        playerMovementSpeed = speed;
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

    private IEnumerator DestroyPowerUpCoroutine(GameObject powerUp, float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(powerUp);
    }


    public void PurchaseBot1() {
        if (bot1 == null) {
            bot1 = Spawn(bot1Prefab, botPosition1, drone1Coins);
            bot1.GetComponent<PlayerBot>().SetPosition(1);
        } else if (bot2 == null) {
            bot2 = Spawn(bot1Prefab, botPosition2, drone1Coins);
            bot2.GetComponent<PlayerBot>().SetPosition(2);
        } else if (bot3 == null) {
            bot3 = Spawn(bot1Prefab, botPosition3, drone1Coins);
            bot3.GetComponent<PlayerBot>().SetPosition(3);
        } else if (bot4 == null) {
            bot4 = Spawn(bot1Prefab, botPosition4, drone1Coins);
            bot4.GetComponent<PlayerBot>().SetPosition(4);
        }
    }

    public void PurchaseBot2() {
        if (bot1 == null) {
            bot1 = Spawn(bot2Prefab, botPosition1, drone2Coins);
            bot1.GetComponent<PlayerBot>().SetPosition(1);
        } else if (bot2 == null) {
            bot2 = Spawn(bot2Prefab, botPosition2, drone2Coins);
            bot2.GetComponent<PlayerBot>().SetPosition(2);
        } else if (bot3 == null) {
            bot3 = Spawn(bot2Prefab, botPosition3, drone2Coins);
            bot3.GetComponent<PlayerBot>().SetPosition(3);
        } else if (bot4 == null) {
            bot4 = Spawn(bot2Prefab, botPosition4, drone2Coins);
            bot4.GetComponent<PlayerBot>().SetPosition(4);
        }
    }

    private GameObject Spawn(GameObject bot, Vector3 position, int price) {
        GameObject botSpawned = Instantiate(bot, transform.position, transform.rotation, transform);
        botSpawned.GetComponent<PlayerBot>().SetTargetPosition(position);
        coins.SubtractCoins(price);
        return botSpawned;
    }

    public bool GetCanFire() {
        return canFire;
    }
}
