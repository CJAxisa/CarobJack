using System.Collections;
using UnityEngine;

namespace Tomes {
  // Tome class provides basic foundation for all types of tomes in the game
  // Cannot be abstract, sadly, because it needs to initialize three public variables
  // TODO: Make a dummy tome and add its default spell/effect here
  [RequireComponent (typeof (TomeManager), typeof (Player))]
  [RequireComponent (typeof (AudioSource), typeof (AudioManager))]
  public class Tome : MonoBehaviour {
    // Inherited fields
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public AudioManager audioManager;
    [HideInInspector]
    public Player player;

    void Awake() {
      audioSource = GetComponent<AudioSource>();
      audioManager = GetComponent<AudioManager>();
      player = GetComponent<Player>();
    }

    // param: toggleUse - tells the script that the tome is in use
    // purpose: to activate a tomes abilities
    public virtual void use(bool toggleUse) {
      return;
    }

    // param: toggleSound - conrols whether or not a sound should be played
    // purpose: plays a sound for a specific tome
    public virtual void playSound(bool toggleSound) {
      return;
    }

    // param: ref GameObject otherGameObject - holds the a reference to another object
    //purpose: to deal with the logistics of a players hitbox hitting a foreign hurtbox
    public virtual void interaction(ref GameObject otherGameObject) {
      return;
    }
	}
}
