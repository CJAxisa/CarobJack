using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
	public abstract class Tome : MonoBehaviour {
		public string tomeName; // unity was yelling at me for using the variable name "name" so I switched it to tomeName
		public bool isActive = false;
		/* private bool isActive*/

		public abstract void use();

		public void toggle() {
			isActive = !isActive;
		}
	}
}
