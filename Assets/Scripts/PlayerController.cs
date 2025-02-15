using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private AudioSource audioSource;

    private float movementX;
    private float movementY;
    public float speed = 0;

    private int count;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public AudioClip collectSound;
    public AudioClip deathSound;
    public AudioClip winSound;
    public AudioClip hitWallSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GameObject.FindWithTag("Non-diegetic Audio").GetComponent<AudioSource>();

        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue) {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText() {
        countText.text = "Count: " + count.ToString();

        if (count >= 12) {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            audioSource.PlayOneShot(winSound);
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);

            count = count + 1;
            SetCountText();
            audioSource.PlayOneShot(collectSound);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            Destroy(gameObject);
            audioSource.PlayOneShot(deathSound);

            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You lose!";
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            audioSource.PlayOneShot(hitWallSound);
        }
    }
}