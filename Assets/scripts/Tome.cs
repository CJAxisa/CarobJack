using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tome : MonoBehaviour {
    public Vector3 targetLocation;
    public Vector3 offset;
    public Vector3 velocity;

    public GameObject player;

    public float maxSpeed;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        targetLocation = player.transform.position;
        offset = player.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (player.GetComponent<Player>().facingRight == true)
            targetLocation = player.transform.position - offset;
        else
            targetLocation = player.transform.position - new Vector3(-offset.x, offset.y, 0f);


        velocity = targetLocation - transform.position;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        //Vector3.
        //transform.Translate(velocity);
        transform.position = transform.position + velocity;

	}
}
