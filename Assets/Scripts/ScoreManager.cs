using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private float score = 0;
    private int coins = 0;

    private void Awake() {
        if (FindObjectsOfType<ScoreManager>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetScore(float score) {
        this.score = score;
    }

    public float GetScore() {
        return score;
    }

    public void SetCoins(int coins) {
        this.coins = coins;
    }

    public int GetCoins() {
        return coins;
    }
}
