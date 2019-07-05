using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

    [Header("Movement Configurations")]
    [SerializeField] List<WaypointConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = true;
    [SerializeField] float delayBetweenWaves = 1f;

    [Header("Boss Configurations")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject bossSpawnPoint;
    [SerializeField] float bossSpawnDelay = 3f;

    [Header("Difficulty: Easy")]
    [SerializeField] int killsBeforeBossArrivesEasy = 30;

    [Header("Difficulty: Normal")]
    [SerializeField] int killsBeforeBossArrivesMedium = 30;

    [Header("Difficulty: Hard")]
    [SerializeField] int killsBeforeBossArrivesHard = 30;


    [Header("UI")]
    [SerializeField] Text enemiesKilledText;
    [SerializeField] Text bossWarningText;

    private int killsBeforeBossArrives;
    private int numberOfEnemiesKilled = 0;
    private Player player;
    private bool startedSpawning = false;
    private bool bossSpawned = false;

    // Start is called before the first frame update
    private void Start() {
        player = FindObjectOfType<Player>();

        int difficulty = FindObjectOfType<GameSession>().GetDifficulty();
        if (difficulty == GameSession.Easy) {
            killsBeforeBossArrives = killsBeforeBossArrivesEasy;
        } else if (difficulty == GameSession.Normal) {
            killsBeforeBossArrives = killsBeforeBossArrivesMedium;
        } else if (difficulty == GameSession.Hard) {
            killsBeforeBossArrives = killsBeforeBossArrivesHard;
        }
        enemiesKilledText.text = "00/" + killsBeforeBossArrives;

    }

    private void Update() {
        // If player is ready, spawn enemies
        if (!startedSpawning && player.IsPlayerReady()) {
            startedSpawning = true;
            StartCoroutine(StartSpawningEnemies());
        }
    }

    IEnumerator StartSpawningEnemies() {
        do {
            if (numberOfEnemiesKilled >= killsBeforeBossArrives) {
                looping = false;
            }
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private IEnumerator SpawnAllWaves() {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++) {
            var currentWave = waveConfigs[waveIndex];

            int num = FindObjectsOfType<EnemyPathing>().Length; // Find number of enemies on the screen.
                                                                // Debug.Log("Enemy Number on screen: " + num);
            if (num > (currentWave.GetNumberOfEnemy() / 2)) {
                yield return new WaitForSeconds((num - currentWave.GetNumberOfEnemy() / 2) * 2f);
            }
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaypointConfig waveConfig) {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemy(); enemyCount++) {
            var enemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWaypoints()[0].transform.position, waveConfig.GetWaypoints()[0].rotation);
            enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
        yield return new WaitForSeconds(delayBetweenWaves);

    }

    public void IncreaseNumberOfEnemyKilled() {
        if (numberOfEnemiesKilled < killsBeforeBossArrives) {
            numberOfEnemiesKilled++;
        }

        if (numberOfEnemiesKilled == killsBeforeBossArrives && !bossSpawned) {
            // Stop spawning enemies.
            StopAllCoroutines(); // NOT RECOMMENDED, check this later. Inside if statement because to allow BossSpawnWithDelay() Coroutine.

            // Show warning message
            bossWarningText.gameObject.SetActive(true);

            bossSpawned = true;
            StartCoroutine(BossSpawnWithDelay(bossSpawnDelay));
        }
        UpdateKillsOnUI();
    }

    IEnumerator BossSpawnWithDelay(float seconds) {
        yield return new WaitForSeconds(seconds);
        bossWarningText.gameObject.SetActive(false);

        SpawnBoss();
    }

    private void UpdateKillsOnUI() {
        if (numberOfEnemiesKilled < 10) {
            enemiesKilledText.text = "0" + numberOfEnemiesKilled.ToString() + "/" + killsBeforeBossArrives;
        } else {
            enemiesKilledText.text = numberOfEnemiesKilled.ToString() + "/" + killsBeforeBossArrives;
        }
    }

    public int GetNumberOfEnemiesKilled() {
        return numberOfEnemiesKilled;
    }

    private void SpawnBoss() {
        Instantiate(bossPrefab, bossSpawnPoint.transform.GetChild(0).transform.position, Quaternion.Euler(0f, 180f, 0f));

    }
}
