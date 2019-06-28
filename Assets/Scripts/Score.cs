using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {

    private float score = 0f;
    private Text scoreText;
    private ScoreManager scoreManager;

    private void Start() {
        scoreText = gameObject.GetComponent<Text>();
        scoreManager = FindObjectOfType<ScoreManager>();

        score = scoreManager.GetScore();
        UpdateScoreOnUI();

    }

    public void AddScore(float score) {
        this.score += score;
        UpdateScoreOnUI();
    }

    public float GetScore() {
        return score;
    }

    public string GetScoreForUI() {
        return "Your Score: " + score;
    }

    public void ResetScore() {
        score = 0;
    }

    private void UpdateScoreOnUI() {
        //scoreText.text = "Score: " + score;
        scoreText.text = "Score: " + score;

        // Inorder to access score in game over menu. Game Over script couldn't find score object, so temperory fix.
        PlayerPrefs.SetFloat("ScoreTempStore", score);
    }
}
