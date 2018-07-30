using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraMovement : MonoBehaviour {
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 smoothedPosition;

    void Start() {
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z) + offset;
    }

	void LateUpdate() {
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, target.position.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
	}
}
