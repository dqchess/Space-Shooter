using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    // Configurations
    [Header("Firing Configurations")]
    [Range(0f, 5f)] [SerializeField] float delayBetweenFiring = 0.1f;
    [SerializeField] GameObject missilePrefab;
    [Range(1000f, 5000f)] [SerializeField] float projectileSpeed = 3000f;

    // Cached references
    private float currentFiringTime = 0f;

    // Update is called once per frame
    void Update() {
        if (currentFiringTime >= delayBetweenFiring) {
            currentFiringTime = 0f;
            GameObject projectile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * -projectileSpeed);
        }
        currentFiringTime += Time.deltaTime;
    }
}
