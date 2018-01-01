using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/* This handles the player's health and check for enemy collision
 *  This is attached to the player gameObject
 *
 *
 */
public class Player : MonoBehaviour {
    public int health;
    private GameObject[] enemies;
    private GameObject[] lava;
    public float timer;
  	public float delay;
    bool Dot;
	// Use this for initialization
	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        lava = GameObject.FindGameObjectsWithTag("Lava");
	}

	// Update is called once per frame
	void Update () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        lava = GameObject.FindGameObjectsWithTag("Lava");
        checkForEnemies();
        if (health <= 0)
        {
            die();
            SceneManager.LoadScene(0);
        }
        if (Dot){
          if (timer > delay){
            Dot = false;
            // timer = 0;
          }
          timer += 1.0f * Time.deltaTime * 5;
          Debug.Log("Timer = " + timer);
          //TODO: lose health every second
          if (timer >= delay - 0.5 && timer <= delay){
            loseHealth(1);
            timer = 0;
          }
        }

	}

    public void die()
    {
        Destroy(gameObject);
    }
    private void loseHealth(int numHealth) {
        health -= numHealth;
    }

    private void knockBack()
    {
        Debug.Log("we out here");
        if(gameObject.GetComponent<Mover>().facingRight)
            gameObject.GetComponent<Mover>().knockbackFactor = new Vector3(-0.5f, 0.2f, 0f);
        else
            gameObject.GetComponent<Mover>().knockbackFactor = new Vector3(0.5f, 0.2f, 0f);
        gameObject.GetComponent<Mover>().knockback();
    }

    public void getHit()
    {
        Debug.Log("HYUCK");
        knockBack();
        loseHealth(1);
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

    public void OnTriggerStay2D(Collider2D co)
    {
        // Debug.Log("AAAAAAAA");
        // if (co.gameObject.CompareTag("Enemy")){
        //     loseHealth(1);
          // }
        if (co.gameObject.CompareTag("Lava")){
            Dot = true;
            // loseHealth(1);
            // knockBack();
        }
    }
    public void OnTriggerExit2D(Collider2D co){
      if (co.gameObject.CompareTag("Lava")){
          Dot = false;
          timer = 0;
        }
      }
}
