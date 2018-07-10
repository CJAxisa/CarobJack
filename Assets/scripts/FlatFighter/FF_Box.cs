/// <summary>
/// This is your hurt/hit box. It goes on the box prefab.
/// You shouldn't need to mess with this too much.
///
/// Place this on the hit/hurt box prefab.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class FF_Box : MonoBehaviour {
	public bool isHitBox = false;
	public GameObject OwnerGameObjectReference; //What game object owns this hit/hurt box?
	public bool isInUse = false;
	public bool isDebug = true;

	public void Make() {
		if (isDebug) {
			if (isHitBox) {
				this.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 0.5f);
			} else {
				this.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0, 0.5f);
			}
		} else {
			this.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		}
	}

  // The way this works is that we get the 'TomeManager' script to access the 'currentTome' variable to call the function 'interaction()'
  // The 'interaction' function deals with the logistics of the collision between the spell and the enemy (see each tome script for more info)
  void OnTriggerEnter2D(Collider2D collider) {
    if(collider.gameObject != null && collider.gameObject.GetComponent<FF_Box>().OwnerGameObjectReference != null) {
      if(isInUse && isHitBox && collider.gameObject.GetComponent<FF_Box>().isHitBox == false) {
        if(collider.gameObject.GetComponent<FF_Box>().OwnerGameObjectReference.CompareTag("Enemy")) {
          Debug.Log("Collided with enemy gameobject: " + collider.gameObject.GetComponent<FF_Box>().OwnerGameObjectReference.name);
          if(gameObject.GetComponent<TomeManager>() != null) {
            gameObject.GetComponent<TomeManager>().currentTome.interaction(ref collider.gameObject.GetComponent<FF_Box>().OwnerGameObjectReference);
          }
        }
      }
    }
  }
}
