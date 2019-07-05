using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour {
    [Header("Level Selection Menu")]
    [SerializeField] Button level2;
    [SerializeField] Button level3;

    [Header("Menu References")]
    [SerializeField] GameObject gamePlayPanel;
    [SerializeField] GameObject nextLevelPanel;

    [Header("Exit Gate Prefab")]
    [SerializeField] GameObject exitGatePrefab;

    private Player player;
    private bool canMove = false;
    private GameObject exitGate;

    void Start() {
        if (SceneManager.GetActiveScene().name == "Menu") {
            int levelUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 1);
            switch (levelUnlocked) {
                case 1:
                    level2.interactable = false;
                    level3.interactable = false;
                    break;
                case 2:
                    level3.interactable = false;
                    break;
            }
        }
        player = FindObjectOfType<Player>();
    }

    public void PlayExitAnimation() {
        int levelUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 1) + 1;
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelUp, new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.EventLevelUp, SceneManager.GetActiveScene().buildIndex));
        PlayerPrefs.SetInt("LevelsUnlocked", levelUnlocked);

        player.DisableJoystickInput();
        player.StopFire();
        player.SetPlayerSpeed(34);
        
        // If last level, add score to leaderboard
        float score = FindObjectOfType<Score>().GetScore();
        PlayGames.AddScoreToLeaderBoard(GPGSIds.leaderboard_space_shooter__leaderboard, long.Parse(score.ToString()));

        StartCoroutine(WaitForPlayerToMove());

        // Spawn exit gate
        Vector3 position = new Vector3(0f, 0f, 250f);
        exitGate = Instantiate(exitGatePrefab, position, Quaternion.Euler(-90f, 0f, 180f));
        // Move player towards that gate
        canMove = true;
    }

    IEnumerator WaitForPlayerToMove() {
        gamePlayPanel.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        // Show Go to next level menu
        nextLevelPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        player.gameObject.SetActive(false);
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update() {
        if (canMove && exitGate != null && player != null) {
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitGate.transform.position, player.GetPlayerSpeed() * Time.deltaTime);
        }
    }
}
