using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
	public class StunTome : Tome {
		public GameObject[] stunBeam;

		void Start() {
			stunBeam = GameObject.FindGameObjectsWithTag("StunBeam");
			stunBeam[0].SetActive(false);
		}

		public override void use(bool inUse) {
			stunBeam[0].SetActive(inUse);
		}
	}
}
