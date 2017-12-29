using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public int health;
	public float timer;
	public float delay;

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
			timer += 1.0f * Time.deltaTime;
		}
		if(timer > delay) {
			CollisionDetector.isStunned = false;
			timer = 0.0f;
		}
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //collision.gameObject.GetComponent<Player>().getHit();
				/* To do: need collision detection against flames + stunbeam
				*  Tags: "Flame" "StunBeam"
				*/
				Debug.Log("Collided with a trigger");
				if(collision.CompareTag("Flame")) {
					// takes damage every second?
					health -= 1;
					Debug.Log("Health = " + health);
				}
				if(collision.CompareTag("StunBeam")) {
					CollisionDetector.isStunned = true;
					//yield return new WaitForSeconds(5);
					//CollisionDetector.isStunned = false;
				}
                else
                {
                    /* Idea: We could add a timer so that the enemy is stunned for a short period after coming into contact with stun beam */
                    CollisionDetector.isStunned = false;
                }
                if (collision.CompareTag("Player"))
                    collision.gameObject.GetComponent<Player>().getHit();
               

    }
}
