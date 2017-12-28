using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class is attached to the main camera in the scene
 * 
 * This class analyzes the player's Viewport position and adjusts the camera to follow the player accordingly
 * The class will also change the scene to the gameOverScene once the player has died
 * 
 * 
 */
public class CameraBehavior : MonoBehaviour {
    private GameObject player;
    private Camera mainCamera;

    private int screenWidth;
    private int screenHeight;

    private Vector3 velocity;

    public float maxSpeed;

    // Use this for initialization
    void Start () {
        mainCamera = gameObject.GetComponent<Camera>();
        screenWidth = mainCamera.pixelWidth;
        screenHeight = mainCamera.pixelHeight;
    }
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            //This means the player is dead and we should cut to the game over scene
        }

        Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(player.transform.position);
        Vector3 newPos = transform.position;
        if (velocity == Vector3.zero)
        {
            if (playerScreenPos.x > screenWidth * 0.6)
            {
                //newPos.x += playerScreenPos
            }
            else if (playerScreenPos.x < screenWidth * 0.2)
            {
                newPos.x -= 0.5f;
            }

            if (playerScreenPos.y > screenHeight * 0.6)
            {
                newPos.y += 0.5f;
            }
            else if (playerScreenPos.y < screenHeight * 0.4)
            {
                newPos.y -= 0.5f;
            }
            velocity = newPos - transform.position;

        }

        if (velocity !=Vector3.zero)
        {
            velocity *= maxSpeed / velocity.magnitude;        
            transform.Translate(velocity);
            velocity = Vector3.zero;
        }

    }
}
