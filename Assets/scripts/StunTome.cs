using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
  [RequireComponent (typeof (AudioManager), typeof (TomeManager))]
	public class StunTome : Tome {
		public GameObject[] stunBeam;
		private AudioSource audioSource;

		void Start() {
			stunBeam = GameObject.FindGameObjectsWithTag("StunBeam");
			stunBeam[0].SetActive(false);
			audioSource = GetComponent<AudioSource>();
		}

		public override void use(bool inUse) {
			stunBeam[0].SetActive(inUse);
		}

		public override void playSound(bool playCan) {
			;
		}
	}
}
