using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTimerUI : MonoBehaviour {

    [SerializeField] Slider slider;

    private bool showTimer = false;
    private float seconds;

    // Update is called once per frame
    void Update() {
        if (showTimer && slider.value > 0) {
            slider.value -= (1 / seconds) * Time.deltaTime;
        }
    }

    public void ShowTimer(float seconds) {
        this.seconds = seconds;
        slider.value = 1f;
        gameObject.SetActive(true);
        showTimer = true;

        StartCoroutine(HideSlider(seconds));
    }

    IEnumerator HideSlider(float seconds) {
        yield return new WaitForSeconds(seconds);
        showTimer = false;
        gameObject.SetActive(false);
    }


}
