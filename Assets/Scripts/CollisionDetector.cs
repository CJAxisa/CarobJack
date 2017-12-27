using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{
	public float MovingForce;
	Collider2D col;
	
    public float LengthOfRay;
    int i;
	RaycastHit HitInfo;
	float  DirectionFactor;
	float margin = 0.015f;
	Ray ray;

	void Start ()
	{
		//Length of the Ray is distance from center to edge
		//LengthOfRay = (GetComponent<Collider2D>().bounds.max.x/((GetComponent<Collider2D>().bounds.max.x)*10))/2;
		//print("Length of ray = " + LengthOfRay + " (the smaller the ray, the closer our enemy will collide with the wall)");
		//Initialize DirectionFactor for right direction
		DirectionFactor = Mathf.Sign (Vector3.right.z);
		col = GetComponent<Collider2D>();
	}

	void Update ()
	{
		//Debug.Log("half of the boundary is at y = " + (col.bounds.max.y + col.bounds.min.y)*0.5f);
		if (!IsCollidingHorizontally ()) {
			transform.Translate (Vector3.right * MovingForce * Time.deltaTime * DirectionFactor);
		}
	}

	bool IsCollidingHorizontally ()
	{
		
			// Ray to be casted.
			ray = new Ray (transform.position, Vector3.right * DirectionFactor);
			//Draw ray in scene view to see visually. Remember visual length is not actual length
			Debug.DrawLine(transform.position,transform.position+Vector3.right * LengthOfRay);
			if (Physics.Raycast (ray, out HitInfo, LengthOfRay)) {
				//  print ("Collided With " + HitInfo.collider.gameObject.name + "; " + HitInfo.distance + " away from gameobject");
				// Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
				DirectionFactor = -DirectionFactor;
				return true;
			}
		
		return false;
	}
	/* */
	void OnTriggerEnter2D(Collider2D other) {

	}

}
