using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour {

    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float powerUpDuration = 5f;

    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject shieldDestroy;

    private Rigidbody rigidbody;
    private Player player;
    private PowerUpSpawner powerUpSpawner;
    private bool powerUpPicked = false;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularVelocity = Vector3.up * rotationSpeed;

        player = FindObjectOfType<Player>();
        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();

		Destroy(gameObject, powerUpDuration);
    }

    private void OnTriggerEnter(Collider other) {

        if (!powerUpPicked) {
            powerUpPicked = true;
            HideShieldIcon();

            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 0.7f);

            SpawnShield();
        }
    }

    private void HideShieldIcon() {
        //if (GetComponent<SphereCollider>() != null) {
            GetComponent<SphereCollider>().enabled = false;

        //}
        // Destroying gameobject will break the coroutine, so just hide all the mesh renderers.
        foreach (Transform child in transform) {
            child.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void SpawnShield() {
        Vector3 spawnPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1.75f, player.transform.position.z + 1.5f);
        GameObject powerUp = Instantiate(shield, spawnPosition, Quaternion.identity);
        powerUp.transform.parent = player.transform;

        powerUpSpawner.ShowTimer(powerUpDuration);

        StartCoroutine(WaitBeforeSpawning(powerUpDuration));

        // Cannot destroy power up in existing script, so use player script to destroy   
		player.DestroyPowerUp(powerUp, powerUpDuration);

    }

    IEnumerator WaitBeforeSpawning(float seconds) {
        yield return new WaitForSeconds(seconds);
        powerUpSpawner.CanSpawnPowerUp(true);
    }

    private void OnDestroy() {
        if (!powerUpPicked) {
            powerUpSpawner.CanSpawnPowerUp(true);
        }
    }
}
