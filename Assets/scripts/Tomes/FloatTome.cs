using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
  public class FloatTome : Tome {
    private float timer = 0.0f;
    private bool toggleTimer = false;
    private bool activateFloat = false;
    public float floatDuration; // set how long you can float (-1 = infinite float)

    void Update() {
      ManageTimer();
    }

    public void ManageTimer() {
      if(toggleTimer) {
        timer += 1.0f * Time.deltaTime;
      }
      if(floatDuration > 0f) {
        if(timer > floatDuration || !activateFloat) {
          activateFloat = false;
          player.isFloating = false;
          if(audioSource != null) {
            audioSource.Stop();
          }
        }
      }
      if(player.isGrounded) {
        timer = 0f;
        toggleTimer = false;
      }
      //Debug.Log("Timer = " + timer);
    }

    public override void use(bool toggleUse) {
      activateFloat = toggleUse; // this will be set to true/false in the TomeManager class (i.e. true if left mouseclick is down, false otherwise)
      if(toggleUse && !player.isGrounded) {
        /* Activate float */
        toggleTimer = true;
        if(timer < floatDuration) {
          player.velocity.y = 0f;
          player.isFloating = true;
          Debug.Log("You are floating");
        }
      }
      else {
        player.isFloating = false;
      }
    }

    public override void playSound(bool toggleSound) {
      // if(toggleSound && activateFloat && audioManager.audioManager.floatSound != null) {
      if(toggleSound && !player.isGrounded && audioManager.floatSound != null && audioSource != null) {
        audioSource.PlayOneShot(audioManager.floatSound, 0.4f);
      }
      else if(!toggleSound && audioSource != null) {
        audioSource.Stop();
      }
    }

    public override void interaction(ref GameObject otherGameObject) {
      Debug.Log("No interaction");
    }
  }
}
