using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb, objectRB;
    private AudioSource audioSource;
    private Animator anim;
    private GameObject character;
    private GameObject camRotation;
    private LayerMask pickUpLayer;

    private Vector3 movement;
    private Vector2 movementAmount;
    private Vector3 lastMovement;
    private bool isPile;
    private Vector3 targetPos;
    [SerializeField] private bool isMoving = false;

    private KeyCode grab;
    private KeyCode release;
    private KeyCode push;

    public float speed = 0;
    public float pushForce;

    public GameObject grabbedObject;
    public GameObject playerCam;
    public GameObject gameManager;
    public AudioClip[] sounds;
    public GameObject[] VFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        VFX[0].SetActive(false);

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

        movement = Vector3.zero;
        movementAmount = Vector2.zero;
    }

    void OnMove(InputValue movementValue) {
        movementAmount = movementValue.Get<Vector2>();
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
                if (objectRB.mass >= 2f) {
                    //offset += new Vector3(objectRB.mass / 4f, 0f, objectRB.mass / 4f);
                }
                objectRB.position = offset;
                objectRB.AddForce(movement * speed);
            }
            else if (grabbedObject.GetComponent<Collider>().enabled) {
                objectRB = grabbedObject.GetComponent<Rigidbody>();
                grabbedObject.GetComponent<Collider>().enabled = false;
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
                    grabbedObject.GetComponent<Collider>().enabled = true;
                    objectRB.isKinematic = false;
                }
                grabbedObject = null;
            }
            else if (Input.GetKeyDown(push)) {
                if (!isPile) {
                    grabbedObject.GetComponent<Collider>().enabled = true;
                    objectRB.isKinematic = false;
                }
                objectRB.AddForce(lastMovement * (pushForce * objectRB.mass));
                grabbedObject = null;
            }

            /*if (isPile && Mathf.Abs(transform.position.y - objectRB.position.y) > 2f) {
                grabbedObject = null;
            }*/
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

        /*if (Input.GetMouseButton(0)) {
            Ray ray = playerCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                    targetPos = hit.point;
                    isMoving = true;
                }
            }
        }
        else {
            isMoving = false;
        }*/
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

        if (isMoving && movement == Vector3.zero) {
            Vector3 direction = targetPos - rb.position;
            direction.Normalize();
            rb.AddForce(direction * speed);
        }

        if (Vector3.Distance(rb.position, targetPos) < 0.5f) {
            isMoving = false;
        }

        rb.AddForce(movement * speed);

        if (Mathf.Abs(rb.linearVelocity.x) >= 0.1f || Mathf.Abs(rb.linearVelocity.z) >= 0.1f) {
            VFX[0].SetActive(true);
            anim.SetBool("Idle", false);
        }
        else {
            VFX[0].SetActive(false);
            anim.SetBool("Idle", true);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            character.SetActive(false);
            speed = 0f;

            audioSource.PlayOneShot(sounds[1]);

            var currentDeathFX = Instantiate(VFX[1], transform.position, Quaternion.identity);
            Destroy(currentDeathFX, 2);

            gameManager.GetComponent<GameManager>().LoseGame();
        }
        else if (!collision.gameObject.CompareTag("Ground")) {
            audioSource.PlayOneShot(sounds[0]);
        }
    }

    public void Win() {
        Instantiate(VFX[2], transform.position, Quaternion.identity);
    }
}