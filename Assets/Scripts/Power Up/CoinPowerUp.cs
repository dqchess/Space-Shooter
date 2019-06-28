using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPowerUp : MonoBehaviour {

    [SerializeField] float coinAngularVelocity = 1f;
    [SerializeField] GameObject coinExplosionEffect;

    private Rigidbody rigidbody;
    private PowerUpSpawner powerUpSpawner;
    private Coins coins;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularVelocity = Vector3.up * coinAngularVelocity;

        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        coins = FindObjectOfType<Coins>();

		Destroy(gameObject, 4f);

    }

    private void OnTriggerEnter(Collider other) {
        powerUpSpawner.CanSpawnPowerUp(true);
        Destroy(gameObject);

        // Increase the coin count
        coins.AddCoins(1);

        // Instantiate coin explosion particles
        GameObject vfx = Instantiate(coinExplosionEffect, transform.position, Quaternion.identity);
        Destroy(vfx, 4f);

    }

    private void OnDestroy() {
        powerUpSpawner.CanSpawnPowerUp(true);
    }
}
