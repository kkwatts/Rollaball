using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private AudioSource audioSource;
    private Animator anim;
    private GameObject character;
    private GameObject camRotation;
    private LayerMask pickUpLayer;

    private Vector3 movement;
    private Vector2 movementAmount;
    private int count;

    private KeyCode grab;
    private KeyCode release;
    private KeyCode push;

    public float speed = 0;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    public AudioClip collectSound;
    public AudioClip deathSound;
    public AudioClip winSound;
    public AudioClip hitWallSound;

    public GameObject pickupFX;
    public GameObject deathFX;
    public GameObject winFX;
    public GameObject trailFX;

    public GameObject mainCam;
    public GameObject playerCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        trailFX.SetActive(false);
        playerCam.SetActive(true);
        mainCam.SetActive(false);

        rb = GetComponent<Rigidbody>();
        audioSource = GameObject.FindWithTag("Non-diegetic Audio").GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        character = transform.GetChild(0).gameObject;
        camRotation = transform.GetChild(1).gameObject;
        pickUpLayer = LayerMask.GetMask("PickUp");

        grab = KeyCode.E;
        release = KeyCode.E;
        push = KeyCode.Space;

        count = 0;
        movement = Vector3.zero;
        movementAmount = Vector2.zero;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue) {
        movementAmount = movementValue.Get<Vector2>();
    }

    void SetCountText() {
        countText.text = "Count: " + count.ToString();

        if (count >= 12) {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            audioSource.PlayOneShot(winSound);
            var currentWinFX = Instantiate(winFX, transform.position, Quaternion.identity, this.transform);
            Destroy(currentWinFX, 11);
        }
    }

    // Update is called once per frame
    private void Update() {
        Vector3 rayPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        
        if (Input.GetKeyDown(grab)) {
            Debug.Log("Grab");
            if (Physics.Raycast(rayPos, character.transform.forward, out RaycastHit hit, -2f, pickUpLayer)) {
                Debug.Log("Grabbed");
                if (hit.transform.gameObject.CompareTag("Collectable")) {

                }
                else if (hit.transform.gameObject.CompareTag("Pile")) {
                    Debug.Log("Grabbed pile");
                }
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        movement = camRotation.transform.forward * movementAmount.y + camRotation.transform.right * movementAmount.x;

        if (!(movementAmount.x == 0f && movementAmount.y == 0f)) {
            Quaternion direction = Quaternion.LookRotation(new Vector3(-movement.x, 0.0f, -movement.z), Vector3.up);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, direction, 0.5f);
        }

        rb.AddForce(movement * speed);

        if (Mathf.Abs(rb.linearVelocity.x) >= 0.1f || Mathf.Abs(rb.linearVelocity.z) >= 0.1f) {
            trailFX.SetActive(true);
            anim.SetBool("Idle", false);
        }
        else {
            trailFX.SetActive(false);
            anim.SetBool("Idle", true);
        }
    }

    /*void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);

            count = count + 1;
            SetCountText();
            audioSource.PlayOneShot(collectSound);

            var currentPickupFX = Instantiate(pickupFX, other.transform.position, Quaternion.identity);
            Destroy(currentPickupFX, 2);
        }
    }*/

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            // Set the speed of the enemy's animation to 0
            collision.gameObject.GetComponentInChildren<Animator>().SetFloat("speed_f", 0);

            mainCam.SetActive(true);

            Destroy(gameObject);
            audioSource.PlayOneShot(deathSound);

            var currentDeathFX = Instantiate(deathFX, transform.position, Quaternion.identity);
            Destroy(currentDeathFX, 2);

            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You lose!";
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            audioSource.PlayOneShot(hitWallSound);
        }
    }
}