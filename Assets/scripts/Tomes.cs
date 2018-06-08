using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
  public abstract class Tome : MonoBehaviour {
    public bool isActive = false;
    public string tomeName;
		public abstract void use(bool inUse);
		public abstract void playSound(bool playCan);
		//public void toggle() {
		//	isActive = !isActive;
		//}
	}
}
