using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeManager : MonoBehaviour {

  private Dictionary<string, bool> inventory;
  private List<string> indexMap;
  private int currentTomeIndex;
  private string currentTomeName;
  //private Tome current;
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
    inventory = new Dictionary<string, bool>();
    indexMap = new List<string>();
    currentTomeIndex = 0;
  }

  void Update() {
    //pressing q decreases currentTomeIndex, if the index is the first tome it will warp to the last index
    if(Input.GetKeyDown("q")) {
      if(indexMap.Count > 0) {
  		  if(currentTomeIndex == 0) {
  			  currentTomeIndex = indexMap.Count - 1;
  		  }
  		  else {
  			  currentTomeIndex -= 1;
  		  }
      }
	  }
    //pressing e increases currentTomeIndex, if the index is the last tome it will warp to the first index
	  if(Input.GetKeyDown("e")) {
		  if(indexMap.Count > 0) {
  		  if(currentTomeIndex == indexMap.Count - 1) {
  			  currentTomeIndex = 0;
  		  }
  		  else {
          currentTomeIndex += 1;
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

	  Debug.Log(currentTomeIndex);
  }

  void AddTome(string newTome) {
    inventory.Add(newTome, true);
    inventory[currentTomeName] = false;
    currentTomeName = newTome;
	  /* then set currentTomeName (inventory[index] to false)*/
  }

  /* We need something for selection */
	/* Here we will keep track of all the tomes the player has */
}
