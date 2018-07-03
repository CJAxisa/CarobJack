using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraMovement : MonoBehaviour {
	public Transform target;

  public float cameraSpeed;
  public float smoothSpeed = 0.125f;
  public float allowableDistance;
  public Vector3 offset;
  private Vector3 smoothedPosition;

  void Start() {
    transform.position = new Vector3(target.position.x, target.position.y, target.position.z) + offset;
  }

	void LateUpdate() {
    Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, target.position.z) + offset;
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    transform.position = smoothedPosition;
    //if(transform.position.x < target.position.x - allowableDistance) {
    //   transform.position += Vector3.right * cameraSpeed * Time.deltaTime;
    //}
    // else if(transform.position.x > target.position.x + allowableDistance) {
    //   transform.position -= Vector3.right * cameraSpeed * Time.deltaTime;
    // }
    // if(transform.position.y > target.position.y + allowableDistance) {
    //   transform.position -= Vector3.up * cameraSpeed * Time.deltaTime;
    // }
    // else if(transform.position.y < target.position.y - allowableDistance) {
    //   transform.position += Vector3.up * cameraSpeed * Time.deltaTime;
    // }
	}


}
