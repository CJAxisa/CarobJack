using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
  [RequireComponent (typeof (TomeManager), typeof (AudioManager))]
	public class FloatTome : Tome {
		private float timer;
    private bool toggleTimer;
		private bool activateFloat;
		private bool canFloat;
		private AudioSource audioSource;
    public float floatDuration; // set how long you can float (-1 = infinite float)

		void Start() {
			timer = 0.0f;
			floatDuration = 1.0f;
			activateFloat = false;
			canFloat = true;
			audioSource = GetComponent<AudioSource>();
      toggleTimer = false;
		}

		void Update() {
      if(toggleTimer) {
        timer += 1.0f * Time.deltaTime;
      }
      if(floatDuration > 0f) {
        if(timer > floatDuration || !activateFloat) {
          activateFloat = false;
          Player.isFloating = false;
          if(audioSource != null) {
            audioSource.Stop();
          }
        }
      }
      if(Player.isGrounded) {
        timer = 0f;
        toggleTimer = false;
      }
      Debug.Log("Timer = " + timer);
    }

		public override void use(bool inUse) {
			activateFloat = inUse; // this will be set to true/false in the TomeManager class (i.e. true if left mouseclick is down, false otherwise)
      if(inUse && !Player.isGrounded) {
        /* Activate float */
        toggleTimer = true;
        if(timer < floatDuration) {
          Player.velocity.y = 0f;
          Player.isFloating = true;
          Debug.Log("You are floating");
        }
      }
      else {
        Player.isFloating = false;
      }
		}

		public override void playSound(bool toggle) {
			// if(toggle && activateFloat && AudioManager.AudioManager.floatSound != null) {
      if(toggle && !Player.isGrounded && AudioManager.floatSound != null && audioSource != null) {
				audioSource.PlayOneShot(AudioManager.floatSound, 0.4f);
			}
			else if(!toggle && audioSource != null) {
				audioSource.Stop();
			}
		}
	}
}
