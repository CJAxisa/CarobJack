/// <summary>
/// This is the base animator and hit/hurtbox handler for anything you want to animate.
/// Hit and hurt boxes are NOT interaction boxes. I suppose you could USE them like that, but that's not what this does.
/// Do not place game logic here. This is only for generating the hit/hurt boxes and animating. It should be first in the stack.
///
/// Hit/Hurt boxes use an object pool that's initialized at -10,000, -10,000 at awake. This is in "floating point" problem territory, so you likely won't be putting
/// anything there.
///
/// Yes some things could be more efficient. I urge you however, if you're pushing for contribution, to select ease of use over hyper performance.
/// For example I chose strings over enums to make it easier to work with for newer programmers.
/// I also included the box utility classes at the bottom for easy reference.
///
///
/// Place on anything you want to animate.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class FF_Animator : MonoBehaviour {

	//Setup
	[Header("Setup")]
	public string DefaultAnimation;
	public int MaxBoxes = 10; //The number of boxes availible in the object pool. Ten is suggested unless you need more. Always allot 10% more than you think you need.
	[Tooltip("Go to, FlatFighter/Assets/Prefabs/ and drop FF_Box here.")]
	public GameObject HitHurtBoxReference; //Make sure to set this to FF_Box prefab, or your own.

	[Header("Debug")]
	public bool isDebugBoxes = true; //Debug hitboxes. (Show them.)

	//Animations
	[HideInInspector]
	public List<FF_Animation> Animations = new List<FF_Animation> ();

	//States
	FF_Animation CurrentAnimation;
	int CurrentAnimationFrameNumber;
	int NextAnimationFrameNumber;
	[HideInInspector]
	public float CurrentFrameTime;
	float NextFrameTime;
	int xFacing = 1; //1 is right -1 is left, it's an integer in case we want to use it for multiplaction of direction later, maybe make an enum?

	//Refs
	SpriteRenderer SpriteRenderRef;

	//Storage
	List<GameObject> HitHurtBoxPool = new List<GameObject>();
	List<GameObject> UsedHitHurtBoxes = new List<GameObject> ();

	/******************************************Initial************************************************/
	void Awake() {
		InitializeHitHurtBoxes ();
	}
	void Start () {
		SpriteRenderRef = this.gameObject.GetComponent<SpriteRenderer> ();
		CurrentAnimation = Animations [0];
		SetAnimation (DefaultAnimation);
	}
	void FixedUpdate () {
		AdvanceFrame ();
	}
	/**************************************************************************************************/

	/****************************************Non-Public************************************************/
	/// <summary>
	/// Utility for advancing each frame when the last one has expired.
	/// Auto handles when switching animation, will finish the last frame before switching.
	/// TODO: Put override for waiting.
	/// </summary>
	void AdvanceFrame() {
		CurrentFrameTime = Time.timeSinceLevelLoad * 1000;

		if (CurrentFrameTime >= NextFrameTime) {
			NextFrameTime = CurrentFrameTime + GetAnimationMillisecondFrameTime();
			SetSprite (NextAnimationFrameNumber);
			GenerateBoxes ();
		}

	}

	/// <summary>
	/// Called once to setup the box pool.
	/// </summary>
	void InitializeHitHurtBoxes() {
		for (int i = 0; i < MaxBoxes; i++) {
			GameObject g = Instantiate (HitHurtBoxReference, new Vector3(-10000,-10000,0), Quaternion.identity, null) as GameObject; //We store them out past floating point error.
			g.GetComponent<BoxCollider2D> ().enabled = false;
			HitHurtBoxPool.Add (g);
		}
	}

	/// <summary>
	/// Generate hit/hurt boxes each frame, pulls them from pool.
	/// </summary>
	void GenerateBoxes() {
		//Cleanup last frame's boxes.
		if (UsedHitHurtBoxes.Count > 0) {
			foreach (GameObject g in UsedHitHurtBoxes) {
				g.GetComponent<FF_Box> ().isInUse = false;
				g.GetComponent<BoxCollider2D> ().enabled = false;
				g.transform.parent = null;
				g.transform.localRotation = Quaternion.Euler (0, 0, 0);
				g.GetComponent<FF_Box> ().isHitBox = false;
				g.transform.position = new Vector3 (-10000, -10000, 0);
			}
		}
		UsedHitHurtBoxes.Clear ();
		//Add in new boxes.
		for (int i = 0; i < CurrentAnimation.animationFrames[CurrentAnimationFrameNumber].boxes.Count; i++) {
			GameObject go = null;
			foreach (GameObject g in HitHurtBoxPool) {
				if (!g.GetComponent<FF_Box> ().isInUse) {
					go = g;
					break;
				}
			}
			go.transform.parent = this.transform;
			/*go.transform.localPosition = new Vector3(CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxPositionSize.x,
				CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxPositionSize.y, 0);
			go.transform.localScale = new Vector3 (CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxPositionSize.width,
				CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxPositionSize.height, 1);*/
			go.transform.localPosition = new Vector3(CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxX,
				CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxY, 0);
			go.transform.localScale = new Vector3 (CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxWidth,
				CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxHeight, 1);
			go.transform.localRotation = Quaternion.Euler(0, 0, CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].boxRotation);
			go.GetComponent<FF_Box> ().isHitBox = CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].boxes [i].isHitBox;
			go.GetComponent<FF_Box> ().OwnerGameObjectReference = this.transform.gameObject;
			go.GetComponent<BoxCollider2D> ().enabled = true;
			go.GetComponent<FF_Box> ().Make ();
			go.GetComponent<FF_Box> ().isInUse = true;
			go.GetComponent<FF_Box> ().isDebug = isDebugBoxes;
			UsedHitHurtBoxes.Add (go);
		}
	}

	/// <summary>
	/// Checks to make sure the next animation frame number is correct before pushing it through into the sprite array.
	/// </summary>
	/// <param name="frame">Frame.</param>
	void SetSprite(int frame) {
		if (frame > CurrentAnimation.animationFrames.Count - 1) {
			NextAnimationFrameNumber = 0;
		}
		if (frame < 0) {
			NextAnimationFrameNumber = 0;
		}
		CurrentAnimationFrameNumber = NextAnimationFrameNumber;
		SpriteRenderRef.sprite = CurrentAnimation.animationFrames [CurrentAnimationFrameNumber].frameSprite;
		NextAnimationFrameNumber = NextAnimationFrameNumber + 1;
	}
	/**************************************************************************************************/

	/******************************************Public**************************************************/
	/// <summary>
	/// Use the string "right" or "left" to make the character face right or left.
	/// </summary>
	/// <param name="xface">Xface.</param>
	public void SetFacing(string xface) {
		if (xface.ToLower().Trim().Replace(" ", "") == "right") {
			xFacing = 1;
			this.transform.localRotation = Quaternion.Euler (0, 0, 0);
		} else if(xface.ToLower().Trim().Replace(" ", "") == "left") {
			xFacing = -1;
			this.transform.localRotation = Quaternion.Euler (0, 180, 0);
		}
	}

	/// <summary>
	/// Returns the string value of which way the character is facing.
	/// </summary>
	/// <returns>The facing.</returns>
	public string GetFacing() {
		if (xFacing == 1) {
			return "right";
		} else if(xFacing == -1) {
			return "left";
		}
		return "ERROR";
	}

	/// <summary>
	/// Sets the next animation frame to the stack.
	/// Pass override for wait here.
	/// </summary>
	/// <param name="frame">Frame.</param>
	public void SetFrame(int frame) {
		NextAnimationFrameNumber = frame;
	}

	/// <summary>
	/// Returns the current frame.
	/// </summary>
	/// <returns>The frame.</returns>
	public int GetFrame() {
		return CurrentAnimationFrameNumber;
	}

	public int GetFrameCountThisAnimation() {
		return CurrentAnimation.animationFrames.Count;
	}

	/// <summary>
	/// Sets the desired animation by string name. It does some parsing on the string to account for case and typos.
	/// Animations are better as strings for ease of use. Avoid enums here unless there's a good reason.
	/// </summary>
	/// <param name="animationName">Animation name.</param>
	/// <param name="startFrame">Integer Start Frame.</param>
	public void SetAnimation(string animationName, bool waitForEndOfFrame = true, int startFrame = 0) {
		if (CurrentAnimation.animationName.ToLower().Trim().Replace(" ", "") == animationName.ToLower().Trim().Replace(" ", "")) {
			return; //Animation is already set to this.
		}

		if (!waitForEndOfFrame) { //we want the animation to start fast
			NextFrameTime = CurrentFrameTime;
		}

		foreach (FF_Animation anim in Animations) {
			if(animationName.ToLower().Trim().Replace(" ", "") == anim.animationName.ToLower().Trim().Replace(" ", "")) { //account for mistypes
				CurrentAnimation = anim;
				NextAnimationFrameNumber = startFrame;
				return;
			}
		}
	}

	/// <summary>
	/// Returns the current animation string.
	/// </summary>
	/// <returns>The animation string name.</returns>
	public string GetAnimation() {
		return CurrentAnimation.animationName.ToLower().Trim();
	}

	public float GetAnimationMillisecondFrameTime() {
    if(CurrentAnimation.framesPerSecond <= 0) {
      Debug.LogError("Cannot set Frames Per Second to 0. Automatically setting FPS to 1.");
      return ((1/ (float)1) * 1000);
    }
		return ((1 / (float)CurrentAnimation.framesPerSecond) * 1000);
	}
	/**************************************************************************************************/






}
//end of main class






/// <summary>
/// Start classes.
/// Yeah, yeah, I should probably have these seperated for tidyness.
/// </summary>

[System.Serializable] //This is the master array of animations, their sprites, hit/hurtboxes, and other information.
public class FF_Animation {
	public string animationName;
	public int framesPerSecond;
	public List<FF_AnimationFrame> animationFrames = new List<FF_AnimationFrame> ();
}

[System.Serializable] //These is each individual animation frame and its information.
public class FF_AnimationFrame {
	public Sprite frameSprite;
	public List<FF_BoxData> boxes = new List<FF_BoxData>(); //First vectors are position, second vectors are scale.
}

[System.Serializable]
public class FF_BoxData {
	//public Rect boxPositionSize;
	public float boxX;
	public float boxY;
	public float boxWidth;
	public float boxHeight;
	public float boxRotation;
	public bool isHitBox;
}
