using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu {
	public class UIManager : MonoBehaviour {
		private GameObject[] pauseObjects;
		public static bool needsReset;

		// Use this for initialization
		void Start () {
			pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
			needsReset = false;
			Time.timeScale = 0;
			togglePause();
		}

		// Update is called once per frame
		void Update () {
			if(Input.GetKeyDown(KeyCode.P)) {
				togglePause();
			}
		}

		public void togglePause() {
			// pause
			if(Time.timeScale == 1) {
				needsReset = true;
				Time.timeScale = 0;
				foreach(GameObject g in pauseObjects) {
					g.SetActive(true);
				}
			}
			// unpause
			else if (Time.timeScale == 0) {
				Time.timeScale = 1;
				foreach(GameObject g in pauseObjects) {
					g.SetActive(false);
				}
			}
		}

		public void load(string level) {
			SceneManager.LoadScene(level);
		}
	}
}
