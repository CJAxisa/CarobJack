using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu;
/*
This script is attached to player.
It handles the following
Movement,
Jumping,
Air velocity,
Checking for platforms,
Checking for walls.
*/

public class Mover : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float jumpHeight;
    public float gravity;
    public GameObject[] platforms;
		public Transform rotationPoint;
    public static bool isGrounded;
    public int numJumps;

    public Vector3 knockbackFactor;

    RaycastHit hitInfo;
    bool speedBoost;
    public static Vector3 velocity; // NEW: needs to be static so i can modify y velocity in float tome class
	public static bool isFloating;  // NEW: needs to be static so i can modify in float tome class - also necessary so you don't apply gravity while floating
	public bool facingRight; // NEW: needed to change direction player is facing
	public bool facingLeft; // NEW: neede to change direction player is facing
    int jumpsLeft;

    // Values should be set in the inspector
    void Start () {
        jumpsLeft = numJumps;
		isFloating = false;
		facingLeft = true; // NEW: player will start facing right
        facingRight = false;
        knockbackFactor = Vector3.zero;
				rotationPoint = gameObject.transform.GetChild(0);
	}

	// Gets horizontal input, check
	void Update () {
        if(UIManager.isPaused) {
          return;
        }
        float movement = 0f;
        if(!wallCheck())
            movement = Input.GetAxis("Horizontal");

        //Debug.Log(movement);
        if (movement != 0)
            checkForPlatform();
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        Vector3 newPos = new Vector3(transform.position.x + movement * speed, transform.position.y, 0f);
        transform.position = newPos;
        Jump();
        knockback();
		/* NEW: Here I added it so that way it flips the players rotation when he is facing left and right */
		if(Input.GetKeyDown("a") && facingLeft == false) {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
						rotationPoint.transform.Rotate(Vector3.up * 180);
            facingLeft = true; // need to set this to true so that way you don't flip the player again when you move to the right
						facingRight = false;
						// HERE ADD SPRITE FACING RIGHT IDLE ANIMATION
		}
		if(Input.GetKeyDown("d") && facingRight == false) {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
						rotationPoint.transform.Rotate(Vector3.up * 180);
						//transform.Rotate(Vector3.up * 180);
						facingRight = true; // need to set this to true so that way you don't flip the player again when you move to the right
						facingLeft = false;
						// HERE ADD SPRITE FACING LEFT IDLE ANIMATION
		}
	}

    void Jump()
    {
			if(!isGrounded) {
					//checkForCeiling();
					checkForPlatform();
			}
        if (Input.GetKeyDown(KeyCode.Space)&&jumpsLeft>0)
        {
            jumpsLeft--;
            isGrounded = false;
            velocity.y = jumpHeight;
        }

        if (isGrounded)
        {
            velocity.y = 0.000001f;
            jumpsLeft = numJumps;
        }
        else
        {
			// NEW: This little if statement is needed for the float tome to work
			if(!isFloating)
                velocity.y -= gravity;
        }


        if(velocity.magnitude>maxSpeed)
            velocity *= maxSpeed / velocity.magnitude;

        transform.position += velocity*Time.deltaTime;
    }

    public void knockback()
    {
        transform.Translate(knockbackFactor);
				//knockbackFactor *= 0.95f;
				knockbackFactor *= 0.9f;
    }

    public void checkForPlatform()
    {
        /*foreach (GameObject p in platforms)
        {
            //AABB collision
            if (gameObject.transform.position.x- gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f < p.transform.position.x+p.GetComponent<SpriteRenderer>().size.x*0.5f
                && gameObject.transform.position.x + gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f > p.transform.position.x - p.GetComponent<SpriteRenderer>().size.x * 0.5f
                && gameObject.transform.position.y - gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f < p.transform.position.y + p.GetComponent<SpriteRenderer>().size.y * 0.5f
                && gameObject.transform.position.y + gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f > p.transform.position.y - p.GetComponent<SpriteRenderer>().size.y * 0.5f)
            {
                isPlatform = true;
            }
        }*/

        Vector3 rayPos = transform.position;
        rayPos.y -= gameObject.GetComponent<Collider2D>().bounds.extents.y;
        rayPos.y += 0.2f;
        Ray rae = new Ray(rayPos, Vector3.down);



        Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

        if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
        {
            if(transform.position.y-gameObject.GetComponent<Collider2D>().bounds.extents.y*0.8f > hitInfo.transform.position.y)
            {
                if (!isGrounded)
                {

                    //print("Collided With " + hitInfo.collider.gameObject.name);
                    Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y + hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
                    isGrounded = true;
                    transform.position = newPos;
                    return;
                }
            }
        }

        rayPos.x += gameObject.GetComponent<Collider2D>().bounds.extents.x;
        rae.origin = rayPos;

        if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
        {
            if (transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.8f > hitInfo.transform.position.y)
            {
                if (!isGrounded)
                {

                    //print("Collided With " + hitInfo.collider.gameObject.name);
                    Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y + hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
                    isGrounded = true;
                    transform.position = newPos;
                    return;
                }
            }
        }

        rayPos.x -= gameObject.GetComponent<Collider2D>().bounds.size.x;
        rae.origin = rayPos;

        if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
        {
            if (transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.8f > hitInfo.transform.position.y)
            {
                if (!isGrounded)
                {

                    //print("Collided With " + hitInfo.collider.gameObject.name);
                    Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y + hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
                    isGrounded = true;
                    transform.position = newPos;
                    return;
                }
            }
        }

        isGrounded = false;


    }


		public void checkForCeiling() {
					Vector3 rayPos = transform.position;
					rayPos.y += gameObject.GetComponent<Collider2D>().bounds.extents.y;
					rayPos.y -= 0.2f;
					Ray rae = new Ray(rayPos, Vector3.up);

					Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

					if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
					{
							Debug.Log("Hit a ceiling");
							Debug.Log(hitInfo.transform.position.y - hitInfo.collider.bounds.min.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f);
							Debug.Log(transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y*0.8f + " < " + hitInfo.transform.position.y);
							if(transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y*0.8f < hitInfo.transform.position.y)
							{
									Debug.Log("Distance between player and ceiling is low");
									if (!isGrounded)
									{
											//print("Collided With " + hitInfo.collider.gameObject.name);
											Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y - hitInfo.collider.bounds.min.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
											//isGrounded = true;
											transform.position = newPos;
											return;
									}
							}
					}

					rayPos.x += gameObject.GetComponent<Collider2D>().bounds.extents.x;
					rae.origin = rayPos;

					Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

					if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
					{
							Debug.Log("Hit a ceiling");
							if (transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.8f < hitInfo.transform.position.y)
							{
									if (!isGrounded)
									{

											//print("Collided With " + hitInfo.collider.gameObject.name);
											Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y - hitInfo.collider.bounds.min.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
											//isGrounded = true;
											transform.position = newPos;
											return;
									}
							}
					}

					rayPos.x -= gameObject.GetComponent<Collider2D>().bounds.size.x;
					rae.origin = rayPos;

					Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

					if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
					{
							Debug.Log("Hit a ceiling");
							if (transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.8f < hitInfo.transform.position.y)
							{
									Debug.Log(hitInfo.collider.bounds.size.y);
									if (!isGrounded)
									{

											//print("Collided With " + hitInfo.collider.gameObject.name);
											Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y - hitInfo.collider.bounds.min.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
											//isGrounded = true;
											transform.position = newPos;
											return;
									}
							}
					}
					isGrounded = false;
		}

    bool wallCheck()
    {

        Vector3 rayPos;
        Ray rae;
        if (facingRight)
        {
            rayPos = transform.position;
            rayPos.x += gameObject.GetComponent<Collider2D>().bounds.extents.x;
            rayPos.x -= 0.3f;
            rayPos.y -= gameObject.GetComponent<Collider2D>().bounds.extents.y-0.01f;
            rae = new Ray(rayPos, Vector3.right);
        }
        else
        {
            rayPos = transform.position;
            rayPos.x -= gameObject.GetComponent<Collider2D>().bounds.extents.x;
            rayPos.x += 0.3f;
            rayPos.y -= gameObject.GetComponent<Collider2D>().bounds.extents.y - 0.01f;
            rae = new Ray(rayPos, Vector3.left);
        }

        if (Physics.Raycast(rae, out hitInfo, 0.3f) && hitInfo.collider.gameObject.CompareTag("Platform"))
        {
            Vector3 newPos;
            if (facingRight)
                newPos = new Vector3(hitInfo.transform.position.x - hitInfo.collider.bounds.extents.x - gameObject.GetComponent<Collider2D>().bounds.extents.x - 0.0000001f, transform.position.y, 0f);
            else
                newPos = new Vector3(hitInfo.transform.position.x + hitInfo.collider.bounds.extents.x + gameObject.GetComponent<Collider2D>().bounds.extents.x + 0.0000001f, transform.position.y, 0f);
            transform.position = newPos;
        }


        rayPos.y += gameObject.GetComponent<Collider2D>().bounds.size.y;
        rae.origin = rayPos;
        if (Physics.Raycast(rae, out hitInfo, 0.3f) && hitInfo.collider.gameObject.CompareTag("Platform"))
        {
            Vector3 newPos;
            if (facingRight)
                newPos = new Vector3(hitInfo.transform.position.x - hitInfo.collider.bounds.extents.x - gameObject.GetComponent<Collider2D>().bounds.extents.x - 0.0000001f, transform.position.y, 0f);
            else
                newPos = new Vector3(hitInfo.transform.position.x + hitInfo.collider.bounds.extents.x + gameObject.GetComponent<Collider2D>().bounds.extents.x + 0.0000001f, transform.position.y, 0f);
            transform.position = newPos;
        }

        rayPos.y -= gameObject.GetComponent<Collider2D>().bounds.size.y;
        rae.origin = rayPos;
        if (Physics.Raycast(rae, out hitInfo, 0.4f) && hitInfo.collider.gameObject.CompareTag("Platform"))
            return true;
        else
            return false;


    }


}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
This script is attached to player.
It handles the following
Movement,
Jumping,
Air velocity,
Checking for platforms,
Checking for walls.


