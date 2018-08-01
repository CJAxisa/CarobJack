using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FF_AnimationSetup : EditorWindow {

	bool isActive = false;

	int SelectedAnimation = 0;
	int SelectedFrame = 0;
	int SelectedBox = 0;
	int LastSelectedAnimation = 0;
	int LastSelectedFrame = 0;
	int LastSelectedBox = 0;

	Color DefaultGUIColor;

	GameObject SelectedObject;
	string SelectedObjectName;

	GameObject DisplayBox;

	Sprite DefaultObjectSpriteBeforeEditing;

	float LastElementY = 0f;

  Vector2 scrollPos;


	[MenuItem("Flat Fighter/Animation Setup")]
	public static void ShowWindow() {
		GetWindow<FF_AnimationSetup> ("Animations");
	}
	FF_AnimationSetup() {
		EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;
	}
	public void HandleOnPlayModeChanged(PlayModeStateChange state) {
	}




	void OnGUI() {
    EditorGUILayout.BeginVertical();
    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(300), GUILayout.MinWidth(0), GUILayout.MaxWidth(0));

		DefaultGUIColor = GUI.color;

		if (SelectedObject == null) {
			if (Selection.activeGameObject != null) {
				if (Selection.activeGameObject.GetComponent<FF_Animator> () != null) {
					SelectedObject = Selection.activeGameObject;
				}
			}
		}
		if (SelectedObject == null) {
			GUILayout.Label ("No object selected.");
			return;
		}
		if (SelectedObject.GetComponent<FF_Animator> () == null) {
			GUILayout.Label ("Object must have FF_Animator attached.");
			return;
		}

		//Generic variables.
		FF_Animator anim = SelectedObject.GetComponent<FF_Animator> ();
		LastElementY = 0f; //Used throughout to get the Y of the last element.
		SelectedObjectName = SelectedObject.name;
		DefaultObjectSpriteBeforeEditing = SelectedObject.GetComponent<SpriteRenderer> ().sprite;

		//TODO Preview Mode
		//TODO Push pop array.

		if (!isActive) {
			//Generic buttons.
			EditorGUILayout.Separator ();
			GUI.color = new Color (.7f, .9f, .7f);
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("O", GUILayout.Width (25))) {
				isActive = true;
			}
			GUILayout.Label ("     Click O to start the editor.");
			GUILayout.EndHorizontal ();
			GUI.color = DefaultGUIColor;
			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
			return;
		} else {
			//Generic buttons.
			EditorGUILayout.Separator ();
			GUI.color = new Color (.9f, .7f, .7f);
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("X", GUILayout.Width (25))) {
				Close ();
				return;
			}
			GUILayout.Label ("     Current Selected Object: " + SelectedObjectName);
			GUILayout.EndHorizontal ();
			GUI.color = DefaultGUIColor;
			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
		}



		LayoutAnimationButtons (anim);

		LayoutFrameButtons (anim);

		LayoutBoxButtons (anim);

		InitializeDisplayBox (anim);

		WriteDisplayBoxData (anim);

    EditorGUILayout.EndScrollView();
    EditorGUILayout.EndVertical();
	}



	void LayoutAnimationButtons(FF_Animator anim) {
		//Layout animation buttons.

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("+", GUILayout.Width (50))) {
			if (SelectedAnimation > anim.Animations.Count - 1) {
				SelectedAnimation = anim.Animations.Count - 1;
			}

			if (anim.Animations.Count == 0) {
				SelectedAnimation = 0;
			} else {
				SelectedAnimation = SelectedAnimation + 1;
			}

			anim.Animations.Insert (SelectedAnimation, new FF_Animation ());

			return;
		}
		if (GUILayout.Button ("-", GUILayout.Width (25))) {
			if (!ValidateAnimationList (anim, 1)) {
				return;
			}
			if (DisplayBox != null) {
				DestroyImmediate (DisplayBox);
			}

			anim.Animations.RemoveAt (SelectedAnimation);

			if (SelectedAnimation > anim.Animations.Count - 1) {
				SelectedAnimation = anim.Animations.Count - 1;
			}

			return;

		}
		if (ValidateAnimationList (anim, 1)) {
			GUILayout.Label ("Animations: " + anim.Animations.Count.ToString ());
		} else {
			GUILayout.Label ("Animations: 0");
		}
		EditorGUILayout.EndHorizontal ();

		if (!ValidateAnimationList (anim, 1)) {
			EditorGUILayout.Separator ();
			return;
		}

		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		LastElementY = GUILayoutUtility.GetLastRect ().y;
		GUILayout.BeginHorizontal ();
		GUI.color = Color.cyan;
		for (int i = 0; i < anim.Animations.Count; i++) {
			Rect AnimationScrubberBoxSize = new Rect (i*(position.width / anim.Animations.Count), LastElementY, position.width / anim.Animations.Count, 25);
			if(i == SelectedAnimation) {
				GUI.color = Color.yellow;
			}
			if (GUI.Button (AnimationScrubberBoxSize, anim.Animations [i].animationName)) {
				SelectedAnimation = i;
				GUIUtility.keyboardControl = 0;
				SelectedFrame = 0; //Always default to the first availible frame of the next animation.
			}
			GUI.color = Color.cyan;
		}
		GUI.color = DefaultGUIColor;
		GUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.BeginHorizontal ();

		GUILayout.Label ("Animation Name:");
		string tempAnimName = EditorGUILayout.TextField (anim.Animations[SelectedAnimation].animationName);
		GUILayout.Label ("Frames Per Second:");
		int tempFPS = EditorGUILayout.IntField (anim.Animations[SelectedAnimation].framesPerSecond);
		anim.Animations [SelectedAnimation].animationName = tempAnimName;
		anim.Animations [SelectedAnimation].framesPerSecond = tempFPS;

		GUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();


	}

	void LayoutFrameButtons(FF_Animator anim) {
		//Layout frame buttons.
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("+", GUILayout.Width (50))) {
			if(!ValidateAnimationList(anim,1)) {
				return;
			}

			if (SelectedFrame > anim.Animations [SelectedAnimation].animationFrames.Count - 1) {
				SelectedFrame = anim.Animations [SelectedAnimation].animationFrames.Count - 1;
			}

			if (anim.Animations [SelectedAnimation].animationFrames.Count == 0) {
				SelectedFrame = 0;
			} else {
				SelectedFrame = SelectedFrame + 1;
			}

			anim.Animations [SelectedAnimation].animationFrames.Insert (SelectedFrame, new FF_AnimationFrame ());

			return;
		}
		if (GUILayout.Button ("-", GUILayout.Width (25))) {
			if(!ValidateAnimationList(anim,2)) {
				return;
			}

			if (DisplayBox != null) {
				DestroyImmediate (DisplayBox);
			}
			if (anim.Animations [SelectedAnimation].animationFrames.Count > 0) {
				anim.Animations [SelectedAnimation].animationFrames.RemoveAt (SelectedFrame);
			}

			if (SelectedFrame > anim.Animations [SelectedAnimation].animationFrames.Count - 1) {
				SelectedFrame = anim.Animations [SelectedAnimation].animationFrames.Count - 1;
			}

			return;

		}
		if (ValidateAnimationList (anim, 2)) {
			GUILayout.Label ("Number of Frames: " + anim.Animations [SelectedAnimation].animationFrames.Count.ToString ());
		} else {
			GUILayout.Label("Number of Frames: 0");
		}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();

		if (ValidateAnimationList (anim, 1)) {
			if (anim.Animations [SelectedAnimation].animationFrames.Count == 0) {
				if (DisplayBox != null) {
					DestroyImmediate (DisplayBox);
				}

				SelectedObject.GetComponent<SpriteRenderer> ().sprite = null;
				return;
			}
		} else {
			return;
		}

		EditorGUILayout.Separator ();
		LastElementY = GUILayoutUtility.GetLastRect ().y;
		GUILayout.BeginHorizontal ();
		GUI.color = Color.cyan;
		for (int i = 0; i < anim.Animations[SelectedAnimation].animationFrames.Count; i++) {
			Rect AnimationScrubberBoxSize = new Rect (i*(position.width / anim.Animations[SelectedAnimation].animationFrames.Count), LastElementY, position.width / anim.Animations[SelectedAnimation].animationFrames.Count, 25);
			if(i == SelectedFrame) {
				GUI.color = Color.yellow;
			}
			if (GUI.Button (AnimationScrubberBoxSize, (i+1).ToString())) {
				SelectedFrame = i;
				GUIUtility.keyboardControl = 0;
				SelectedBox = 0;
			}
			GUI.color = Color.cyan;
		}

		if (SelectedObject != null) {
			SelectedObject.GetComponent<SpriteRenderer> ().sprite = anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].frameSprite;
		}

		GUI.color = DefaultGUIColor;
		GUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.BeginHorizontal ();
		Sprite tempSprite = EditorGUILayout.ObjectField ("Sprite: ", anim.Animations[SelectedAnimation].animationFrames[SelectedFrame].frameSprite, typeof(Sprite), true) as Sprite;
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();

		anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].frameSprite = tempSprite;
	}

	void LayoutBoxButtons(FF_Animator anim) {
		//Fix stupid no box selection after coming from animation with no boxes
		if (!ValidateAnimationList (anim, 2)) {
			SelectedBox = 0;
		}

		//Layout box buttons.
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("+", GUILayout.Width (50))) {//ADD A BOX AFTER THE SELECTED INDICIE
			if(!ValidateAnimationList(anim,2)) {
				return;
			}

			if (SelectedBox > anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count) {
				SelectedBox = anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count;
			}

			if (anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count != 0) {
				anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Insert (SelectedBox + 1, new FF_BoxData ());
				SelectedBox = SelectedBox + 1;
			} else {
				anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Insert (0, new FF_BoxData ());
				SelectedBox = 0;
			}

			return;

		}
		if (GUILayout.Button ("-", GUILayout.Width (25))) { //DELT THIS
			if(!ValidateAnimationList(anim,3)) {
				return;
			}

			DestroyImmediate(DisplayBox);

			if (anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count != 0) {
				anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.RemoveAt (SelectedBox);
			}

			if (SelectedBox > anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count - 1) {
				SelectedBox = anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count - 1;
			}

			return;
		}

		if (ValidateAnimationList (anim, 3)) {
			GUILayout.Label ("Number of Boxes: " + anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count.ToString ());
		} else {
			GUILayout.Label ("Number of Boxes: 0");
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();

		if (!ValidateAnimationList(anim,3)) {
			if (DisplayBox != null) {
				DestroyImmediate (DisplayBox);
			}
			return;
		}

		EditorGUILayout.Separator ();
		LastElementY = GUILayoutUtility.GetLastRect ().y;
		GUILayout.BeginHorizontal ();
		GUI.color = Color.cyan;
		for (int i = 0; i < anim.Animations[SelectedAnimation].animationFrames[SelectedFrame].boxes.Count; i++) {
			Rect AnimationScrubberBoxSize = new Rect (i*(position.width / anim.Animations[SelectedAnimation].animationFrames[SelectedFrame].boxes.Count), LastElementY, position.width / anim.Animations[SelectedAnimation].animationFrames[SelectedFrame].boxes.Count, 25);
			if(i == SelectedBox) {
				GUI.color = Color.yellow;
			}
			if (GUI.Button (AnimationScrubberBoxSize, (i+1).ToString())) {
				SelectedBox = i;
				GUIUtility.keyboardControl = 0;
			}
			GUI.color = Color.cyan;
		}
		GUI.color = DefaultGUIColor;
		GUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.BeginHorizontal ();
		if(ValidateAnimationList(anim,3)) {
			bool tempCanDoDdamage = EditorGUILayout.Toggle ("Can Do Damage", anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].isHitBox);
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].isHitBox = tempCanDoDdamage;
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
	}

	void InitializeDisplayBox(FF_Animator anim) {
		if (anim.Animations.Count == 0) {
			return;
		}
		if (anim.Animations [SelectedAnimation].animationFrames.Count == 0) {
			return;
		}
		if (anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count == 0) {
			return;
		}

		if(anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes.Count < 1) {
			return;
		}

		Rect boxParam = new Rect(anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes[SelectedBox].boxX,
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes[SelectedBox].boxY,
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes[SelectedBox].boxWidth,
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes[SelectedBox].boxHeight);
		float rotationParam = anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].boxRotation;


		if (LastSelectedAnimation != SelectedAnimation || LastSelectedFrame != SelectedFrame || LastSelectedBox != SelectedBox || DisplayBox == null) {
			LastSelectedAnimation = SelectedAnimation;
			LastSelectedFrame = SelectedFrame;
			LastSelectedBox = SelectedBox;

			MakeDisplayBox (anim,boxParam,rotationParam);
		}

		if (DisplayBox != null) {
			bool isHitBox = anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].isHitBox;
			if (isHitBox) {
				DisplayBox.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 0.5f);
			} else {
				DisplayBox.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0, 0.5f);
			}
		}
	}

	void MakeDisplayBox(FF_Animator anim, Rect boxParam, float rotationParam) {
		if (DisplayBox != null) {
			GameObject.DestroyImmediate (DisplayBox);
		}

		DisplayBox = PrefabUtility.InstantiatePrefab (anim.HitHurtBoxReference.gameObject) as GameObject;
		DisplayBox.transform.parent = SelectedObject.transform;
		DisplayBox.name = "FF_Box_TEMPONLY_" + SelectedBox.ToString ();
		DisplayBox.transform.localPosition = new Vector3 (boxParam.x,boxParam.y,1f);
		float localWidth;
		float localHeight;
		if (boxParam.width <= 0)
			localWidth = 3;
		else
			localWidth = boxParam.width;

		if (boxParam.height <= 0)
			localHeight = 3;
		else
			localHeight = boxParam.height;
		DisplayBox.transform.localScale = new Vector3 (localWidth,localHeight,1f);
		DisplayBox.transform.localRotation = Quaternion.Euler (0,0,rotationParam);



		System.GC.Collect (); //Creating and destroying objects seems to freeze the editor in time, no matter what you're doing. Do a garbage collection.

	}

	//Returns true or false for a particular array being more than 0
	bool ValidateAnimationList(FF_Animator anim, int layer) { //Sends true or false for whether something exist in the anim.Animations array chain however many layers deep up to 1-3
		if (SelectedBox == -1) {
			SelectedBox = 0;
		}
		if (SelectedFrame == -1) {
			SelectedFrame = 0;
		}
		if (SelectedAnimation == -1) {
			SelectedFrame = 0;
		}


		if (layer == 1) {
			if (SelectedAnimation > anim.Animations.Count - 1) {
				SelectedAnimation = 0;
			}

			if (anim.Animations.Count == 0) {
				return false;
			}
		}

		if (layer == 2) {
			if (SelectedAnimation > anim.Animations.Count - 1) {
				SelectedAnimation = 0;
			}

			if (anim.Animations.Count == 0) {
				return false;
			} else {
				if (SelectedFrame > anim.Animations [SelectedAnimation].animationFrames.Count - 1) {
					SelectedFrame = anim.Animations [SelectedAnimation].animationFrames.Count - 1;
				}
			}
			if (anim.Animations [SelectedAnimation].animationFrames.Count == 0) {
				return false;
			}
		}

		if (layer == 3) {
			if (SelectedAnimation > anim.Animations.Count - 1) {
				SelectedAnimation = 0;
			}

			if (anim.Animations.Count == 0) {
				return false;
			} else {
				if (SelectedFrame > anim.Animations [SelectedAnimation].animationFrames.Count - 1) {
					SelectedFrame = anim.Animations [SelectedAnimation].animationFrames.Count - 1;
				}
			}

			if (anim.Animations [SelectedAnimation].animationFrames.Count == 0) {
				return false;
			} else {
				if (SelectedBox > anim.Animations [SelectedAnimation].animationFrames[SelectedFrame].boxes.Count - 1) {
					SelectedBox = anim.Animations [SelectedAnimation].animationFrames[SelectedFrame].boxes.Count - 1;
				}
			}

			if(anim.Animations [SelectedAnimation].animationFrames[SelectedFrame].boxes.Count == 0) {
				return false;
			}
		}



		return true;
	}

	void WriteDisplayBoxData(FF_Animator anim) {
		//Set the box info while a box is selected.
		if (DisplayBox != null) {
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].boxX = DisplayBox.transform.localPosition.x;
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].boxY = DisplayBox.transform.localPosition.y;
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].boxWidth = DisplayBox.transform.localScale.x;
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].boxHeight = DisplayBox.transform.localScale.y;
			anim.Animations [SelectedAnimation].animationFrames [SelectedFrame].boxes [SelectedBox].boxRotation = DisplayBox.transform.localEulerAngles.z;
		}
	}


	/*****************************************Utility**************************************************/
	void Close() { //Saves are automagic now.
		if (SelectedObject != null) {
			SelectedObject.GetComponent<SpriteRenderer> ().sprite = DefaultObjectSpriteBeforeEditing;
		}

		SelectedObject = null;
		Selection.activeGameObject = null;
		SelectedAnimation = 0;
		SelectedFrame = 0;
		SelectedBox = 0;
		LastSelectedAnimation = 0;
		LastSelectedFrame = 0;
		LastSelectedBox = 0;
		isActive = false;

		if (DisplayBox != null) {
			DestroyImmediate (DisplayBox);
		}

		return;
	}

	/*void RemoveListEntry<T>(List<T> inList,int indexToRemove) {
		List<T> tempList = new List<T> ();

		for (int i = 0; i < inList.Count; i++) {
			if (i != indexToRemove) {
				tempList.Add (inList [i]);
			}
		}

		inList.Clear ();

		for (int i = 0; i < tempList.Count; i++) {
			inList.Add (tempList [i]);
		}
	}*/
	/**************************************************************************************************/


}
