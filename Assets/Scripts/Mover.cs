using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed;
    public float maxSpeed;
    public float jumpHeight;
    public float gravity;
    public float collDist;
    public GameObject[] platforms;
    public bool isGrounded;
    public int numJumps;

    RaycastHit hitInfo;
    bool speedBoost;
    Vector3 velocity;
    int jumpsLeft;

    // Use this for initialization
    void Start () {
        jumpsLeft = numJumps;

	}

	// Update is called once per frame
	void LateUpdate () {
        float movement = Input.GetAxis("Horizontal");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        Vector3 newPos = new Vector3(transform.position.x + movement * speed, transform.position.y, 0f);
        transform.position = newPos;
        checkForPlatform();
        Jump();
	}

    void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space)&&jumpsLeft>0)
        {
            jumpsLeft--;
            isGrounded = false;
            velocity.y = jumpHeight;
            //Debug.Log("weeenr");
        }

        if (isGrounded)
        {
            velocity.y = 0;
            jumpsLeft = numJumps;
        }
        else
        {
            velocity.y -= gravity;
        }
        checkForPlatform();

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
                && gameObject.transform.position.x +  gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f > p.transform.position.x - p.GetComponent<SpriteRenderer>().size.x * 0.5f
                && gameObject.transform.position.y -gameObject.GetComponent<SpriteRenderer>().size.x *0.5f < p.transform.position.y + p.GetComponent<SpriteRenderer>().size.y *0.5f
                && gameObject.transform.position.y + gameObject.GetComponent<SpriteRenderer>().size.x * 0.5f > p.transform.position.y - p.GetComponent<SpriteRenderer>().size.y * 0.5f)
            {
                isPlatform = true;
            }
        }*/


        Ray rae = new Ray(transform.position, Vector3.down * collDist);
        //Draw ray on screen to see visually. Remember visual length is not actual length.
        Debug.DrawRay(transform.position, Vector3.down * collDist, Color.yellow);
        if (Physics.Raycast(rae, out hitInfo, gameObject.GetComponent<Collider2D>().bounds.size.y*0.5f))
        {
			 // Debug.Log("BIG WEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEn");
            print("Collided With " + hitInfo.collider.gameObject.name);
            // Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
            isGrounded = true;
            //Vector3 newPos = new //Vector3(transform.position.x,hitInfo.transform.position.y+gameObject.GetComponent<Collider2D>().bounds.size.y+0.002f, 0f);
				Vector3 newPos = new Vector3(transform.position.x,hitInfo.transform.position.y+hitInfo.collider.bounds.size.y*0.5f+gameObject.GetComponent<Collider2D>().bounds.size.y*0.5f+0.002f, 0f);
				//Debug.Log("New x: " +transform.position.x+ " New y: " +transform.position.y+ "New Z: " +transform.position.z);
            transform.position = newPos;
        }
        else
        {
            isGrounded = false;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //isGrounded = true;
    }

}
