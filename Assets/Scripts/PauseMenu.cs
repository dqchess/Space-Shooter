using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    [SerializeField] GameObject gameplayPanel;

    public void Resume() {
        gameObject.SetActive(false);
        gameplayPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Pause() {
        Time.timeScale = 0f;
        gameplayPanel.SetActive(false);
        gameObject.SetActive(true);
    }
}
