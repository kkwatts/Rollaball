using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    public bool hover;
    public float hoverFactor;
    public float hoverSpeed;
    public GameObject[] camPositions;

    private float num;
    private GameObject targetPosition;

    void Start() {
        num = 0;
        if (gameObject.name == "Raccoon") {
            transform.GetChild(0).GetComponent<Animator>().speed = 0f;
        }
        else if (gameObject.CompareTag("MainCamera")) {
            transform.position = camPositions[0].transform.position;
            transform.rotation = camPositions[0].transform.rotation;
            targetPosition = camPositions[0];
        }
    }

    void Update() {
        if (hover) {
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(num) * hoverFactor, transform.position.z);
            num += hoverSpeed;
        }
        else if (gameObject.CompareTag("MainCamera")) {
            transform.position = Vector3.Lerp(transform.position, targetPosition.transform.position, 0.01f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetPosition.transform.rotation, 0.01f);
        }
    }

    public void GoToMain() {
        Camera.main.gameObject.GetComponent<MenuScript>().SwitchCamPosition("Main");
    }

    public void GoToSettings() {
        Camera.main.gameObject.GetComponent<MenuScript>().SwitchCamPosition("Settings");
    }

    public void GoToLevelSelect() {
        Camera.main.gameObject.GetComponent<MenuScript>().SwitchCamPosition("Level Select");
    }

    public void GoToInstructions() {

    }

    public void GoToLevel1() {
        SceneManager.LoadScene("Level1");
    }

    public void GoToLevel2() { 
        
    }

    public void GoToLevel3() { 
        
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SwitchCamPosition(string position) {
        if (position.Equals("Main")) {
            targetPosition = camPositions[0];
        }
        else if (position.Equals("Settings")) {
            targetPosition = camPositions[1];
        }
        else if (position.Equals("Level Select")) {
            targetPosition = camPositions[2];
        }
    }
}
