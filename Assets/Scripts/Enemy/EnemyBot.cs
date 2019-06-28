using UnityEngine;

public class EnemyBot : MonoBehaviour {

    [Header("Bot Configurations")]
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float botFireDelay = 1.2f;
    [SerializeField] int playerPoints = 200;

    private Transform targetPosition;
    private bool canShoot = false;
    private float timer = 0f;
    private int difficulty;

    private void Start() {
        difficulty = FindObjectOfType<GameSession>().GetDifficulty();
    }

    public void SetTargetPosition(Transform position) {
        targetPosition = position;
    }

    // Update is called once per frame
    void Update() {
        if (targetPosition != null) {
            MoveToPosition();
        }

        if (canShoot) {
            Fire();
        }
    }

    private void MoveToPosition() {
        float distance = Vector3.Distance(targetPosition.position, transform.position);

        if (distance >= 0) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, Time.deltaTime * movementSpeed);
        }

        if (distance <= 0.05f) {
            canShoot = true;
        }
    }

    private void Fire() {
        if (timer <= 0) {
            timer = botFireDelay;

            // Fire
            if (difficulty == GameSession.Hard) {
                Instantiate(projectilePrefab, transform.position, transform.rotation);
            } else {
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            }
        }
        timer -= Time.deltaTime;
    }
    public float GetBotKillScore() {
        return playerPoints;
    }
}
