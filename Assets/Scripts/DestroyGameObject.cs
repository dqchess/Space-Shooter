using System.Collections;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour {
    [SerializeField] float destroyAfterSeconds;

    private void Start() {
        StartCoroutine(DestroyAfter(destroyAfterSeconds));
    }

    IEnumerator DestroyAfter(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
