using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBot : MonoBehaviour {

    [SerializeField] float movementSpeed = 20f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float botFireDelay = 1f;
    [SerializeField] float projectileSpeed = 20000f;

    private float timer = 0f;
    private Vector3 targetPosition;
    private Player player;

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    public void SetTargetPosition(Vector3 position) {
        targetPosition = position;
    }

    // Update is called once per frame
    void Update() {

        Fire();

        MaintainDistance();
    }

    private void MaintainDistance() {
        transform.position = player.transform.position - targetPosition;
    }

    private void Fire() {
        if (timer <= 0) {
            timer = botFireDelay;
            GameObject missile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            missile.GetComponent<Rigidbody>().AddForce(missile.transform.forward * projectileSpeed);
            missile.transform.parent = null;
        }
        timer -= Time.deltaTime;
    }
}
