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
*/

public class Mover : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float jumpHeight;
    public float gravity;    
    public GameObject[] platforms;
    public bool isGrounded;
    public int numJumps;

    RaycastHit hitInfo;
    bool speedBoost;
    Vector3 velocity;
    int jumpsLeft;

    // Values should be set in the inspector
    void Start () {
        jumpsLeft = numJumps;

	}

	// Gets horizontal input, check
	void Update () {
        float movement = Input.GetAxis("Horizontal");
        //Debug.Log(movement);
        if (movement != 0)
            checkForPlatform();
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        Vector3 newPos = new Vector3(transform.position.x + movement * speed, transform.position.y, 0f);
        transform.position = newPos;
        Jump();
	}

    void Jump()
    {
        if(!isGrounded)
            checkForPlatform();

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
            velocity.y -= gravity;
        }
        

        if(velocity.magnitude>maxSpeed)
            velocity *= maxSpeed / velocity.magnitude;

        transform.position += velocity*Time.deltaTime;
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
        
        Ray rae = new Ray(transform.position, Vector3.down);


        Debug.DrawLine(rae.origin, rae.origin + rae.direction * gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f);
        
        if (Physics.Raycast(rae, out hitInfo, gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f) && hitInfo.collider.gameObject.CompareTag("Platform"))   // put an && that checks that your position is above the platform to fix snapping from below
        {
            //Debug.Log("iagsdkug");
            if (!isGrounded)
            {

                //print("Collided With " + hitInfo.collider.gameObject.name);
                Vector3 newPos = new Vector3(transform.position.x, hitInfo.transform.position.y + hitInfo.collider.bounds.size.y * 0.5f + gameObject.GetComponent<Collider2D>().bounds.size.y * 0.5f + 0.002f, 0f);
                isGrounded = true;
                transform.position = newPos;
            }

        }
        else
        {
            isGrounded = false;
        }
        
        
    }


}
