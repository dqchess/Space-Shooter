using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    [Header("Player")]
    [SerializeField] AudioClip playerDead;

    [Header("Boss")]
    [SerializeField] AudioClip bossDead;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPlayerDeadSound() {
        audioSource.PlayOneShot(playerDead);
    }

    public void PlayBossDeadSound() {
        audioSource.PlayOneShot(bossDead);
    }

}
