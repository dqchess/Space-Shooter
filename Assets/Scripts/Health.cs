using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [Range(0f, 500f)] [SerializeField] float health;
    [SerializeField] GameObject explosionPrefab;
    [Range(0f, 2f)] [SerializeField] float delayBeforeDestroy = 0.1f;


    public float GetHealth() {
        return health;
    }

    public void ReduceHealth(float amount) {
        health -= amount;
        if (health <= 0) {
            DestroyShip();
        }
    }

    private void DestroyShip() {
        Destroy(gameObject, delayBeforeDestroy);
        PlayExplosionVFX();
    }

    private void PlayExplosionVFX() {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(explosion, 2f);
    }
}
