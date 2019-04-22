using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour {

    [SerializeField] Slider loading;
    [SerializeField] Text percentage;

    public void LoadGamePlay() {
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously() {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game Play");
        
        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loading.value = progress;
            percentage.text = (progress * 100).ToString("F0") + "%";
            yield return null;
        }
    }

    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void Quit() {
        Application.Quit();
    }
}
