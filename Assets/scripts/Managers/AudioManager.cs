using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose of this class is to centralize the location of all audioclips & manage volume settings
// i.e. setting sound and music volume
public class AudioManager : MonoBehaviour {
  // ******************************* Audio Clips *******************************
  public AudioClip cannotUse;
  public float cannotUseSoundDelay = 1.0f;
  public AudioClip collectedTome;
  public AudioClip switchTome;
  public AudioClip floatSound;
  public AudioClip initialFlame;
  public AudioClip sustainedFlame;
  public AudioClip stun;

  // ***************************** Volume Settings *****************************
  public float soundVolume;
  public float musicVolume;
  public bool mute;
}
