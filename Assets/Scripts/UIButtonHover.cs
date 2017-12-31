using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu {
	public class UIButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
		private Text t;

		void Start() {
			t = GetComponentInChildren<Text>();
		}

		void Update() {
			if(UIManager.needsReset) {
				resetText();
				UIManager.needsReset = false;
			}
		}

		public void OnPointerEnter(PointerEventData eventData) {
			t.text = "> " + t.text;
		}

		public void OnPointerExit(PointerEventData eventData) {
			resetText();
		}

		public void resetText() {
			if(t.text.Contains(">")) {
				t.text = t.text.Substring(2);
			}
		}
	}
}
