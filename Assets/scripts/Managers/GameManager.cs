using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private bool paused;
    public GameObject tomeMenu;
	// Use this for initialization
	void Start () {
        paused = false;
        tomeMenu = GameObject.FindGameObjectWithTag("TomeSelectMenu");
        tomeMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            Time.timeScale = 0f;
            paused = true;
            tomeMenu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            Time.timeScale = 1f;
            paused = false;
            tomeMenu.SetActive(false);
        }
    }
}
