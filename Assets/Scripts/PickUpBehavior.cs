using UnityEngine;

public class PickUpBehavior : MonoBehaviour {
    private MeshFilter filter;
    private MeshRenderer render;
    private GameObject gameManager;
    private int num;

    public float size;
    public bool isRandomized;
    public Mesh[] meshes;
    public Material[] materials;

    void Start() {
        filter = GetComponent<MeshFilter>();
        render = GetComponent<MeshRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        gameManager.GetComponent<GameManager>().GetCount();

        num = Random.Range(0, materials.Length);
    }

    // Update is called once per frame
    void Update() {
        //transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        if (isRandomized) {
            filter.mesh = meshes[num];
            render.material = materials[num];
        }
    }
}