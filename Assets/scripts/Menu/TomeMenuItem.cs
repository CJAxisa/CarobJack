using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeMenuItem : MonoBehaviour {

    public bool grabbed;
    public string tomeName;
    private bool grabbedPrev;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 startingPos;
    private TomeManager playerTM;

    // Use this for initialization
    void Start () {
        playerTM = GameObject.FindGameObjectWithTag("Player").GetComponent<TomeManager>();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (grabbed)
            transform.position = Input.mousePosition;
        if (grabbedPrev != grabbed && !grabbed)
            transform.position = startingPos;

        grabbedPrev = grabbed;
    }


    public void setGrabbed()
    {
        if (grabbed)
            grabbed = false;
        else
            grabbed = true;
    }

    //returns the Tome slot that the item is placed in
    private int checkPos()
    {
        //first check the y co-ordinates since all tome slots have same y co-ordinates
        if (transform.position.y > -126.2f && transform.position.y < -4.59f)
        {
            //check x for tome 1
            if (transform.position.x > -27.1f && transform.position.x < 37.8f)
                return 1;
            //check x for tome 2
            if (transform.position.x > 70f && transform.position.x < 135.5f)
                return 2;
            //check x for tome 3
            if (transform.position.x > 170f && transform.position.x < 234.8f)
                return 3;
            else
                return 0;
        }
        else
            return 0;

        
    }
}
