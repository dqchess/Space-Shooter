using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    [SerializeField] Text finalScoreText;

    private ScoreManager scoreManager;

    private void Start() {
        float finalScore = PlayerPrefs.GetFloat("ScoreTempStore", 0);
        finalScoreText.text = "Score: " + finalScore.ToString();

 
        Firebase.Analytics.Parameter score = new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterScore, finalScore.ToString());;
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventPostScore, score);

        // Add Score to Leaderboard
        PlayGames.AddScoreToLeaderBoard(GPGSIds.leaderboard_space_shooter__leaderboard, long.Parse(finalScore.ToString()));

        // Destroy all the enemies.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }

        // Destroy boss is exists.
        GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss");

        foreach (GameObject b in boss) {
            Destroy(b);
        }
    }


    private void ResetStats() {

        Destroy(FindObjectOfType<GameSession>());
        Destroy(FindObjectOfType<Score>());
        PlayerPrefs.SetFloat("ScoreTempStore", 0f);

        // To restart level from pause menu, scoremanager should be initialised
        scoreManager = FindObjectOfType<ScoreManager>();

        scoreManager.SetScore(0);
        scoreManager.SetCoins(0);
    }

    public void LoadMainMenu() {
        ResetStats();
        SceneManager.LoadScene("Menu");
    }

    public void RestartSameLevel() {
        ResetStats();
        // Restart Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
