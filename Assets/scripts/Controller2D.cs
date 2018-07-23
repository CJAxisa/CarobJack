using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public LayerMask collisionMask;

	const float skinWidth = .015f;		// We choose some arbitrarily small number
	public int horizontalRayCount = 4;		// The number of rays that are fired horizontally
	public int verticalRayCount = 4;			// The number of rays that are fired vertically
  public int diagonalRayCount = 4;

  float maxClimbAngle = 80;

	float horizontalRaySpacing;			// the distance between each horizontal ray
	float verticalRaySpacing;				// The distance between each vertical ray

	BoxCollider2D boxCollider;					// This will hold our player's BoxCollider2D
	RaycastOrigins raycastOrigins;	// See the struct that we have created at the bottom of the file
	public CollisionInfo collisions;				// See the struct that we have created at the bottom of the file

	void Start() {
		boxCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing ();
	}

	public void Move(Vector3 velocity) {
		UpdateRaycastOrigins ();
		collisions.Reset();			// We will set our collisions in HorizontalCollision and VerticalCollision so we can reset this now

		// Here we handle horizontal and vertical collisions
		// We only want to handle horizontal and vertical collisions if we are moving horizontally or vertically (i.e. velocity.x != 0 and velocity.y != 0 respectively)
		if(velocity.x != 0) {
			HorizontalCollision(ref velocity);
		}
		if(velocity.y != 0) {
			VerticalCollision(ref velocity); // We are passing in a reference to our velocity
		}
    if(velocity.x < 0 && velocity.y != 0) {
      DiagonalCollisionLeft(ref velocity);
    }
    else if(velocity.x > 0 && velocity.y != 0) {
      DiagonalCollisionRight(ref velocity);
    }

		transform.Translate (velocity);
	}

	void HorizontalCollision(ref Vector3 velocity) {
		float directionX = Mathf.Sign(velocity.x); 						// If we are moving left, directionX = -1, else directionX = 1
		float rayLength = Mathf.Abs(velocity.x) + skinWidth; 	// force velocity.x to be positive

		for(int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;		// If we are moving left then we want our ray to start in the bottom left corner, else we are moving right and we set ray origin to bottom right
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);		//TODO: figure out why we add velocity.x
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);		// Here we are creating a raycast which starts at rayOrigin, shoots out in the direction Vector2.right * directionX, has a length of rayLength, and only collides with objects with a LayerMask equal to collisionMask

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			// If our raycast hits something then the first thing we want to do is set our x velocity to the amount we have to move to get from our current position to the point at which the ray intersected with an obstacle; essentially the ray distance
			if(hit) {
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

        if(i == 0 && slopeAngle <= maxClimbAngle) { // this only tests slope against the lowest ray
           float distanceToSlopeStart = 0;
           if(slopeAngle != collisions.slopeAngleOld) {
             distanceToSlopeStart = hit.distance - skinWidth;
             velocity.x -= distanceToSlopeStart * directionX;
           }
           ClimbSlope(ref velocity, slopeAngle);
           velocity.x += distanceToSlopeStart * directionX;
        }

        if(!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
				    velocity.x = (hit.distance - skinWidth) * directionX;
				    rayLength = hit.distance; // we change our ray length to the distance between our raycast origin and the object

            if(collisions.climbingSlope) {
              velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
            }

				    collisions.left = directionX == -1; // As mentioned on line 44, if we are moving left then directionX = -1
				    collisions.right = directionX == 1; // ^^
        }
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

        if(collisions.climbingSlope) {
          velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
        }

				collisions.below = directionY == -1; // As mentioned on line 67, if we are moving down then directionY = -1
				collisions.above = directionY == 1;	 // ^^
			}
		}
	}

  void DiagonalCollisionLeft(ref Vector3 velocity) {
    Vector3 rayOrigin = Vector3.zero;
    if (velocity.x < 0 && velocity.y < 0) {
      rayOrigin = raycastOrigins.bottomLeft;
    }
    else if (velocity.x < 0 && velocity.y > 0) {
      rayOrigin = raycastOrigins.topLeft;
    }

    float slope = Mathf.Abs(velocity.x) / Mathf.Abs(velocity.y);
    float diagonalSkinWidth;
    if (slope <= 1) {
      diagonalSkinWidth = new Vector2(skinWidth * slope, skinWidth).magnitude;
    }
    else {
      diagonalSkinWidth = new Vector2(skinWidth, (1 / slope) * skinWidth).magnitude;
    }

    float rayLength = velocity.magnitude + diagonalSkinWidth;
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, velocity, rayLength, collisionMask);

    Debug.DrawRay(rayOrigin, velocity.normalized * rayLength, Color.white);
    if (hit) {
      velocity = velocity.normalized * (hit.distance - diagonalSkinWidth);
    }
  }

  void DiagonalCollisionRight(ref Vector3 velocity) {
    Vector3 rayOrigin = Vector3.zero;
    if (velocity.x > 0 && velocity.y < 0) {
      rayOrigin = raycastOrigins.bottomRight;
    }
    else if (velocity.x > 0 && velocity.y > 0) {
      rayOrigin = raycastOrigins.topRight;
    }

    float slope = Mathf.Abs(velocity.x) / Mathf.Abs(velocity.y);
    float diagonalSkinWidth;
    if (slope <= 1) {
      diagonalSkinWidth = new Vector2(skinWidth * slope, skinWidth).magnitude;
    }
    else {
      diagonalSkinWidth = new Vector2(skinWidth, (1 / slope) * skinWidth).magnitude;
    }

    float rayLength = velocity.magnitude + diagonalSkinWidth;
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, velocity, rayLength, collisionMask);

    Debug.DrawRay(rayOrigin, velocity.normalized * rayLength, Color.white);
    if (hit) {
      velocity = velocity.normalized * (hit.distance - diagonalSkinWidth);
    }
  }

  void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
    float moveDistance = Mathf.Abs(velocity.x);
    float climbVelocityY = Mathf.Sin(slopeAngle*Mathf.Deg2Rad) * moveDistance;

    if(velocity.y <= climbVelocityY) {
      velocity.y = Mathf.Sin(slopeAngle*Mathf.Deg2Rad) * moveDistance;
      velocity.x = Mathf.Cos(slopeAngle*Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
      collisions.below = true; // allows player to jump on a slope
      collisions.climbingSlope = true;
      collisions.slopeAngle = slopeAngle;
    }
  }

	void UpdateRaycastOrigins() {
		Bounds bounds = boxCollider.bounds;
		// So that the bounds are shrunken inward:
		bounds.Expand(skinWidth * -2); // Causes the origin of our rays to be slightly INSIDE of the players box boxCollider which is good because (?)

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
		Bounds bounds = boxCollider.bounds;
		// So that the bounds are shrunken inward:
		bounds.Expand(skinWidth * -2); // Causes the origin of our rays to be slightly INSIDE of the players box boxCollider which is good because (?)

		// This will make sure that at least 2 rays are being fired:
		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);	// horizontally
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);			// vertically
    diagonalRayCount = Mathf.Clamp(diagonalRayCount, 2, int.MaxValue);

		// This will evenly distribute rays:
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);		// horizontally by dividing the BoxCollider2D's y axis by the total number of horizontal rays being fired - 1
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);				// vertically by dividing the BoxCollider2D's x-axis by the total number of vertical rays being fired - 1
	}

	// Purpose: to hold the origin vectors for each vertex of the BoxCollider2D
	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

    public bool climbingSlope;
    public float slopeAngle, slopeAngleOld;

		public void Reset() {
			above = below = left = right = false;
      climbingSlope = false;
      slopeAngleOld = slopeAngle;
      slopeAngle = 0;
		}
	}
}
