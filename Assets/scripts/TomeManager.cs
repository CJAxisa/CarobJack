using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tomes;

[RequireComponent (typeof (Tomes))]
public class TomeManager : MonoBehavior {
  private List <Tome> inventory;
  private Tome current;
  private int tomeIndex;

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
			//audioSource.PlayOneShot(collectedTome, 0.4f);
		}
	}
}
