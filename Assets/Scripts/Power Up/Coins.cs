using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour {

    private int coins = 0;
    private ScoreManager scoreManager;
    private Text text;
    private PurchaseBots purchaseBots;

    // Start is called before the first frame update
    void Start() {
        scoreManager = FindObjectOfType<ScoreManager>();
        text = gameObject.GetComponent<Text>();
        coins = scoreManager.GetCoins();

        purchaseBots = FindObjectOfType<PurchaseBots>();

        coins = scoreManager.GetCoins();
        UpdateUI();
    }

    public void AddCoins(int coins) {
        this.coins += coins;

        PlayGames.IncrementAchievements(GPGSIds.achievement_gold_grab, 1);

        UpdateUI();
    }

    public int GetCoins() {
        return coins;
    }

    public void SubtractCoins(int coins) {
        if (this.coins >= coins) {
            this.coins -= coins;
        } else {
            this.coins = 0;
        }

        UpdateUI();
    }

    private void UpdateUI() {
        // Update coins on UI
        text.text = coins.ToString();
        scoreManager.SetCoins(coins);

        if (coins >= 3) {
            purchaseBots.EnableBot1Button(true);
        } else {
            purchaseBots.EnableBot1Button(false);
        }

        if (coins >= 4) {
            purchaseBots.EnableBot2Button(true);
        } else {
            purchaseBots.EnableBot2Button(false);
        }
    }
}
