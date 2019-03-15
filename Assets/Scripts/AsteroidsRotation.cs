using System.Collections;
using UnityEngine;

public class AsteroidsRotation : MonoBehaviour {


    float tumble = 0f;
    private float velocity = 20f;

    // Cached references
    Rigidbody rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularVelocity = Random.insideUnitSphere * tumble;

    }

    void Update() {
        rigidbody.velocity = Vector3.back * velocity;

        if (transform.position.z <= -50) {
            Destroy(gameObject);
        }
    }
}