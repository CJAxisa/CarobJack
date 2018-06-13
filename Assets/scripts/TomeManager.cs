using System.Collections;
using System.Collections.Generic;
using Tomes;
using UnityEngine;

[RequireComponent (typeof (FireTome), typeof (FloatTome), typeof (StunTome))]
[RequireComponent (typeof (AudioManager))]
public class TomeManager : MonoBehaviour {
  private List <Tome> inventory;
  private Tome currentTome;
  private int tomeIndex;
  private float timer = 0f;
  private bool playSound = true;
  private AudioSource audioSource;

  void Start() {
    inventory = new List<Tome>();
    tomeIndex = 0;
  }

  void Update() {
    SwitchTome();
    UseTome();
  }

  void SwitchTome() {
    if(Input.GetKeyDown("q") || Input.GetAxis("Mouse ScrollWheel") < 0) {
      if(inventory.Count > 1) {
        currentTome.use(false);
        if(tomeIndex == 0) {
          tomeIndex = inventory.Count - 1;
          if(AudioManager.switchTome != null) {
            audioSource.PlayOneShot(AudioManager.switchTome, 0.4f);
          }
        }
        else {
          tomeIndex -= 1;
          if(AudioManager.switchTome != null) {
            audioSource.PlayOneShot(AudioManager.switchTome, 0.4f);
          }
        }
        currentTome = inventory[tomeIndex];
      }
    }
    if(Input.GetKeyDown("e") || Input.GetAxis("Mouse ScrollWheel") > 0) {
      if(inventory.Count > 1) {
        currentTome.use(false);
        if(tomeIndex == inventory.Count - 1) {
          tomeIndex = 0;
          if(AudioManager.switchTome != null) {
            audioSource.PlayOneShot(AudioManager.switchTome, 0.4f);
          }
        }
        else {
          tomeIndex += 1;
          if(AudioManager.switchTome != null) {
            audioSource.PlayOneShot(AudioManager.switchTome, 0.4f);
          }
        }
        currentTome = inventory[tomeIndex];
      }
    }
  }

  void UseTome() {
	  if(inventory.Count > 0 || currentTome != null) {
	    if(Input.GetMouseButtonDown(0) || Input.GetKeyDown("l")) {
        print("floating");
		    currentTome.use(true);
			  currentTome.playSound(true);
	    }
	    if(Input.GetMouseButtonUp(0) || Input.GetKeyUp("l")) {
		    currentTome.use(false);
			  currentTome.playSound(false);
		  }
    }
    else {
      if(Input.GetMouseButtonDown(0)) {
			  if(playSound) {
				  audioSource.PlayOneShot(AudioManager.cannotUse, 0.4f);
					playSound = false;
				}
			}
			if(!playSound) {
			  timer += 1.0f * Time.deltaTime;
				if(timer > AudioManager.cannotUseSoundDelay) {
				  playSound = true;
					timer = 0f;
				}
			}
    }
  }

  void AddTome(Tome newTome) {
    if(inventory.Count > 0) {
      currentTome.use(false);
    }
    inventory.Add(newTome);
    currentTome = newTome; //<----- This makes your new tome the newly acquired tome
    tomeIndex = inventory.Count - 1;
    print("Added tome");
  }

  void OnTriggerEnter2D(Collider2D other) {
		// This first check is here so that way I can just destroy the other.gameObject with one line of code at the bottom of this block o code
	  if(other.CompareTag("Tome")) {
		  /* We have collided with a tome object so lets add the tome based on the game objects name */
			Debug.Log("Collided with the: " + other.name);
			if(other.name == "FireTome") {
				AddTome(gameObject.GetComponent<FireTome>());
			}
			else if(other.name == "StunTome") {
				AddTome(gameObject.GetComponent<StunTome>());
			}
			else if(other.name == "FloatTome") {
				AddTome(gameObject.GetComponent<FloatTome>());
			}
			/* Combat Tomes:
			else if(other.name == "LazerTome") {
				AddTome(gameObject.GetComponent<LazerTome>());
			}
			else if(other.name == "PunchTome") {
				AddTome(gameObject.GetComponent<PunchTome>());
			}
			else if(other.name == "IceTome") {
				AddTome(gameObject.GetComponent<IceTome>());
			}
			else if(other.name == "ShieldTome") {
				AddTome(gameObject.GetComponent<ShieldTome>());
			}
			else if(other.name == "SuplexTome") {
				AddTome(gameObject.GetComponent<SuplexTome>());
			}
			else if(other.name == "JumpAttackTome") {
				AddTome(gameObject.GetComponent<JumpAttackTome>());
			}
			***Non-Combat Tomes:
			else if(other.name == "StealthTome") {
				AddTome(gameObject.GetComponent<StealthTome>());
			}
			else if(other.name == "FlyTome") {
				AddTome(gameObject.GetComponent<FlyTome>());
			}
			else if(other.name == "BargainingTome") {
				AddTome(gameObject.GetComponent<BargainingTome>());
			}
			else if(other.name == "SpeedBoostTome") {
				AddTome(gameObject.GetComponent<SpeedBoostTome>());
			}
			else if(other.name == "IntimidationTome") {
				AddTome(gameObject.GetComponent<IntimidationTome>());
			}
			else if(other.name == "DisguiseTome") {
				AddTome(gameObject.GetComponent<DisguiseTome>());
			}
			else if(other.name == "HealTome") {
				AddTome(gameObject.GetComponent<HealTome>());
			}
			else if(other.name == "GoofyTome") {
				AddTome(gameObject.GetComponent<GoofyTome>());
			}
			else if(other.name == "InvestigationTome") {
				AddTome(gameObject.GetComponent<InvestigationTome>());
			}
			else if(other.name == "TallTome") {
				AddTome(gameObject.GetComponent<TallTome>());
			}
			else if(other.name == "TinyTome") {
				AddTome(gameObject.GetComponent<TinyTome>());
			}
			else if(other.name == "TimeTome") {
				AddTome(gameObject.GetComponent<TimeTome>());
			}
			***Summoning Tomes:
			else if(other.name == "DeadEnemiesTome") {
				AddTome(gameObject.GetComponent<DeadEnemiesTome>());
			}
			else if(other.name == "GodTome") {
				AddTome(gameObject.GetComponent<GodTome>());
			}
			*/
			Destroy(other.gameObject);
      if(AudioManager.collectedTome != null) {
			     audioSource.PlayOneShot(AudioManager.collectedTome, 0.4f);
      }
		}
	}
}
