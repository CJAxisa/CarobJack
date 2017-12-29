using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float health;
	public float stunTimer;
	public float burnTimer;
	public float delay;
	public bool isBurning;
	public bool dmgOverTime;

	void Start() {
		delay = 2.0f;
	}

	// Update is called once per frame
	void Update () {
		if(health <= 0) {
			Debug.Log("DEAD");
			Destroy(gameObject);
		}
		if(CollisionDetector.isStunned) {
			stunTimer += 1.0f * Time.deltaTime;
		}
		if(stunTimer > delay) {
			CollisionDetector.isStunned = false;
			stunTimer = 0.0f;
		}
		if(isBurning) {
			health -= 1 * Time.deltaTime;
		}
		if(dmgOverTime) {
			if(burnTimer > delay) {
				burnTimer = 0f;
				dmgOverTime = false;
			}
			else {
				health -= 0.1f * Time.deltaTime;
				Debug.Log("Health (DOT)= " + health);
				burnTimer += 1.0f * Time.deltaTime;
			}
		}
	}

  public void OnTriggerEnter2D(Collider2D collision)
  {
      //collision.gameObject.GetComponent<Player>().getHit();
		  /* To do: need collision detection against flames + stunbeam
			*  Tags: "Flame" "StunBeam"
			*/
			Debug.Log("Collided with a trigger");
			if(collision.CompareTag("StunBeam")) {
				CollisionDetector.isStunned = true;
				//yield return new WaitForSeconds(5);
				//CollisionDetector.isStunned = false;
			}
      else if(stunTimer > delay){
        /* Idea: We could add a stunTimer so that the enemy is stunned for a short period after coming into contact with stun beam */
        CollisionDetector.isStunned = false;
      }
      if (collision.CompareTag("Player"))
      	collision.gameObject.GetComponent<Player>().getHit();
  }

	public void OnTriggerStay2D(Collider2D collision) {
		  if(collision.CompareTag("Flame")) {
				if(dmgOverTime) {
					dmgOverTime = false; // stop the dmgOverTime
					burnTimer = 0f; // reset DOT timer
				}
				// takes damage every second?
				isBurning = true;
				Debug.Log("Health = " + health);
			}
	}

	public void OnTriggerExit2D(Collider2D collision) {
		if(collision.CompareTag("Flame")) {
			isBurning = false;
			dmgOverTime = true;
		}
	}
}