public class Mover : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float jumpHeight;
    public float gravity;
    public GameObject[] platforms;
		public Transform rotationPoint;
    public static bool isGrounded;
    public int numJumps;

    public Vector3 knockbackFactor;

    RaycastHit hitInfo;
    bool speedBoost;
    public static Vector3 velocity; // NEW: needs to be static so i can modify y velocity in float tome class
	public static bool isFloating;  // NEW: needs to be static so i can modify in float tome class - also necessary so you don't apply gravity while floating
	public bool facingRight; // NEW: needed to change direction player is facing
	public bool facingLeft; // NEW: neede to change direction player is facing
    int jumpsLeft;

    // Values should be set in the inspector
    void Start () {
        jumpsLeft = numJumps;
		isFloating = false;
		facingLeft = true; // NEW: player will start facing right
        facingRight = false;
        knockbackFactor = Vector3.zero;
				rotationPoint = gameObject.transform.GetChild(0);
	}

	// Gets horizontal input, check
	void Update () {
        float movement = 0f;
        if(!wallCheck())
            movement = Input.GetAxis("Horizontal");

        //Debug.Log(movement);
        if (movement != 0)
            checkForPlatform();
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        Vector3 newPos = new Vector3(transform.position.x + movement * speed, transform.position.y, 0f);
        transform.position = newPos;
        Jump();
        knockback();
		/* NEW: Here I added it so that way it flips the players rotation when he is facing left and right
		if(Input.GetKeyDown("a") && facingLeft == false) {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
						rotationPoint.transform.Rotate(Vector3.up * 180);
            facingLeft = true; // need to set this to true so that way you don't flip the player again when you move to the right
						facingRight = false;
						// HERE ADD SPRITE FACING RIGHT IDLE ANIMATION
		}
		if(Input.GetKeyDown("d") && facingRight == false) {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
						rotationPoint.transform.Rotate(Vector3.up * 180);
						//transform.Rotate(Vector3.up * 180);
						facingRight = true; // need to set this to true so that way you don't flip the player again when you move to the right
						facingLeft = false;
						// HERE ADD SPRITE FACING LEFT IDLE ANIMATION
		}
	}

    void Jump()
    {
        if(!isGrounded) {
						checkForCeiling();
            checkForPlatform();
				}
        if (Input.GetKeyDown(KeyCode.Space)&&jumpsLeft>0)
        {
            jumpsLeft--;
            isGrounded = false;
            velocity.y = jumpHeight;
        }

        if (isGrounded)
        {
            velocity.y = 0.000001f;
            jumpsLeft = numJumps;
        }
        else
        {
			// NEW: This little if statement is needed for the float tome to work
			if(!isFloating)
                velocity.y -= gravity;
        }


        if(velocity.magnitude>maxSpeed)
            velocity *= maxSpeed / velocity.magnitude;

        transform.position += velocity*Time.deltaTime;
    }

    public void knockback()
    {
        transform.Translate(knockbackFactor);
        //knockbackFactor *= 0.95f;
				knockbackFactor *= 0.9f;
    }

    public void checkForPlatform()
    {
        /*foreach (GameObject p in platforms)
        {
            //AABB collision
            if (gameObject.transform.position.x- gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f < p.transform.position.x+p.GetComponent<SpriteRenderer>().size.x*0.5f
                && gameObject.transform.position.x + gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f > p.transform.position.x - p.GetComponent<SpriteRenderer>().size.x * 0.5f
                && gameObject.transform.position.y - gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f < p.transform.position.y + p.GetComponent<SpriteRenderer>().size.y * 0.5f
                && gameObject.transform.position.y + gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f > p.transform.position.y - p.GetComponent<SpriteRenderer>().size.y * 0.5f)
            {
                isPlatform = true;
            }
        }

        Vector3 rayPos = transform.position;
        rayPos.y -= gameObject.GetComponent<Collider2D>().bounds.extents.y;
        rayPos.y += 0.2f;
        Ray rae = new Ray(rayPos, Vector3.down);



        Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

        if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
        {
            if(transform.position.y-gameObject.GetComponent<Collider2D>().bounds.extents.y*0.8f > hitInfo.transform.position.y)
            {
                if (!isGrounded)
                {

                    //print("Collided With " + hitInfo.collider.gameObject.name);
                    Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y + hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
                    isGrounded = true;
                    transform.position = newPos;
                    return;
                }
            }
        }

        rayPos.x += gameObject.GetComponent<Collider2D>().bounds.extents.x;
        rae.origin = rayPos;

				Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

        if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
        {
            if (transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.8f > hitInfo.transform.position.y)
            {
                if (!isGrounded)
                {

                    //print("Collided With " + hitInfo.collider.gameObject.name);
                    Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y + hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
                    isGrounded = true;
                    transform.position = newPos;
                    return;
                }
            }
        }

        rayPos.x -= gameObject.GetComponent<Collider2D>().bounds.size.x;
        rae.origin = rayPos;

				Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

        if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
        {
            if (transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.8f > hitInfo.transform.position.y)
            {
                if (!isGrounded)
                {

                    //print("Collided With " + hitInfo.collider.gameObject.name);
                    Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y + hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
                    isGrounded = true;
                    transform.position = newPos;
                    return;
                }
            }
        }

        isGrounded = false;


    }

		public void checkForCeiling() {
			Vector3 rayPos = transform.position;
			rayPos.y += gameObject.GetComponent<Collider2D>().bounds.extents.y;
			rayPos.y -= 0.2f;
			Ray rae = new Ray(rayPos, Vector3.up);

			Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

			if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
			{
					Debug.Log("Hit a ceiling");
					if(transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y*0.8f > hitInfo.transform.position.y)
					{
							if (!isGrounded)
							{

									//print("Collided With " + hitInfo.collider.gameObject.name);
									Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y - hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.x * 0.5f + 0.002f, 0f);
									//isGrounded = true;
									transform.position = newPos;
									return;
							}
					}
			}

			rayPos.x += gameObject.GetComponent<Collider2D>().bounds.extents.x;
			rae.origin = rayPos;

			Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

			if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
			{
					Debug.Log("Hit a ceiling");
					if (transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.8f > hitInfo.transform.position.y)
					{
							if (!isGrounded)
							{

									//print("Collided With " + hitInfo.collider.gameObject.name);
									Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y - hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.x * 0.5f + 0.002f, 0f);
									//isGrounded = true;
									transform.position = newPos;
									return;
							}
					}
			}

			rayPos.x -= gameObject.GetComponent<Collider2D>().bounds.size.x;
			rae.origin = rayPos;

			Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);

			if (Physics.Raycast(rae, out hitInfo, 0.2f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
			{
					Debug.Log("Hit a ceiling");
					if (transform.position.y - gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.8f > hitInfo.transform.position.y)
					{
							Debug.Log(hitInfo.collider.bounds.size.y);
							if (!isGrounded)
							{

									//print("Collided With " + hitInfo.collider.gameObject.name);
									Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y - hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.x * 0.5f + 0.002f, 0f);
									//isGrounded = true;
									transform.position = newPos;
									return;
							}
					}
			}
			isGrounded = false;
		}

    bool wallCheck()
    {

        Vector3 rayPos;
        Ray rae;
        if (facingRight)
        {
            rayPos = transform.position;
            rayPos.x += gameObject.GetComponent<Collider2D>().bounds.extents.x;
            rayPos.x -= 0.3f;
            rayPos.y -= gameObject.GetComponent<Collider2D>().bounds.extents.y-0.01f;
            rae = new Ray(rayPos, Vector3.right);
        }
        else
        {
            rayPos = transform.position;
            rayPos.x -= gameObject.GetComponent<Collider2D>().bounds.extents.x;
            rayPos.x += 0.3f;
            rayPos.y -= gameObject.GetComponent<Collider2D>().bounds.extents.y - 0.01f;
            rae = new Ray(rayPos, Vector3.left);
        }

        if (Physics.Raycast(rae, out hitInfo, 0.3f) && hitInfo.collider.gameObject.CompareTag("Platform"))
        {
            Vector3 newPos;
            if (facingRight)
                newPos = new Vector3(hitInfo.transform.position.x - hitInfo.collider.bounds.extents.x - gameObject.GetComponent<Collider2D>().bounds.extents.x - 0.0000001f, transform.position.y, 0f);
            else
                newPos = new Vector3(hitInfo.transform.position.x + hitInfo.collider.bounds.extents.x + gameObject.GetComponent<Collider2D>().bounds.extents.x + 0.0000001f, transform.position.y, 0f);
            transform.position = newPos;
        }


        rayPos.y += gameObject.GetComponent<Collider2D>().bounds.size.y;
        rae.origin = rayPos;
        if (Physics.Raycast(rae, out hitInfo, 0.3f) && hitInfo.collider.gameObject.CompareTag("Platform"))
        {
            Vector3 newPos;
            if (facingRight)
                newPos = new Vector3(hitInfo.transform.position.x - hitInfo.collider.bounds.extents.x - gameObject.GetComponent<Collider2D>().bounds.extents.x - 0.0000001f, transform.position.y, 0f);
            else
                newPos = new Vector3(hitInfo.transform.position.x + hitInfo.collider.bounds.extents.x + gameObject.GetComponent<Collider2D>().bounds.extents.x + 0.0000001f, transform.position.y, 0f);
            transform.position = newPos;
        }

        rayPos.y -= gameObject.GetComponent<Collider2D>().bounds.size.y;
        rae.origin = rayPos;
        if (Physics.Raycast(rae, out hitInfo, 0.4f) && hitInfo.collider.gameObject.CompareTag("Platform"))
            return true;
        else
            return false;


    }


}
*/
