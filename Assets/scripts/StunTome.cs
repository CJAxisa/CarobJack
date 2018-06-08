using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Tomes))]
namespace Tomes {
	public class StunTome : Tome {
		public GameObject[] stunBeam;
		public AudioClip stun;
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
