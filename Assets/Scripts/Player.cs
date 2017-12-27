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
        //goes through each enemy and does AABB (Axis Aligned Bounding Box) to check ffor collisions.
        foreach (GameObject enem in enemies)
        {
            
            if (transform.position.x - gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f < enem.transform.position.x + enem.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f &&
                transform.position.x + gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f > enem.transform.position.x - enem.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f &&
                transform.position.y + gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f > enem.transform.position.y - enem.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f &&
                transform.position.y - gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f < enem.transform.position.y + enem.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f
               )
            {
                Debug.Log(enem);

            }
        }
    }

    public void OnCollisionEnter2D(Collision2D co)
    {
        Debug.Log("AAAAAAAA");
        if (co.gameObject.CompareTag("Enemy")) 
            loseHealth(1);
    }
}
