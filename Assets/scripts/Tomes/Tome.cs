using System.Collections;
using UnityEngine;

namespace Tomes {
  // Tome class provides basic foundation for all types of tomes in the game
  [RequireComponent (typeof (TomeManager), typeof (Player))]
  [RequireComponent (typeof (AudioSource), typeof (AudioManager))]
  public class Tome : MonoBehaviour {
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public AudioManager audioManager;
    [HideInInspector]
    public Player player;

    void Start() {
      audioSource = GetComponent<AudioSource>();
      audioManager = GetComponent<AudioManager>();
      player = GetComponent<Player>();
    }

		// public abstract void use(bool inUse);
		// public abstract void playSound(bool toggle);
    // public abstract void interaction(ref GameObject target);
    public virtual void use(bool toggleUse) {
      return;
    }
    public virtual void playSound(bool toggleSound) {
      return;
    }
    public virtual void interaction(ref GameObject otherGameObject) {
      return;
    }
	}
}
