using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour {

    [SerializeField] AudioSource audioSource;

    [SerializeField] private AudioClip touchSound;
    [SerializeField] private AudioClip menuSound;
    [SerializeField] private AudioClip loadingSound;
    Coroutine sound;
    public void PlayTouchSound() {
        audioSource.clip = touchSound;

        audioSource.Play();


    }

    public void PlayMenuSound() {
        audioSource.clip = menuSound;
        audioSource.Play();
    }

    public void PlayLoadingSound() {
        audioSource.clip = loadingSound;
        audioSource.Play();
    }
}

