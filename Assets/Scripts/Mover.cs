using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed;
    public float jumpHeight;
    public float gravity;
    public float collDist;
    public GameObject[] platforms;
    public bool isGrounded;

    RaycastHit hitInfo;
    bool speedBoost;
    Vector3 velocity;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float movement = Input.GetAxis("Horizontal");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        Vector3 newPos = new Vector3(transform.position.x + movement * speed, transform.position.y, 0f);
        transform.position = newPos;
        Jump();
	}

    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            velocity.y += jumpHeight;
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y+0.09f, 0f);
            transform.position = newPos;
            Debug.Log("weeenr");
        }

        if (isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= gravity;
        }
        checkForPlatform();
        
        
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
        if (Physics.Raycast(rae, out hitInfo, gameObject.GetComponent<Collider2D>().bounds.extents.y*2f))
        {
            print("Collided With " + hitInfo.collider.gameObject.name);
            // Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
            isGrounded = true;            
        }

      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //isGrounded = true;
    }

}
