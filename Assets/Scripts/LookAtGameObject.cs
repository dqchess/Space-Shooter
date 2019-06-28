using UnityEngine;

public class LookAtGameObject : MonoBehaviour {

    [SerializeField] Transform target;

    public void SetTarget(Transform target) {
        this.target = target;
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(target);
    }
}
