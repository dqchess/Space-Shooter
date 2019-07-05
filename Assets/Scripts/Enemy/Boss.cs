using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

    [Header("Bots Configurations")]
    [SerializeField] GameObject botPrefab;

    [Header("Side Bots Transforms")]
    [SerializeField] Transform slot1;
    [SerializeField] Transform slot2;
    [SerializeField] Transform slot3;
    [SerializeField] Transform slot4;

    [Header("Boss Configurations")]
    [SerializeField] GameObject projectile;
    [SerializeField] float fireDelayInSeconds = 1.8f;
    [SerializeField] int playerPoints = 300;
    [SerializeField] GameObject waypointPrefab;
    [SerializeField] float movementSpeed = 10f;

    [Header("Health UI")]
    [SerializeField] Image image;

    [Header("Boss 1 Specifics")]
    [SerializeField] Transform gun1B1;

    [Header("Boss 2 Specifics")]
    [SerializeField] Transform gun1B2;
    [SerializeField] Transform gun2B2;

    [Header("Boss 3 Specifics")]
    [SerializeField] Transform gun1B3;
    [SerializeField] Transform gun2B3;
    [SerializeField] Transform gun3B3;

    private GameObject enemyBot1;
    private GameObject enemyBot2;
    private GameObject enemyBot3;
    private GameObject enemyBot4;
    private Player player;
    private float timer = 0f;
    private int waypointIndex = 0;
    private bool canShoot = false;
    private string levelName;
    private Health health;

    private float boss1Health = 1250f;
    private float boss2Health = 1450f;
    private float boss3Health = 1650f;


    private Vector3 movementDirection;
    private float speed;

    // Start is called before the first frame update
    void Start() {

        levelName = SceneManager.GetActiveScene().name;
        player = FindObjectOfType<Player>();
        health = GetComponent<Health>();

        // Instantiate bots.
        SpawnLeftBots();
        SpawnRightBots();

        // Set transform for the Gun to look at.
        GetComponentInChildren<LookAtGameObject>().SetTarget(player.transform);

        // Boss will shoot only after he is fully visible in the screen
        StartCoroutine(DelayBeforeFirstFire());

    }

    IEnumerator DelayBeforeFirstFire() {
        yield return new WaitForSeconds(3f);
        canShoot = true;
    }

    // Update is called once per frame
    void Update() {
        if (enemyBot1 == null && enemyBot2 == null) {
            SpawnLeftBots();
        }

        if (enemyBot3 == null && enemyBot4 == null) {
            SpawnRightBots();
        }

        Move();

        if (canShoot) {
            Fire();
        }
        UpdateHealthUI();
    }

    private void UpdateHealthUI() {
        if (gameObject.name == "Boss 1 Container(Clone)") {
            image.fillAmount = health.GetHealth() / boss1Health;
        } else if (gameObject.name == "Boss 2 Container(Clone)") {
            image.fillAmount = health.GetHealth() / boss2Health;
        } else if (gameObject.name == "Boss 3 Container(Clone)") {
            image.fillAmount = health.GetHealth() / boss3Health;
        }
    }

    private void Move() {
        Vector3 targetPosition = waypointPrefab.transform.GetChild(waypointIndex).position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);

        if (waypointIndex > 0) {
            movementDirection = waypointPrefab.transform.GetChild(waypointIndex).position - waypointPrefab.transform.GetChild(waypointIndex - 1).position;
        }


        if (Vector3.Distance(transform.position, targetPosition) <= 0.05f) {
            waypointIndex = waypointIndex % (waypointPrefab.transform.childCount - 1) + 1;
        }
    }

    private void SpawnLeftBots() {

        enemyBot1 = Instantiate(botPrefab, transform.position, transform.rotation, transform);
        enemyBot1.GetComponent<EnemyBot>().SetTargetPosition(slot1);


        enemyBot2 = Instantiate(botPrefab, transform.position, transform.rotation, transform);
        enemyBot2.GetComponent<EnemyBot>().SetTargetPosition(slot2);
    }

    private void SpawnRightBots() {
        enemyBot3 = Instantiate(botPrefab, transform.position, transform.rotation, transform);
        enemyBot3.GetComponent<EnemyBot>().SetTargetPosition(slot3);

        enemyBot4 = Instantiate(botPrefab, transform.position, transform.rotation, transform);
        enemyBot4.GetComponent<EnemyBot>().SetTargetPosition(slot4);
    }

    private void Fire() {
        timer -= Time.deltaTime;
        if (timer <= 0f) {

            switch (levelName) {
                case "Level 1":
                    // Fire
                    Instantiate(projectile, gun1B1.position, gun1B1.rotation);
                    break;
                case "Level 2":
                    Instantiate(projectile, gun1B2.position, gun1B2.rotation);
                    Instantiate(projectile, gun2B2.position, gun2B2.rotation);
                    break;
                case "Level 3":
                    Instantiate(projectile, gun1B3.position, gun1B3.rotation);
                    Instantiate(projectile, gun2B3.position, gun2B3.rotation);
                    Instantiate(projectile, gun3B3.position, gun3B3.rotation);
                    break;
            }
            timer = fireDelayInSeconds;
        }
    }

    public float GetBossKillScore() {
        return playerPoints;
    }

    public float GetSpeed() {
        return speed;
    }

    public Vector3 GetVelocityVector() {
        return movementDirection;
    }

}
