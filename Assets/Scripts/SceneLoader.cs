using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour {

    [SerializeField] Slider loading;
    [SerializeField] Text percentage;

    private void Start() {
        Time.timeScale = 1f;
    }

    public void LoadNextScene() {
        Debug.Log("LoadNextScene()");
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        Debug.Log("Score Manager loaded");
        if (scoreManager != null) { // GameSession object will not be there during Menu Scene
            scoreManager.SetScore(PlayerPrefs.GetFloat("ScoreTempStore", 0));
            Debug.Log("Score Saved!");
        }

        int scene = SceneManager.GetActiveScene().buildIndex + 1; // Next scene build index.
        Debug.Log("Scene Build Index: " + scene);

        StartCoroutine(LoadAsynchronously(buildIndex: scene));
    }

    public void LoadSceneByName(string scene) {
        StartCoroutine(LoadAsynchronously(sceneName: scene));
    }

    IEnumerator LoadAsynchronously(string sceneName = null, int buildIndex = 0) {
        Debug.Log("LoadAsynchronously coroutine is called");
        AsyncOperation operation;
        // Load scene by scene name or build index number.
        if (buildIndex == 0) {
            operation = SceneManager.LoadSceneAsync(sceneName);
            //Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelUp);
            
        } else {
            operation = SceneManager.LoadSceneAsync(buildIndex);
        }

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("progress: " + progress);

            loading.value = progress;
            percentage.text = (progress * 100).ToString("F0") + "%";
            yield return null;
        }
    }

    public void Quit() {
        Application.Quit();
    }
}
