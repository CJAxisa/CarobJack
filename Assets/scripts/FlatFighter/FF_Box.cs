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
  // public string parentTag;
	public bool isInUse = false;
  //public string parentTag = OwnerGameObjectReference.Tag();
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
  
  // The way this works is that it gets the TomeManager script to access the currentTome and calls the function interaction using currentTome to deal with the logistics of the collision between the spell and the enemy
  void OnTriggerEnter2D(Collider2D collider) {
    // This will tell us if we have collided with a hurtbox
    if(collider.gameObject != null && collider.gameObject.GetComponent<FF_Box>().OwnerGameObjectReference != null) {
      if(isInUse && isHitBox && collider.gameObject.GetComponent<FF_Box>().isHitBox == false) {
        // This will get the hurtboxes parent tag
        if(collider.gameObject.GetComponent<FF_Box>().OwnerGameObjectReference.CompareTag("Enemy")) {
          // Here we get the currentTome object and pass in a reference to the enemy that we have collided with
          Debug.Log("Collided with enemy " + collider.gameObject.GetComponent<FF_Box>().OwnerGameObjectReference.name);
          if(gameObject.GetComponent<TomeManager>() != null) {
            gameObject.GetComponent<TomeManager>().currentTome.interaction(ref collider.gameObject.GetComponent<FF_Box>().OwnerGameObjectReference);
          }
        }
      }
    }
  }
}
