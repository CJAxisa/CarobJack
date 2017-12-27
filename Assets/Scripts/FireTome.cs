using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
	public class FireTome : Tome {
		public GameObject[] flames;
		bool inUse;
		bool toggle;

		public FireTome() {
			tomeName = "fire";
		}

		void Start() {
			flames = GameObject.FindGameObjectsWithTag("Flame");

			flames[0].SetActive(false);
			flames[1].SetActive(false);
			flames[2].SetActive(false);

			//inUse = false;
		}

		void Update() {
			if(Input.GetMouseButtonDown(0)) {
				inUse = true;
				use();
			}
			if(Input.GetMouseButtonUp(0)) {
				inUse = false;
				use();
			}
		}

		public override void use() {
			/* Set each flame object to inUse*/
			flames[0].SetActive(inUse);
			flames[1].SetActive(inUse);
			flames[2].SetActive(inUse);
		}
	}
}
