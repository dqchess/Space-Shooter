using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Enemy Stats")]
    [SerializeField] int playerPoints = 150;

    [Header("Shooting")]
    //[SerializeField] float shotCounter = 0.5f;
    [SerializeField] GameObject projectile;

    [Header("Easy")]
    [SerializeField] float easyFireRate = 1.2f;

    [Header("Normal")]
    [SerializeField] float mediumFireRate = 1.7f;

    [Header("Hard")]
    [SerializeField] float hardFireRate = 1.2f;

    private int difficulty;

    private float fireRate;
    private float shotCounter = 0f;
    private Player player;
    private bool canShoot = false;
    private Renderer renderer;

    void Start() {

        difficulty = FindObjectOfType<GameSession>().GetDifficulty();
        player = FindObjectOfType<Player>();
        renderer = transform.parent.transform.GetChild(0).GetComponent<Renderer>();

        SetFireRate();
    }

    // Update is called once per frame
    void Update() {
        if (renderer.isVisible) {
            CountDownAndShoot();
        }

        if (player != null) {
            transform.LookAt(player.transform);
        }

    }

    private void SetFireRate() {
        if (difficulty == GameSession.Easy) {
            fireRate = easyFireRate;
        } else if (difficulty == GameSession.Normal) {
            fireRate = mediumFireRate;
        } else if (difficulty == GameSession.Hard) {
            fireRate = hardFireRate;
        }
    }

    public float GetEnemyKillScore() {
        return playerPoints;
    }

    private void CountDownAndShoot() {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f) {
            Fire();
            shotCounter = fireRate;
        }
    }

    private void Fire() {
        if (difficulty == GameSession.Hard) {
            Instantiate(projectile, transform.position, transform.rotation);
        } else {
            Instantiate(projectile, transform.position, Quaternion.identity);
        }
    }

    private void OnBecameVisible() {
        canShoot = true;
    }

    private void OnBecameInvisible() {
        canShoot = false;
    }
}
