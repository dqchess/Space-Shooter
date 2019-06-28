using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    private void Awake() {
        //Application.backgroundLoadingPriority = ThreadPriority.High;
        if (FindObjectsOfType<MusicPlayer>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);
        
    }

    public void UpdateMusicVolume(float volume) {
        GetComponent<AudioSource>().volume = volume;
    }
}
