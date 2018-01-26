using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu;
using UnityEngine.SceneManagement;
/* This handles the player's health and check for enemy collision
 *  This is attached to the player gameObject
 *
 *
*/
[RequireComponent (typeof (CharacterController2D))]
public class Player : MonoBehaviour {

  private GameObject[] enemies;
  private GameObject[] lava;

	public int health;
	public float walkSpeed;
	public int numJumps;
	private int jumpCount;
	public float jumpForce = 8.5f;
	public float gravity = -29;
	public bool facingRight, facingLeft;
	public float knockbackFactorX, knockbackFactorY;

	private Vector3 velocity;
	private CharacterController2D controller;
	private Transform rotationPoint;

	private float timer;
  public float delay;
  private bool Dot;

	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        lava = GameObject.FindGameObjectsWithTag("Lava");

				controller = GetComponent<CharacterController2D>();
				rotationPoint = gameObject.transform.Find("RotationPoint");
				jumpCount = 0;
	}

	void Update () {
		if(UIManager.isPaused) {
			return;
		}
		Vector2 input = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
		velocity.x = input.x * walkSpeed;
		velocity.y += gravity * Time.deltaTime;
		ManageJump();		// This handles the players ability to jump when grounded/not grounded

		controller.Move(velocity * Time.deltaTime);		// Move is a function in CharacterController2D script which handles movement of the player

		ManagePlayerOrientation();	// Here we handle orientation of the player, sprite, and colliders of tomes
		ManagePlayerHealth();				// Here we handle taking damage + what to do at certain HP (i.e. if hp == 0 then we call Die() function)
		//checkForEnemies();
	}

	void ManagePlayerOrientation() {
		if(velocity.x < 0 && !facingLeft) {
			gameObject.GetComponent<SpriteRenderer>().flipX = true;
			rotationPoint.transform.Rotate(Vector3.up * 180);
			facingLeft = true;
			facingRight = false;
		}
		else if(velocity.x > 0 && !facingRight) {
			gameObject.GetComponent<SpriteRenderer>().flipX = false;
			rotationPoint.transform.Rotate(Vector3.up * -180);
			facingRight = true;
			facingLeft = false;
		}
	}

	void ManageJump() {
		if(controller.collisions.below) {
			// Here, the player is grounded
			velocity.y = 0f;
			if(jumpCount != 0) {
				jumpCount = 0;	// reset jump count
			}
			if(Input.GetKeyDown("w")) {
				velocity.y = jumpForce;
				jumpCount++;
			}
		}
		else {
			// Here, the player is NOT grounded
			if(controller.collisions.above) {
				velocity.y = 0f;	// This resets gravity when you hit the ceiling which feels nice
			}
			if(Input.GetKeyDown("w")) {
				if(jumpCount < numJumps) {
					velocity.y = jumpForce;
					jumpCount++;
				}
			}
			if(velocity.y > 0) {
				// Here the player has jumped and has not reached the apex of the jump
				if(Input.GetKeyUp("w")) {
					// Basically, if the player taps 'w' then they won't get the full jump height
					if(velocity.y >= jumpForce * 0.7) {
						velocity.y -= velocity.y * 0.3f;
					}
				}
			}
			else if(velocity.y < 0) {
				// Here the player has reached the apex of the jump
				//TODO: Increase gravity so that you fall faster over time after a jump or when falling
			}
			/* Uncomment here if we want it so that the player loses a jump after walking off a platform (like in smash melee)
			if(jumpCount == 0) {
				jumpCount++;
			}
			*/
		}
	}

	void ManagePlayerHealth() {
		if (health <= 0) {
			Die();
			SceneManager.LoadScene(2);
		}
		if (Dot) {
			if (timer > delay){
				Dot = false;
			// timer = 0;
			}
			timer += 1.0f * Time.deltaTime * 5;
			Debug.Log("Timer = " + timer);
			//TODO: lose health every second
			if (timer >= delay - 0.5 && timer <= delay){
				LoseHealth(1);
				timer = 0;
			}
		}
	}

  public void Die() {
  	Destroy(gameObject);
  }

  private void LoseHealth(int numHealth) {
  	health -= numHealth;
  }

  private void knockBack() {
  	Debug.Log("we out here");
    if(facingRight)
    	controller.knockbackFactor = new Vector3(-knockbackFactorX, knockbackFactorY, 0f);
    else
    	controller.knockbackFactor = new Vector3(knockbackFactorX, knockbackFactorY, 0f);
    controller.knockback();
  }

  public void getHit() {
  	LoseHealth(1);
    knockBack();
  }

  private void checkForEnemies() {
        /*
        //goes through each enemy and does AABB (Axis Aligned Bounding Box) to check ffor collisions.
        foreach (GameObject enem in enemies)
        {
            /*
             * This will check if;
             * box B's x min is less than box a's x max
             * box B's x max is greater than box a's x min
             * box B's y min is less than box a's y max
             * box B's y max is greater than box a's y min
             *
             * if all of these are true, then we have a collision

            if (gameObject.GetComponent<Collider2D>().bounds.min.x < enem.GetComponent<Collider2D>().bounds.max.x &&
                gameObject.GetComponent<Collider2D>().bounds.min.x > enem.GetComponent<Collider2D>().bounds.max.x &&
                gameObject.GetComponent<Collider2D>().bounds.min.y > enem.GetComponent<Collider2D>().bounds.max.y &&
                gameObject.GetComponent<Collider2D>().bounds.min.y < enem.GetComponent<Collider2D>().bounds.max.y
               )
            {
                Debug.Log(enem);

            }
        }
    */
  }

  public void OnTriggerStay2D(Collider2D co) {
  	// Debug.Log("AAAAAAAA");
    // if (co.gameObject.CompareTag("Enemy")){
    //     loseHealth(1);
    // }
    if (co.gameObject.CompareTag("Lava")){
    	Dot = true;
      // loseHealth(1);
      // knockBack();
		}
  }

	public void OnTriggerEnter2D(Collider2D co) {
		if(co.gameObject.CompareTag("Victory")){
			SceneManager.LoadScene(3);
		}
	}

  public void OnTriggerExit2D(Collider2D co){
  	if (co.gameObject.CompareTag("Lava")){
    	Dot = false;
      timer = 0;
    }
  }
}
