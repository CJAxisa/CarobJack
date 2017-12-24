using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{
	public float MovingForce;
	Collider col;
	Vector3 StartPoint;
	Vector3 Origin;
	public int NoOfRays = 10;
	int i;
	RaycastHit HitInfo;
	float LengthOfRay, DistanceBetweenRays, DirectionFactor;
	float margin = 0.015f;
	Ray ray;

	void Start ()
	{
		//Length of the Ray is distance from center to edge
		LengthOfRay = (GetComponent<Collider>().bounds.max.x/((GetComponent<Collider>().bounds.max.x)*10))/2;
		print("Length of ray = " + LengthOfRay + " (the smaller the ray, the closer our enemy will collide with the wall)");
		//Initialize DirectionFactor for right direction
		DirectionFactor = Mathf.Sign (Vector3.right.z);
		col = GetComponent<Collider>();
	}

	void Update ()
	{
		//Debug.Log("half of the boundary is at y = " + (col.bounds.max.y + col.bounds.min.y)*0.5f);
		StartPoint = new Vector3 (GetComponent<Collider>().bounds.min.x + margin, (col.bounds.max.y + col.bounds.min.y)*0.5f, transform.position.z);
		if (!IsCollidingHorizontally ()) {
			transform.Translate (Vector3.right * MovingForce * Time.deltaTime * DirectionFactor);
		}
	}

	bool IsCollidingHorizontally ()
	{
		Origin = StartPoint;
		DistanceBetweenRays = (GetComponent<Collider>().bounds.size.x - 2 * margin) / (NoOfRays - 1);
		for (i = 0; i<NoOfRays; i++) {
			// Ray to be casted.
			ray = new Ray (Origin, Vector3.right * DirectionFactor);
			//Draw ray in scene view to see visually. Remember visual length is not actual length
			Debug.DrawRay (Origin, Vector3.right * DirectionFactor, Color.yellow);
			if (Physics.Raycast (ray, out HitInfo, LengthOfRay)) {
				print ("Collided With " + HitInfo.collider.gameObject.name + "; " + HitInfo.distance + " away from gameobject");
				// Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
				DirectionFactor = -DirectionFactor;
				return true;
			}
			Origin += new Vector3 (DistanceBetweenRays, 0, 0);
		}
		return false;

	}


}
