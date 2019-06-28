using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [Range(0f, 5000f)] [SerializeField] float health;
    [SerializeField] GameObject explosionPrefab;
    [Range(0f, 2f)] [SerializeField] float delayBeforeDestroy = 0.1f;
    [Range(0f, 20f)] [SerializeField] float delayBeforeVFXDestroy = 5f;

    [Header("Player Specific")]
    [SerializeField] Text healthText;

    private Score scoreObject;
    private EnemySpawner enemySpawner;
    private PowerUpSpawner powerUpSpawner;
    private SoundManager soundManager;

    void Start() {
        scoreObject = FindObjectOfType<Score>();
        // When Game Over UI panel is shown, the score object gets disabled.
        if (scoreObject != null) {
            scoreObject = scoreObject.GetComponent<Score>();
        }
        enemySpawner = FindObjectOfType<EnemySpawner>();
        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        soundManager = FindObjectOfType<SoundManager>();

    }

    public float GetHealth() {
        return health;
    }

    public void ReduceHealth(float amount) {
        health -= amount;
        if (gameObject.CompareTag("Player")) {
            UpdateHealthOnUI();
            Vibrate();
        }

        if (health <= 0) {
            float score = 0f;
            if (gameObject.CompareTag("Enemy")) {
                score = gameObject.GetComponentInChildren<Enemy>().GetEnemyKillScore();
                enemySpawner.IncreaseNumberOfEnemyKilled();
                powerUpSpawner.SpawnPowerUp(gameObject.transform.position);

                PlayGames.IncrementAchievements(GPGSIds.achievement_drone_buster, 1);
            } else if (gameObject.CompareTag("Boss")) {
                score = gameObject.GetComponent<Boss>().GetBossKillScore();
                soundManager.PlayBossDeadSound();
                // Start loading next level with animation
                FindObjectOfType<GameSession>().GetComponent<LevelControl>().PlayExitAnimation();
            } else if (gameObject.CompareTag("EnemyBot")) {
                score = gameObject.GetComponent<EnemyBot>().GetBotKillScore();
            } else if (gameObject.CompareTag("Player")) {
                soundManager.PlayPlayerDeadSound();
            }

            scoreObject.AddScore(score);
            DestroyShip();
        }
    }

    private void Vibrate() {
        float vibrationSensitivity = PlayerPrefs.GetFloat("VibrationSensitivity", 1f);
        long milliseconds = 50 * Convert.ToInt64(vibrationSensitivity);

        Vibration.Vibrate(milliseconds);
    }

    private void UpdateHealthOnUI() {
        healthText.text = "Health: " + health;
    }

    private void DestroyShip() {
        Destroy(gameObject, delayBeforeDestroy);
        PlayExplosionVFX();
    }

    private void PlayExplosionVFX() {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(explosion, delayBeforeVFXDestroy);
    }

    public void SetHealth(float newHealth) {
        health = newHealth;
    }
}
