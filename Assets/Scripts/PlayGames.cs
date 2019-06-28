using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGames : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
    }

    void SignIn() {
        Social.localUser.Authenticate(success => { });
    }

    public static void IncrementAchievements(string id, int stepToIncrement) {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepToIncrement, success => { });
    }

    public static void AddScoreToLeaderBoard(string id, long score) {
        Social.ReportScore(score, id, success => { });
    }

    public void ShowLeaderBoardUI() {
        Debug.Log("***** ShowLeaderBoardUI() *****");
        Social.ShowLeaderboardUI();
        Debug.Log("***** LeaderboardUI shown *****");
    }

    public void ShowAchievementsUI() {
        Debug.Log("***** ShowAchievementsUI() *****");

        Social.ShowAchievementsUI();
        Debug.Log("***** Achievements shown *****");

    }
}
