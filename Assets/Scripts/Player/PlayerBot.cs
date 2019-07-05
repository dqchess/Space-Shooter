using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBot : MonoBehaviour {

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float botFireDelay = 1f;
    [SerializeField] float velocity = 350f;

    private float timer = 0f;
    private Vector3 targetPosition;
    private Player player;
    private Transform gun;
    private int position;
    private Vector3 offset;

    private float xMin = -35.8f;
    private float xMax = 35f;
    private float yMin = -3.2f;
    private float yMax = 30.2f;
    private EnemyPathing enemy;
    private Boss boss;
    private EnemyBot bossBot;


    private void Start() {
        player = FindObjectOfType<Player>();
        gun = transform.Find("Gun").transform;

    }

    public void SetTargetPosition(Vector3 position) {
        targetPosition = position;
    }

    // Update is called once per frame
    void Update() {

        CalculateFiringPosition();

        MaintainDistance();
    }

    private void MaintainDistance() {
        transform.position = player.transform.position - targetPosition;
    }

    private void CalculateFiringPosition() {

        if (enemy == null) { // Select enemy only if previous selected enemy is dead
            enemy = GetRandomEnemy();
        }

        if (enemy != null) { // If there are no enemy in the scene
            float speed = enemy.GetSpeed();
            Vector3 direction = enemy.GetMovementDirection();

            Vector3 enemyDirection = direction * speed;

            if (timer <= 0 && player.GetCanFire()) {
                timer = botFireDelay;
                GameObject missile = Instantiate(projectilePrefab, gun.position, gun.rotation);

                Vector3 firingDirection = enemy.transform.position + enemyDirection.normalized * (speed / 2);

                gun.transform.LookAt(firingDirection);
                missile.GetComponent<Rigidbody>().velocity = gun.transform.forward * velocity;
                missile.transform.parent = null;
            }
            timer -= Time.deltaTime;
        } else {
            if (boss == null) {
                boss = FindObjectOfType<Boss>();
            }
            if (bossBot == null) {
                bossBot = FindObjectOfType<EnemyBot>();
            }
            if (boss != null) {
                Vector3 direction = boss.GetVelocityVector();
                Vector3 movementDirection = direction * boss.GetSpeed();

                if (timer <= 0 && player.GetCanFire()) {
                    timer = botFireDelay;
                    GameObject missile = Instantiate(projectilePrefab, gun.position, gun.rotation);

                    Vector3 firingDirection = bossBot.transform.position + movementDirection.normalized * (boss.GetSpeed() / 2);

                    gun.transform.LookAt(firingDirection);
                    missile.GetComponent<Rigidbody>().velocity = gun.transform.forward * velocity;
                    missile.transform.parent = null;
                }
                timer -= Time.deltaTime;
            }
        }
    }

    private EnemyPathing GetRandomEnemy() {
        EnemyPathing[] enemies = FindObjectsOfType<EnemyPathing>();

        foreach (EnemyPathing enemy in enemies) {
            if (enemy.transform.position.x > xMin && enemy.transform.position.x < xMax && enemy.transform.position.y > yMin && enemy.transform.position.y < yMax) {
                return enemy;
            }
        }
        return null;
    }

    public void SetPosition(int position) {
        this.position = position;
    }
}
