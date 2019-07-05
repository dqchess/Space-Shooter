using System;
using UnityEngine;

public class Missile : MonoBehaviour {

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float destroyVfxAfterSeconds = 2f;

    [Header("Enemy Specific")]

    [Header("Easy")]
    [SerializeField] float easyProjectileSpeed = 180f;
    [Range(0f, 1000f, order = 1)] [SerializeField] float damageEasy = 10f;

    [Header("Normal")]
    [SerializeField] float mediumProjectileSpeed = 40f;
    [Range(0f, 1000f, order = 1)] [SerializeField] float damageMedium = 3f;

    [Header("Hard")]
    [SerializeField] float hardProjectileSpeed = 60f; // Lower, since all of the enemies will aim at player's position
    [Range(0f, 1000f, order = 1)] [SerializeField] float damageHard = 2f;

    [Header("Player Specific")]
    [SerializeField] float playerMissileDamage = 50f;

    private int difficulty;

    private Player player;
    private Vector3 target;
    private float projectileSpeed = 10f; // Default speed
    private float damage;
    private Rigidbody rigidbody;

    void Start() {
        player = FindObjectOfType<Player>();
        difficulty = FindObjectOfType<GameSession>().GetDifficulty();
        rigidbody = GetComponent<Rigidbody>();
        if (transform.CompareTag("Player")) {
            damage = playerMissileDamage;
            target = transform.forward * -1000;

            if (GetComponent<ParticleSystem>() != null) {
                GetComponent<ParticleSystem>().Play();
            }



        } else {
            if (player != null) { // To avoid NullReferenceExpection when player is destroyed
                SetEnemyProjectileSpeedAndTarget();
                Vector3 playerVelocityVector = player.GetVelocityVector();
                //Vector3 missileVelocityVector = target * projectileSpeed;
                //target += playerVelocityVector;
                projectileSpeed += (playerVelocityVector.magnitude * 0.1f);
            }
        }
    }

    private void SetEnemyProjectileSpeedAndTarget() {
        // If missile is fired from the Boss, then shoot at player's position
        if (transform.CompareTag("Boss")) {
            difficulty = GameSession.Hard;
        }

        if (difficulty == GameSession.Easy) {
            damage = damageEasy;
            projectileSpeed = easyProjectileSpeed;
            target = transform.forward * -1000;
        } else if (difficulty == GameSession.Normal) {
            damage = damageMedium;
            transform.LookAt(player.transform);
            projectileSpeed = mediumProjectileSpeed;
            target = player.transform.position - transform.position;
        } else if (difficulty == GameSession.Hard) {
            damage = damageHard;
            transform.LookAt(player.transform);
            projectileSpeed = hardProjectileSpeed;
            target = player.transform.position - transform.position;
        }

    }

    void Update() {
        // Destroy projectiles that are not visible in the gameplay.
        if (transform.position.z >= 250) {
            Destroy(gameObject);
        } else if (transform.position.z <= -50) {
            Destroy(gameObject);
        }

        if (transform.CompareTag("Enemy") || transform.CompareTag("Boss")) {
            //transform.position = Vector3.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);
            rigidbody.velocity = transform.forward * projectileSpeed;
            //if (Vector3.Distance(transform.position, target) <= 0.05f) {
            //    Destroy(gameObject);
            //}
        }
    }

    private void OnTriggerEnter(Collider other) {
        try { // If collider is on parent.
            if (!other.CompareTag("Shield")) {
                other.GetComponent<Health>().ReduceHealth(damage);
            }
            Destroy(gameObject);
            PlaySmallExplosionVFX();
        } catch (NullReferenceException e) {
            try { // If collider is on child, and Health script is attached to parent.
                if (!other.CompareTag("Shield")) {
                    other.GetComponentInParent<Health>().ReduceHealth(damage);
                }
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
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, destroyVfxAfterSeconds);
    }


}
