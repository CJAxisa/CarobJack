using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
	/** 	Objective
	 *		- Get the enemy moving forward and backward
	 *		- When the enemy collides with a "Wall" its direction is reversed
	 */

	public float walkSpeed;
	public Vector2 moveVector;
	public bool isGrounded;

	void Start() {
		walkSpeed = 6.0f;
		moveVector = new Vector2(6.0f, 0f);
		isGrounded = true;
	}

	void Update() {
		if(moveVector.x >= 0) {
			// sprite change (facing right)
		}
		else {
			// ''''''''''' face left
		}
		if(!isGrounded) {
			transform.Translate(moveVector.x * Time.deltaTime, -1.0f * Time.deltaTime, 0f);
		}
		transform.Translate(moveVector.x * Time.deltaTime, 0f, 0f);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Wall")) {
			// reverse the direction of the enemy
			Debug.Log("Hit wall");
			moveVector = new Vector2(-1*moveVector.x, 0f);
		}
		/*			 isGrounded = true;
		 }
		 else {
			 isGrounded = false;
		 } */
	}
}