using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class CharacterController2D : MonoBehaviour {

	public LayerMask collisionMask;

	private const float skinWidth = .015f;		// We choose some arbitrarily small number
	public int horizontalRayCount = 4;		// The number of rays that are fired horizontally
	public int verticalRayCount = 4;			// The number of rays that are fired vertically

	private float horizontalRaySpacing;			// the distance between each horizontal ray
	private float verticalRaySpacing;				// The distance between each vertical ray

	private BoxCollider2D collider;					// This will hold our player's BoxCollider2D
	private RaycastOrigins raycastOrigins;	// See the struct that we have created at the bottom of the file
	public CollisionInfo collisions;				// See the struct that we have created at the bottom of the file
	public Vector3 knockbackFactor;					// This will be modified in the Player class and handles knockback

	void Start() {
		knockbackFactor = new Vector3(0f, 0f, 0f);
		collider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing ();
	}

	public void Move(Vector3 velocity) {
		UpdateRaycastOrigins ();
		collisions.Reset();			// We will reset our collisions in HorizontalCollision and VerticalCollision so we can reset this now

		// Here we handle horizontal and vertical collisions
		// We only want to handle horizontal and vertical collisions if we are moving horizontally or vertically (i.e. velocity.x != 0 and velocity.y != 0 respectively)
		knockback();
		if(velocity.x != 0) {
			HorizontalCollision(ref velocity);		// We are passing in a reference to our velocity
		}
		if(velocity.y != 0) {
			VerticalCollision(ref velocity);			// We are passing in a reference to our velocity
		}

		transform.Translate (velocity);					// This is where we actually move our player based on the velocity vector
		// Debug.Log(velocity);
	}

	void HorizontalCollision(ref Vector3 velocity) {
		float directionX = Mathf.Sign(velocity.x); 						// If we are moving left, directionX = -1, else directionX = 1
		float rayLength = Mathf.Abs(velocity.x) + skinWidth; 	// force velocity.x to be positive

		for(int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;		// If we are moving left then we want our ray to start in the bottom left corner, else we are moving right and we set ray origin to bottom right
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);		// This will change which ray we are dealing with based on the current iteration (i.e. if i = 0 then we are dealing with the first ray which should start at bottom left corner or bottom right corner)
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);		// Here we are creating a raycast which starts at rayOrigin, shoots out in the direction Vector2.right * directionX, has a length of rayLength, and only collides with objects with a LayerMask equal to collisionMask

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			// If our raycast hits something then the first thing we want to do is set our x velocity to the amount we have to move to get from our current position to the point at which the ray intersected with an obstacle; essentially the ray distance
			if(hit) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance; // we change our ray length to the distance between our raycast origin and the object

				collisions.left = directionX == -1; // As mentioned on line 44, if we are moving left then directionX = -1
				collisions.right = directionX == 1; // ^^
			}
		}
	}


	void VerticalCollision(ref Vector3 velocity) {
		float directionY = Mathf.Sign(velocity.y); 						// If we are moving down, directionY = -1, else directionY = 1
		float rayLength = Mathf.Abs(velocity.y) + skinWidth; 	// force velocity.y to be positive

		for(int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;		// If we are moving down then we want our ray to start in the bottom left corner, else we are moving up and we set ray origin to top left
			rayOrigin += Vector2.right * (verticalRaySpacing * i);		//TODO: figure out why we add velocity.x
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);		// Here we are creating a raycast which starts at rayOrigin, shoots out in the direction Vector2.up * direction.Y, has a length of rayLength, and only collides with objects with a LayerMask equal to collisionMask

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			// If our raycast hits something then the first thing we want to do is set our y velocity to the amount we have to move to get from our current position to the point at which the ray intersected with an obstacle; essentially the ray distance
			if(hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance; // we change our ray length to the distance between our raycast origin and the object

				collisions.below = directionY == -1; // As mentioned on line 67, if we are moving down then directionY = -1
				collisions.above = directionY == 1;	 // ^^
			}
		}
	}

	void UpdateRaycastOrigins() {
		Bounds bounds = collider.bounds;
		// So that the bounds are shrunken inward:
		bounds.Expand(skinWidth * -2); // Causes the origin of our rays to be slightly INSIDE of the players box collider which is good because (?)

		// So if you think of a box, this will represent the raycast origin:
		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);	// from the bottom left vertex
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y); // from the bottom right vertex
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y); 		// from the top left vertex
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);		// from the top right vertex


	}

	/* Purpose:
	1. to make sure at least 2 rays are being fired
	2. to make sure the origin of each horizontal/vertical ray is evenly distributed along the BoxCollider2D's x and y axis
	*Note*
	vertical rays run perpendicular to the x-axis:

											------ (x-axis of BoxCollider2D)
											|    |
											|		 |
											V		 V
				(verical_ray01)	   (vertical_ray02)

	horizontal rays runs perpendicular to the y-axis:

											|-------> horizontal_ray01
											|
											|
											|-------> horizontal_ray02
										y-axis of BoxCollider2D
	*/
	void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		// So that the bounds are shrunken inward:
		bounds.Expand(skinWidth * -2); // Causes the origin of our rays to be slightly INSIDE of the players box collider which is good because it leads to less jittery collisions & stops the player from randomly falling through an object

		// This will make sure that at least 2 rays are being fired:
		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);	// horizontally
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);			// vertically

		// This will evenly distribute rays:
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);		// horizontally by dividing the BoxCollider2D's y axis by the total number of horizontal rays being fired - 1
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);				// vertically by dividing the BoxCollider2D's x-axis by the total number of vertical rays being fired - 1
	}

	public void knockback() {
		transform.Translate(knockbackFactor);
		//knockbackFactor *= 0.95f;
		knockbackFactor *= 0.9f;
		if(knockbackFactor.x != 0) {
			HorizontalCollision(ref knockbackFactor);		// We are passing in a reference to our velocity
		}
		if(knockbackFactor.y != 0) {
			VerticalCollision(ref knockbackFactor);			// We are passing in a reference to our velocity
		}
	}

	// Purpose: to hold the origin vectors for each vertex of the BoxCollider2D
	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	// Purpose: basically tells us if we are colliding with walls, ceilings, or if we are grounded
	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public void Reset() {
			above = below = left = right = false;
		}
	}
}
