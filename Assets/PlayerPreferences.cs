using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferences : MonoBehaviour {

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Dropdown difficulty;

    // Start is called before the first frame update
    void Start() {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 100f);
        difficulty.value = PlayerPrefs.GetInt("Difficulty", 0);
    }

    public void SetMusicVolume(float volume) {
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume) {
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetDifficulty(int difficulty) {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }
}
