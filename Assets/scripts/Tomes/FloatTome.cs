using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomes {
  public class FloatTome : Tome {
    private float timer = 0.0f;
    private bool toggleTimer = false;
    private bool activateFloat = false;

    [Range (-1, 2)] public float floatDuration; // Sets how long you can float (-1 = infinite float)
    public float fallSpeed;

    void Start() {
      enabled = activateFloat;
    }

    void Update() {
      ManageTimer();
    }

    // purpose: increments the timer & checks if player should be floating or slowly falling
    public void ManageTimer() {
      if(toggleTimer) {
        timer += 1.0f * Time.deltaTime;
      }
      if(floatDuration > 0f) {
        if(timer > floatDuration || !activateFloat) {
          player.isFloating = false;
          if(audioSource != null) {
            audioSource.Stop();
          }
        }
        if(timer > floatDuration && activateFloat) {
          player.velocity.y += fallSpeed * Time.deltaTime;
        }
      }
      if(player.isGrounded) {
        timer = 0f;
        toggleTimer = false;
        enabled = false;
      }
    }

    public override void use(bool toggleUse) {
      activateFloat = toggleUse;
      enabled = true;
      if(toggleUse && !player.isGrounded) {
        // Activate float timer
        toggleTimer = true;
        if(timer < floatDuration) {
          player.velocity.y = 0f;
          player.isFloating = true;
        }
      }
      else {
        player.isFloating = false;
      }
    }

    public override void playSound(bool toggleSound) {
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
