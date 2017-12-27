using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* This handles the player's health and check for enemy collision
 *  This is attached to the player gameObject
 * 
 * 
 */
public class Player : MonoBehaviour {
    public int health;
    private GameObject[] enemies;
	// Use this for initialization
	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	// Update is called once per frame
	void Update () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        checkForEnemies();
	}

    private void loseHealth(int numHealth) {
        health -= numHealth;
    }

    private void hit()
    {
        Debug.Log("HYUCK");
    }

    private void checkForEnemies()
    {
        /*
        //goes through each enemy and does AABB (Axis Aligned Bounding Box) to check ffor collisions.
        foreach (GameObject enem in enemies)
        {
            /*
             * This will check if;
             * box B's x min is less than box a's x max
             * box B's x max is greater than box a's x min
             * box B's y min is less than box a's y max
             * box B's y max is greater than box a's y min
             * 
             * if all of these are true, then we have a collision
             
            if (gameObject.GetComponent<Collider2D>().bounds.min.x < enem.GetComponent<Collider2D>().bounds.max.x &&
                gameObject.GetComponent<Collider2D>().bounds.min.x > enem.GetComponent<Collider2D>().bounds.max.x &&
                gameObject.GetComponent<Collider2D>().bounds.min.y > enem.GetComponent<Collider2D>().bounds.max.y &&
                gameObject.GetComponent<Collider2D>().bounds.min.y < enem.GetComponent<Collider2D>().bounds.max.y
               )
            {
                Debug.Log(enem);

            }
        }
    */
    }

    public void OnCollisionEnter(Collision co)
    {
        Debug.Log("AAAAAAAA");
        if (co.gameObject.CompareTag("Enemy")) 
            loseHealth(1);
    }
}
