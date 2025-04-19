using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb, objectRB;
    private AudioSource audioSource;
    private Animator anim;
    private GameObject character;
    private GameObject camRotation;
    private GameObject grabbedObject;
    private LayerMask pickUpLayer;

    private Vector3 movement;
    private Vector2 movementAmount;
    private Vector3 lastMovement;
    private int count;
    private bool isPile;

    private KeyCode grab;
    private KeyCode release;
    private KeyCode push;

    public float speed = 0;
    public float pushForce;

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
        grabbedObject = null;

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
        Vector3 rayPos = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        Debug.DrawRay(rayPos, character.transform.forward * -2f, Color.red);

        if (grabbedObject != null) {
            if (isPile) {
                objectRB = grabbedObject.GetComponent<Rigidbody>();
                objectRB.linearVelocity = new Vector3(0f, objectRB.linearVelocity.y, 0f);
                Vector3 offset = new Vector3(transform.position.x + character.transform.forward.x * -2f, objectRB.position.y, transform.position.z + character.transform.forward.z * -2f);
                objectRB.position = offset;
                objectRB.AddForce(movement * speed);
            }
            else if (grabbedObject.GetComponent<BoxCollider>().enabled) {
                objectRB = grabbedObject.GetComponent<Rigidbody>();
                grabbedObject.GetComponent<BoxCollider>().enabled = false;
                objectRB.isKinematic = true;
            }
            else {
                Vector3 offset = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                objectRB.position = offset;
                objectRB.rotation = Quaternion.Euler(Vector3.zero);
                objectRB.linearVelocity = Vector3.zero;
            }

            if (Input.GetKeyDown(release)) {
                if (!isPile) {
                    grabbedObject.GetComponent<BoxCollider>().enabled = true;
                    objectRB.isKinematic = false;
                }
                grabbedObject = null;
            }
            else if (Input.GetKeyDown(push)) {
                if (!isPile) {
                    grabbedObject.GetComponent<BoxCollider>().enabled = true;
                    objectRB.isKinematic = false;
                }
                objectRB.AddForce(lastMovement * pushForce);
                grabbedObject = null;
            }

            if (isPile && Mathf.Abs(transform.position.y - objectRB.position.y) > 2f) {
                grabbedObject = null;
            }
        }

        if (Input.GetKeyDown(grab) && objectRB == null) {
            if (Physics.Raycast(rayPos, character.transform.forward * -1f, out RaycastHit hit, 2f, pickUpLayer)) {
                if (hit.transform.gameObject.CompareTag("Collectable") && hit.transform.gameObject.GetComponent<Rigidbody>().mass <= rb.mass) {
                    grabbedObject = hit.transform.gameObject;
                    isPile = false;
                }
                else if (hit.transform.gameObject.CompareTag("Pile")) {
                    grabbedObject = hit.transform.gameObject;
                    isPile = true;
                }
            }
        }

        if (grabbedObject == null) {
            objectRB = null;
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        movement = camRotation.transform.forward * movementAmount.y + camRotation.transform.right * movementAmount.x;

        if (movement != Vector3.zero) {
            lastMovement = movement;
        }

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