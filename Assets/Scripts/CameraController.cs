using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;

    private Vector3 offset;
    private Quaternion originalRotation;

    private GameObject rotator;
    private KeyCode rotateCW;
    private KeyCode rotateCCW;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        offset = transform.position - player.transform.position;
        rotateCW = KeyCode.LeftArrow;
        rotateCCW = KeyCode.RightArrow;

        if (gameObject.tag == "Game Camera") {
            rotator = transform.parent.gameObject;
            originalRotation = rotator.transform.rotation;
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if (player != null && gameObject.tag == "Game Camera") {
            rotator.transform.rotation = originalRotation;
            if (Input.GetKey(rotateCW) && !Input.GetKey(rotateCCW)) {
                rotator.transform.Rotate(0, 1, 0);
                originalRotation = rotator.transform.rotation;
            }
            if (Input.GetKey(rotateCCW) && !Input.GetKey(rotateCW)) {
                rotator.transform.Rotate(0, -1, 0);
                originalRotation = rotator.transform.rotation;
            }
        }
    }
}