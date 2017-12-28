using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{
	public float MovingForce;
	Collider2D col;

    private float LengthOfRay;
    int i;
	RaycastHit HitInfo;
	float  DirectionFactor;
	float margin = 0.015f;
	Ray ray;

	void Start ()
	{
		//Length of the Ray is distance from center to edge
		//LengthOfRay = (GetComponent<Collider2D>().bounds.max.x/((GetComponent<Collider2D>().bounds.max.x)*10))/2;
		LengthOfRay = GetComponent<Collider2D>().bounds.extents.x;
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
			/* original ray origin via Nick below */
			Vector3 origin = new Vector3((col.bounds.min.x + col.bounds.max.x) * 0.5f, (col.bounds.min.y + col.bounds.max.y) * 0.5f, transform.position.z);
			ray = new Ray (origin, Vector3.right * DirectionFactor);
			/* original ray origin via Nick above */

			/* CJ's way of casting the ray below*/
			//CJ: ray = new Ray (transform.position, Vector3.right * DirectionFactor);
			/* CJ's way of casting the ray above*/

			//Draw ray in scene view to see visually. Remember visual length is not actual length
			//CJ: Debug.DrawLine(origin,transform.position+Vector3.right * LengthOfRay, Color.yellow);
			Debug.DrawRay (origin, Vector3.right * DirectionFactor, Color.yellow);

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
