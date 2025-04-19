using UnityEngine;

public class PileBehavior : MonoBehaviour {
    private float radius;
    private Rigidbody rb;

    public float startSize;
    public GameObject gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        radius = startSize;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        transform.localScale = new Vector3(radius, radius, radius);
        rb.mass = radius;
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Collectable") && col.gameObject.GetComponent<Rigidbody>().mass <= rb.mass) {
            col.gameObject.GetComponent<Collider>().enabled = false;
            col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            col.transform.parent = transform;
            col.transform.localPosition = GetPosition();
            radius += col.gameObject.GetComponent<PickUpBehavior>().size;
            gameManager.GetComponent<GameManager>().DisplayCount();
        }
    }

    private Vector3 GetPosition() {
        int num1 = Random.Range(0, 3), num2 = Random.Range(0, 2);
        float x = 0, y = 0, z = 0;

        if (num1 == 0) {
            x = radius / 2f;
            if (num2 == 0) {
                y = Random.Range(0f, radius / 2f);
                x -= (x - y);
                z = Random.Range(0f, radius / 2f);
                y -= (y - z);
            }
            else {
                z = Random.Range(0f, radius / 2f);
                x -= (x - z);
                y = Random.Range(0f, radius / 2f);
                z -= (z - y);
            }
        }
        else if (num1 == 1) {
            y = radius / 2f;
            if (num2 == 0) {
                x = Random.Range(0f, radius / 2f);
                y -= (y - x);
                z = Random.Range(0f, radius / 2f);
                x -= (x - z);
            }
            else {
                z = Random.Range(0f, radius / 2f);
                y -= (y - z);
                x = Random.Range(0f, radius / 2f);
                z -= (z - x);
            }
        }
        else {
            z = radius / 2f;
            if (num2 == 0) {
                x = Random.Range(0f, radius / 2f);
                z -= (z - x);
                y = Random.Range(0f, radius / 2f);
                x -= (x - y);
            }
            else {
                y = Random.Range(0f, radius / 2f);
                z -= (z - y);
                x = Random.Range(0f, radius / 2f);
                y -= (y - x);
            }
        }

        if (Random.Range(0, 1) == 0) {
            x = -x;
        }
        if (Random.Range(0, 1) == 0) {
            y = -y;
        }
        if (Random.Range(0, 1) == 0) {
            z = -z;
        }

        return new Vector3(x, y, z);
    }
}
