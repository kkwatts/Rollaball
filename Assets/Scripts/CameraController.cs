using UnityEngine;

public class CameraController : MonoBehaviour {
    private KeyCode rotateCW;
    private KeyCode rotateCCW;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rotateCW = KeyCode.LeftArrow;
        rotateCCW = KeyCode.RightArrow;
        player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void LateUpdate() {
        if (Input.GetKey(rotateCW) && !Input.GetKey(rotateCCW)) {
            transform.Rotate(0, 1, 0);
        }
        if (Input.GetKey(rotateCCW) && !Input.GetKey(rotateCW)) {
            transform.Rotate(0, -1, 0);
        }
    }
}