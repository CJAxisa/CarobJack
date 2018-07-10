using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D), typeof (AudioManager))]
public class Player : MonoBehaviour {

  private GameObject [] cameras;
  private Vector3 respawnPoint;
	private float jumpCount;
  private float jumpTimer;
  [Range(0, 25)] public float walkSpeed;
	[Range (1, 10)] public int numJumps;
  [Range (0.0f, 0.5f)] public float modifyJumpHeightTimeWindow;
  [Range(0, 10)] public float fallOffJumpForce;
	[Range (-30, 0)] public float gravity = -20;
	[Range (0, 15)] public float jumpForce = 8.5f;
  [Range (0.0f, 1.0f)] public float secondJumpModifier;
	public Vector3 velocity;
  public bool isGrounded;
  public bool isFloating;

  public Attributes playerAttributes = new Attributes();
  //public TomeManager tomeManager = new TomeManager();

	Controller2D controller;

	void Start () {
		controller = GetComponent<Controller2D> ();
    CheckIfGrounded();
    isFloating = false;
		jumpCount = 0;
    cameras = new GameObject[4];
    cameras[0] = GameObject.Find("Main Camera");
    cameras[1] = GameObject.Find("IntroCamera");
    cameras[2] = GameObject.Find("BossCamera");
    cameras[3] = GameObject.Find("SecretCamera");

    for(int i = 1; i < cameras.Length; i++) {
      if(cameras[i] != null) {
        cameras[i].SetActive(false);
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
    if(Input.GetButtonDown("Jump") && controller.collisions.below) {
      // So if the player presses 'w' AND the player object is standing on something
      velocity.y = jumpForce;
      jumpCount++;
    }
    else if(Input.GetButtonDown("Jump") && !controller.collisions.below && jumpCount < numJumps) {
      velocity.y = (jumpForce - (jumpCount/jumpForce)) * secondJumpModifier; //* 0.75f;
      jumpCount++;
      jumpTimer = 0;
    }
    jumpTimer += Time.deltaTime;
    if(Input.GetButtonUp("Jump") && !controller.collisions.below && velocity.y > 0) {
      //if(jumpTimer < modifyJumpHeightTimeWindow) {
        velocity.y = fallOffJumpForce;
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
      cameras[1].SetActive(true);
    }
    if(collider.gameObject.CompareTag("Enemy")) {
      print("Enemy detected");
    }
    if(collider.gameObject.CompareTag("ToggleCamera")) {
      print("Toggle Main Camera off!");
      cameras[0].SetActive(false);

      if(collider.gameObject.name == "ToggleIntroCamera") {
        print("Intro camera on!");
        cameras[1].SetActive(true);
      }
      else if(collider.gameObject.name == "ToggleBossCamera") {
        print("Boss camera on!");
        cameras[2].SetActive(true);
      }
      else {
        print("Secret camera on!");
        cameras[3].SetActive(true);
      }
    }
  }

  void OnTriggerExit2D(Collider2D collider) {
    if(collider.gameObject.CompareTag("ToggleCamera")) {
      if(collider.gameObject.name == "ToggleIntroCamera") {
        print("Intro camera off!");
        cameras[1].SetActive(false);
      }
      else if(collider.gameObject.name == "ToggleBossCamera") {
        print("Boss camera off!");
        cameras[2].SetActive(false);
      }
      else {
        print("Secret camera off!");
        cameras[3].SetActive(false);
      }

      print("Main camera on!");
      cameras[0].SetActive(true);
      cameras[0].transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 10);
    }
  }
}

// This class will be used to store the player statistics
public class Attributes {
  //TODO: Discuss whether this should be a float or int
  public float MaxHealth;

  //TODO: Discuss what other resistances the player will have
  public float MaxAttackDefense;
  public float MaxMagicDefense;

  //TODO: Discuss what other damage types the player will have
  public float MaxMagicDamage;
  public float MaxAttackDamage;

  //TODO: Discuss other types of luck attributes we can have
  public float DodgeChance;
  public float CritChance;

  //TODO: Discuss other reductions we could have
  public float BaseCooldownReduction;

  //TODO: Use these in the main Player class
  public float CurrentHealth;
  public int CurrentNumJumps;
  public int MaxNumJumps;
}
