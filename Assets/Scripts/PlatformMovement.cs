using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

	public float speedP;
	public float timer = 0f;
	public float delay = 5f;

	public bool temporaryLife;

	public float distance = 0f;
	public bool moveForward = true;
	public float lowerBound = 0f;
	public float upperBound = 5f;
	public Transform player;
	public Transform initialParent;

	void Start() {
		//speedP = 2.0f;
	}

	void Update() {
		if(distance < upperBound) {
			if(moveForward) {
				transform.Translate(speedP * Time.deltaTime, 0, 0);
				distance += Time.deltaTime;
			}
			else {
				transform.Translate(-speedP * Time.deltaTime, 0, 0);
				distance += Time.deltaTime;
			}
		}
		else if(distance > upperBound) {
			moveForward = !moveForward;
			distance = 0.0f;
		}
		//else {
		//	moveForward = !moveForward;
		//}
		//if(distance > lowerBound && !moveForward){
			//transform.Translate(0, 0, -speedP * Time.deltaTime);
	//	distance -= Time.deltaTime;
	//	}
		//else {
		//	moveForward = !moveForward;
	//	}
		if(temporaryLife == true) {
			timer += Time.deltaTime;
		}
		if(timer > delay) {
			player.parent = null;
			Destroy(gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if(other.gameObject.tag == "Player") {
			Debug.Log("Player on board!");
			if(timer > delay) {
				player.parent = null;
				Destroy(gameObject);
			}
			else {
				initialParent = other.transform.parent;
				other.transform.parent = this.transform;
				player = other.transform;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.tag == "Player") {
			other.transform.parent = initialParent.transform;
		}
	}
}
