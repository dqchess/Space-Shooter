using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

    [SerializeField] GameObject coinsPrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject multipleGuns;
    [SerializeField] GameObject timer;

    private bool canSpawnPowerUp = true;

    private const int COINS = 1;
    private const int SHIELD = 2;
    private const int LASER_GUN = 3;

    private int SelectPowerUp() {
        int powerUpRandom = Random.Range(0, 101); // Randomly select the power up
        int powerUp = -1;

        if (powerUpRandom > 40 && powerUpRandom <= 80) {
            powerUp = COINS;
        } else if (powerUpRandom > 80 && powerUpRandom <= 90) {
            powerUp = SHIELD;
        } else if (powerUpRandom > 90) {
            powerUp = LASER_GUN;
        }

        //Debug.Log("***** POWER UP: " + powerUp + " *****");
        return powerUp;
    }

    public void SpawnPowerUp(Vector3 spawnPosition) {
        if (canSpawnPowerUp) {
            int powerUp = SelectPowerUp();
            //Debug.Log("Power Up: " + powerUp);
            if (powerUp != -1) {
                // Don't spawn powerup till the spawned power up is destoryed.
                CanSpawnPowerUp(false);

            }

            switch (powerUp) {
                case COINS:
                    Instantiate(coinsPrefab, spawnPosition, Quaternion.identity);
                    break;
                case SHIELD:
                    Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
                    break;
                case LASER_GUN:
                    Instantiate(multipleGuns, spawnPosition, Quaternion.identity);
                    break;
            }
        }
    }

    public void CanSpawnPowerUp(bool state) {
        canSpawnPowerUp = state;
        //Debug.Log("Can Spawn PowerUp: " + canSpawnPowerUp);

    }

    public void ShowTimer(float seconds) {
        timer.GetComponent<PowerUpTimerUI>().ShowTimer(seconds);
        StartCoroutine(EnablePowerUpSpawn(seconds));
    }

    private IEnumerator EnablePowerUpSpawn(float seconds) {
        yield return new WaitForSeconds(seconds);
        CanSpawnPowerUp(true);
    }
}
