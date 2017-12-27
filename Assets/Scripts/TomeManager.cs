﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tomes;

/** This is the "inventory"
 *	This script will be attatched to the player object
 *	This script handles tome selection
 */
public class TomeManager : MonoBehaviour {
  private List<Tome> inventory;
  private int tomeIndex;

  // private Tome current;
  // keeps track of current item - index
  // general tome object = Tome tome;
  // to call functions for each tome: compare the string value of the current index

  // Tome current;
  // ********int index;
  // ********Dictionary indexMap (this guy associates int index to strings to reference inventory)
  // ********some selection has to happen. if we're using Q and E to scroll, we can have an int
  // ********that increments and decrements (index), and we can reference the name of the tome associated
  // ********to that int using the dictionary indexMap
  // ********otherwise we dont need index or indexMap if we use the pausing system bc we can associate
  // ********the tomes themselves in the pause menu to strings (name)
  // name is a String like "fire"
  // inventory["fire"] is the bool associated to the key "fire" in inventory
  // (if inventory[name]) { current = new FireTome; }


  void Start() {
    inventory = new List<Tome>();
    tomeIndex = 0;
  }

  void Update() {
    //pressing q decreases tomeIndex, if the index is the first tome it will warp to the last index
    if(Input.GetKeyDown("q")) {
      if(inventory.Count > 0) {
  		  if(tomeIndex == 0) {
  			  tomeIndex = inventory.Count - 1;
  		  }
  		  else {
  			  tomeIndex -= 1;
  		  }
      }
	  }
    //pressing e increases tomeIndex, if the index is the last tome it will warp to the first index
	  if(Input.GetKeyDown("e")) {
		  if(inventory.Count > 0) {
  		  if(tomeIndex == inventory.Count - 1) {
  			  tomeIndex = 0;
  		  }
  		  else {
          tomeIndex += 1;
  		  }
      }
	  }
	 //to use tome
	 /* Could use mouse click */
   if(Input.GetMouseButtonDown(0)) {
     // tome.use();
     // snapping for fire tome will go into that method, NOT here
     // this is bc each tome has different behaviour - for example jump tome
     // does not require aiming. no reason to execute that code when it's not needed
     Debug.Log("Used Tome!");
   }

	  Debug.Log(tomeIndex);
  }

  void AddTome(Tome newTome) {
    inventory.Add(newTome);
    //currentTomeName = newTome;
	  /* then set currentTomeName (inventory[index] to false)*/
  }
  /* We need something for selection */
	/* Here we will keep track of all the tomes the player has */
}
