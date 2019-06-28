using System.Collections;
using UnityEngine;

public class GameSession : MonoBehaviour {

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject joystickPanel;

    public static int Easy = 0;
    public static int Normal = 1;
    public static int Hard = 2;
    private int difficulty;


    private Health playerHealth;
    private bool startedGameOverCoroutine = false;

    private void Awake() {
        // In Awake(), should be loaded before EnemySpawner access difficulty variable.
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
        Time.timeScale = 1f;
        //Time.fixedDeltaTime = Time.unscaledDeltaTime;
    }

    public int GetDifficulty() {
        return difficulty;
    }

    // Update is called once per frame
    void Update() {
        if (playerHealth.GetHealth() <= 0 && !startedGameOverCoroutine) {
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            startedGameOverCoroutine = true;
            StartCoroutine(ShowPanelWithDelay());
        }
    }

    IEnumerator ShowPanelWithDelay() {
        joystickPanel.SetActive(false);
        yield return new WaitForSeconds(0.3f); // With time scale 0.2, gives a delay of 1.5 seconds
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        Time.timeScale = 0f;    // Pause spawning of enemies
    }
}
