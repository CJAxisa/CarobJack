using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Tomes))]
namespace Tomes {
	public class FireTome : Tome {
		public GameObject[] flames;
		bool toggle;
    //bool inUse;
		public AudioClip initialFlame;
		public AudioClip sustainedFlame;
		private AudioSource audioSource;
		/*public FireTome() {
			tomeName = "fire";
		}*/

		void Start() {
			flames = GameObject.FindGameObjectsWithTag("Flame");
      if(flames != null) {
			   flames[0].SetActive(false);
			   flames[1].SetActive(false);
			   flames[2].SetActive(false);
      }
			audioSource = GetComponent<AudioSource>();
			//inUse = false;
		}

		void Update() {
			/*if(Input.GetMouseButtonDown(0)) {
				inUse = true;
				use();
			}
			if(Input.GetMouseButtonUp(0)) {
				inUse = false;
				use();
			}*/
		}

		public override void use(bool inUse) {
			flames[0].SetActive(inUse);
			flames[1].SetActive(inUse);
			flames[2].SetActive(inUse);
		}

		public override void playSound(bool playCan) {
			;
		}
	}
}
