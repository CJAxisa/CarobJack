using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
	public class FloatTome : Tome {
		private float timer;
		private bool activateFloat;
		private bool canFloat;
		private AudioSource audioSource;

    public AudioClip floatSound;
    public float delay; // set how long you can float (-1 = infinite float)

		void Start() {
			timer = 0.0f;
			delay = 1.0f;
			activateFloat = false;
			canFloat = true;
			audioSource = GetComponent<AudioSource>();
		}

		void Update() {
			if(activateFloat && timer < delay) {
				/* Activate float */
				Player.velocity.y = 0f; // here we are actually changing the characters vertical velocity to zero to simulate floating
				Player.isFloating = true; // necessary so that the Mover class will not apply gravity to the player when he is not grounded
				timer += 1.0f * Time.deltaTime;
				Debug.Log("You are floating");
			}
			if(delay > 0f) {
				if(timer > delay || !activateFloat) {
					activateFloat = false;
					//Mover.isFloating = false;
					//timer = 0f;
					//Debug.Log("You stopped floating");
				}
				if(timer > delay) {
					audioSource.Stop();
				}
			}
			if(Player.isGrounded) {
				timer = 0f;
			}
		}

		public override void use(bool inUse) {
			activateFloat = inUse; // this will be set to true/false in the TomeManager class (i.e. true if left mouseclick is down, false otherwise)
		}

		public override void playSound(bool playCan) {
			if(playCan && activateFloat) {
				audioSource.PlayOneShot(floatSound, 0.4f);
			}
			else if(playCan == false) {
				audioSource.Stop();
			}
		}
	}
}
