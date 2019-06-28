using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPowerUp : MonoBehaviour {

    [SerializeField] float rotationSpeed;
    [SerializeField] float powerUpDuration;
    [SerializeField] GameObject explosion;

    private PowerUpTimerUI timer;
    private PowerUpSpawner powerUpSpawner;
    private Rigidbody rigidbody;
    private Player player;
    private bool enablePowerUp = true;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularVelocity = Vector3.up * rotationSpeed;

        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        player = FindObjectOfType<Player>();
        timer = FindObjectOfType<PowerUpTimerUI>();

		Destroy(gameObject, powerUpDuration);
    }

    private void OnTriggerEnter(Collider other) {
        if (enablePowerUp) {
            powerUpSpawner.CanSpawnPowerUp(false);
            player.SetPowerUpGunActive(powerUpDuration);
            powerUpSpawner.ShowTimer(powerUpDuration);

            GameObject vfx = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(vfx, 4f);
            Destroy(gameObject);
    
        }
        enablePowerUp = false;
    }

    private void OnDestroy() {
        if (!enablePowerUp) {
            powerUpSpawner.CanSpawnPowerUp(true);
        }
    }
}
