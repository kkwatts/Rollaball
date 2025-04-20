using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject itemCountText;
    public GameObject winText;
    public GameObject loseText;
    public AudioSource audioSource;
    public AudioClip winSound;
    public GameObject player;

    private int itemCount;
    private int totalItems = 0;
    private Vector3 itemCountPosition;
    private RectTransform itemRectPos;
    private Vector3 winTextPosition;
    private RectTransform winRectPos;
    private Vector3 loseTextPosition;
    private RectTransform loseRectPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        itemCount = 0;

        itemCountPosition = new Vector3(-108f, 298f, 0f);
        itemRectPos = itemCountText.GetComponent<RectTransform>();
        itemRectPos.localPosition = itemCountPosition;

        winTextPosition = new Vector3(-108f, 298f, 0f);
        winRectPos = winText.GetComponent<RectTransform>();
        winRectPos.localPosition = winTextPosition;

        loseTextPosition = new Vector3(-108f, 298f, 0f);
        loseRectPos = loseText.GetComponent<RectTransform>();
        loseRectPos.localPosition = loseTextPosition;
    }

    // Update is called once per frame
    void Update() {
        itemRectPos.localPosition = Vector3.Lerp(itemRectPos.localPosition, itemCountPosition, 0.01f);
        winRectPos.localPosition = Vector3.Lerp(winRectPos.localPosition, winTextPosition, 0.01f);
        loseRectPos.localPosition = Vector3.Lerp(loseRectPos.localPosition, loseTextPosition, 0.01f);
    }

    public void GetCount() {
        totalItems++;
    }

    public void DisplayCount() {
        itemCount++;
        if (itemCount == totalItems) {
            audioSource.PlayOneShot(winSound);
            player.GetComponent<PlayerController>().Win();
            winTextPosition = new Vector3(-108f, 92f, 0f);
            StartCoroutine(Wait(3, 2));
        }
        else {
            itemCountText.GetComponent<TextMeshProUGUI>().text = "Items collected: " + itemCount;
            itemCountPosition = new Vector3(-108f, 225f, 0f);
            StartCoroutine(Wait(3, 1));
        }
    }

    private IEnumerator Wait(int seconds, int type) {
        yield return new WaitForSeconds(seconds);
        if (type == 1) {
            itemCountPosition = new Vector3(-108f, 298f, 0f);
        }
        else if (type == 2) {
            SceneManager.LoadScene("MainMenu");
        }
        else if (type == 3) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void LoseGame() {
        if (itemCount != totalItems) {
            loseTextPosition = new Vector3(-108f, 92f, 0f);
            StartCoroutine(Wait(3, 3));
        }
    }
}
