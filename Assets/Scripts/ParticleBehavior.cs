using UnityEngine;

public class ParticleBehavior : MonoBehaviour {
    Quaternion originalRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        originalRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = originalRotation;
    }
}
