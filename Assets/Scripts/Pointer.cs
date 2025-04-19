using UnityEngine;

public class Pointer : MonoBehaviour {
    public GameObject player;
    public GameObject pile;
    public GameObject cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        GameObject grabbedObject = player.GetComponent<PlayerController>().grabbedObject;
        if (grabbedObject == pile) {
            transform.position = new Vector3(0f, -200f, 0f);
        }
        else {
            gameObject.SetActive(true);
            transform.position = pile.transform.position + new Vector3(0f, 3f, 0f);
            Vector3 targetPos = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
            transform.rotation = Quaternion.LookRotation(-targetPos + transform.position, Vector3.up);
        }
    }
}
