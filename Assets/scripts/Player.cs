using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D), typeof (AudioManager))]
public class Player : MonoBehaviour {

    // CJ CODE ---------------------------------
    public enum PlayerStates
    {
        Empty,
        Idle,
        Walking,
        Rising,
        Falling,
        Casting
    }
    public bool facingRight;
    public PlayerStates currentState;
    private PlayerStates prevState;
    Animator playerAnimator;
    SpriteRenderer playerSpriteRenderer;

    // NICK CODE -------------------------------
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
            camera[i].SetActive(false);
        }

        respawnPoint = new Vector3(-234.8f, 36.375f, 0f);

        currentState = PlayerStates.Idle;
        playerAnimator = gameObject.GetComponent<Animator>();
        playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        facingRight = true;
	}

	void Update () {
        // NICK CODE --------------------------
     	CheckIfGrounded();
        ManageJump();
        ManageMovement();

        // CJ CODE ----------------------------
        //used for checking if state has changed since last frame
        prevState = currentState;

        //animation stuff
        if (velocity.x > 0)
            facingRight = true;
        else if (velocity.x < 0)
            facingRight = false;

        if (isGrounded && Mathf.Abs(velocity.x) > 0)
            currentState = PlayerStates.Walking;
        if (isGrounded && velocity.x == 0)
            currentState = PlayerStates.Idle;
        if (!isGrounded && velocity.y < 0)
            currentState = PlayerStates.Falling;
        if (!isGrounded && velocity.y > 0)
            currentState = PlayerStates.Rising;
        if (Input.GetKeyDown("p"))
        {
            currentState = PlayerStates.Casting;
        }

        if (facingRight)
            playerSpriteRenderer.flipX = false;
        else
            playerSpriteRenderer.flipX = true;

        if (prevState != currentState)
        {
            //Debug.Log("STATE HAS CHANGED");
            switch (currentState)
            {
                case PlayerStates.Idle:
                    playerAnimator.SetBool("IsIdle", true);
                    playerAnimator.SetBool("IsWalking", false);
                    playerAnimator.SetBool("IsRising", false);
                    playerAnimator.SetBool("IsFalling", false);
                    playerAnimator.SetBool("IsCasting", false);
                    break;
                case PlayerStates.Walking:
                    playerAnimator.SetBool("IsIdle", false);
                    playerAnimator.SetBool("IsWalking", true);
                    playerAnimator.SetBool("IsRising", false);
                    playerAnimator.SetBool("IsFalling", false);
                    playerAnimator.SetBool("IsCasting", false);
                    break;
                case PlayerStates.Rising:
                    playerAnimator.SetBool("IsIdle", false);
                    playerAnimator.SetBool("IsWalking", false);
                    playerAnimator.SetBool("IsRising", true);
                    playerAnimator.SetBool("IsFalling", false);
                    playerAnimator.SetBool("IsCasting", false);
                    break;
                case PlayerStates.Falling:
                    playerAnimator.SetBool("IsIdle", false);
                    playerAnimator.SetBool("IsWalking", false);
                    playerAnimator.SetBool("IsRising", false);
                    playerAnimator.SetBool("IsFalling", true);
                    playerAnimator.SetBool("IsCasting", false);
                    break;
                case PlayerStates.Casting:
                    playerAnimator.SetBool("IsIdle", false);
                    playerAnimator.SetBool("IsWalking", false);
                    playerAnimator.SetBool("IsRising", false);
                    playerAnimator.SetBool("IsFalling", false);
                    playerAnimator.SetBool("IsCasting", true);
                    break;
                default:
                    break;
            }
        }
    }

    void Attack()
    {
    }

    // NICK CODE ----------------------------------
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
        currentState = PlayerStates.Rising;
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
            gameObject.GetComponent<TomeManager>().createTomes();
            camera[1].SetActive(true);
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
    public float MaxPhysicalDefense;
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
