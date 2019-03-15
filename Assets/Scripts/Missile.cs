using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    [Range(0f, 1000f)] [SerializeField] float damage;
    [SerializeField] GameObject explosionPrefab;

    void Update() {
        // Destroy projectiles that are not visible in the gameplay.
        if (transform.position.z >= 250) {
            Destroy(gameObject);
        } else if (transform.position.z <= -50) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {

        try { // If collider is on parent.
            other.GetComponent<Health>().ReduceHealth(damage);
            Destroy(gameObject);
            PlaySmallExplosionVFX();
        } catch (NullReferenceException e) {
            Debug.Log(other.name);
            try { // If collider is on child, and Health script is attached to parent.
                other.GetComponentInParent<Health>().ReduceHealth(damage);
                Destroy(gameObject);
                PlaySmallExplosionVFX();
            } catch (NullReferenceException ex) {
                Destroy(gameObject); // If Health script is not attached.
            }
        }
    }

    /// <summary>
    /// When the missile hits any object, play some explosion VFX when the missile gets destroyed
    /// </summary>
    private void PlaySmallExplosionVFX() {
        Debug.Log("PlaySmallExplosionVFX()");
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 2f);
    }


}
