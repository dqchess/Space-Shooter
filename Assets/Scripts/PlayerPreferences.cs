using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferences : MonoBehaviour {

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Dropdown difficulty;
    [SerializeField] Slider vibrationSensitivitySlider;

    // Start is called before the first frame update
    void Start() {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        difficulty.value = PlayerPrefs.GetInt("Difficulty", 1);
        vibrationSensitivitySlider.value = PlayerPrefs.GetFloat("VibrationSensitivity", 1f);
    }

    public void SetMusicVolume(float volume) {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        FindObjectOfType<MusicPlayer>().UpdateMusicVolume(volume);
    }

    public void SetSFXVolume(float volume) {
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetDifficulty(int difficulty) {
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void SetVibrationSensitivity(float sensitivity) {
        Debug.Log("Sensitivity: " + sensitivity);
        PlayerPrefs.SetFloat("VibrationSensitivity", sensitivity);
    }
}
