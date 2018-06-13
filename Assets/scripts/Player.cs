using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D), typeof (AudioManager))]
public class Player : MonoBehaviour {

  private GameObject [] camera;
  private Vector3 respawnPoint;
	private float jumpCount;
  private float jumpTimer;
  public float walkSpeed;
	public float numJumps;
  public float modifyJumpHeightTimeWindow;
  public float fallOffJumpHeight;
	public float gravity = -20;
	public float jumpForce = 8.5f;
  public float secondJumpModifier;
	public static Vector3 velocity;
  public static bool isGrounded;
  public static bool isFloating;

	Controller2D controller;

	void Start () {
		controller = GetComponent<Controller2D> ();
    CheckIfGrounded();
    isFloating = false;
		jumpCount = 0;
    camera = new GameObject[4];
    camera[0] = GameObject.Find("Main Camera");
    camera[1] = GameObject.Find("IntroCamera");
    camera[2] = GameObject.Find("BossCamera");
    camera[3] = GameObject.Find("SecretCamera");

    for(int i = 1; i < camera.Length; i++) {
      if(camera[i] != null) {
        camera[i].SetActive(false);
      }
    }

    respawnPoint = new Vector3(-234.8f, 36.375f, 0f);
	}

	void Update () {
    CheckIfGrounded();
    ManageJump();
    ManageMovement();
	}

  void CheckIfGrounded() {
    if(controller.collisions.below) {
      isGrounded = true;
      isFloating = false;
      jumpCount = 0;
      jumpTimer = 0;
      velocity.y = 0f;
    }
    else if(controller.collisions.above) {
      isGrounded = false;
      velocity.y = 0f; // The player will fall as soon as he collides with the ceiling
    }
    else {
      isGrounded = false; // The player is in mid-air
    }
  }

  void ManageJump() {
    if(Input.GetKeyDown("w") && controller.collisions.below) {
      // So if the player presses 'w' AND the player object is standing on something
      velocity.y = jumpForce;
      jumpCount++;
    }
    else if(Input.GetKeyDown("w") && !controller.collisions.below && jumpCount < numJumps) {
      velocity.y = (jumpForce - (jumpCount/jumpForce)) * secondJumpModifier; //* 0.75f;
      jumpCount++;
      jumpTimer = 0;
    }
    jumpTimer += Time.deltaTime;
    if(Input.GetKeyUp("w") && !controller.collisions.below && velocity.y > 0) {
      //if(jumpTimer < modifyJumpHeightTimeWindow) {
        velocity.y = fallOffJumpHeight;
      //}
    }
  }

  void ManageMovement() {
    Vector2 input = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
    velocity.x = input.x * walkSpeed;
    if(!isFloating) {
      velocity.y += gravity * Time.deltaTime;
    }
    controller.Move(velocity * Time.deltaTime);
  }

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.CompareTag("Hazard")) {
      print("Hazard detected!");
      gameObject.transform.position = respawnPoint;
      camera[1].SetActive(true);
    }
    if(collider.gameObject.CompareTag("Enemy")) {
      print("Enemy detected");
    }
    if(collider.gameObject.CompareTag("ToggleCamera")) {
      print("Toggle Main Camera off!");
      camera[0].SetActive(false);

      if(collider.gameObject.name == "ToggleIntroCamera") {
        print("Intro camera on!");
        camera[1].SetActive(true);
      }
      else if(collider.gameObject.name == "ToggleBossCamera") {
        print("Boss camera on!");
        camera[2].SetActive(true);
      }
      else {
        print("Secret camera on!");
        camera[3].SetActive(true);
      }
    }
  }

  void OnTriggerExit2D(Collider2D collider) {
    if(collider.gameObject.CompareTag("ToggleCamera")) {
      if(collider.gameObject.name == "ToggleIntroCamera") {
        print("Intro camera off!");
        camera[1].SetActive(false);
      }
      else if(collider.gameObject.name == "ToggleBossCamera") {
        print("Boss camera off!");
        camera[2].SetActive(false);
      }
      else {
        print("Secret camera off!");
        camera[3].SetActive(false);
      }

      print("Main camera on!");
      camera[0].SetActive(true);
    }
  }
}
