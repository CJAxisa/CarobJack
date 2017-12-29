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

    private float maxSpeed;
    private float speed;
    private float gravity;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        mainCamera = gameObject.GetComponent<Camera>();

        screenWidth = mainCamera.pixelWidth;
        screenHeight = mainCamera.pixelHeight;

        speed = player.GetComponent<Mover>().speed;
        maxSpeed = player.GetComponent<Mover>().maxSpeed;
        gravity = player.GetComponent<Mover>().gravity;

    }

    // Update is called once per frame
    void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            //This means the player is dead and we should cut to the game over scene
        }
        Vector3 playerScreenPos = mainCamera.WorldToScreenPoint(player.transform.position);
        velocity = Vector3.zero; 

        if (playerScreenPos.x > screenWidth * 0.6)
        {
            //transform.parent=player.transform;
            float movement = Mathf.Abs(Input.GetAxis("Horizontal"));
            velocity.x = movement * speed;

        }
        else if (playerScreenPos.x < screenWidth * 0.35)
        {
            //velocity.x += (float)(screenWidth * 0.2) -playerScreenPos.x;
            float movement = -Mathf.Abs(Input.GetAxis("Horizontal"));
            velocity.x = movement * speed;
        }

        if (playerScreenPos.y > screenHeight * 0.6)
        {
            //transform.parent=player.transform;
            velocity.y = 0.1f;

        }
        else if (playerScreenPos.y < screenHeight * 0.4)
        {
            velocity.y = -maxSpeed*Time.deltaTime;
        }
        transform.Translate(velocity);

    }
}
