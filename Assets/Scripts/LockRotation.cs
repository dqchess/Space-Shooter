using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {

    private Quaternion rotation;

    // Start is called before the first frame update
    void Start() {
        rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Update is called once per frame
    void LateUpdate() {
        transform.rotation = rotation;
    }
}
