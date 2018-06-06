using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeManager : MonoBehaviour {
    public enum Tomes
    {
        Empty,
        Fireball,
        Lightning,
        HighJump,
        Float,
        MagicMissile,
        Lazer,
        Dash,
        SpeedUp,
        PowerUp,
        Stun,
    }

    public Tomes firstTome;
    public Tomes secondTome;
    public Tomes thirdTome;

    public GameObject tomePrefab;

    public GameObject firstTomeObj;
    public GameObject secondTomeObj;
    public GameObject thirdTomeObj;

    GameObject player;

    // Use this for initialization
    void Start () {
        firstTome = Tomes.Fireball;
        secondTome = Tomes.Dash;
        thirdTome = Tomes.Float;

        createTomes();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void createTomes()
    {
        //nullcheck
        if (firstTomeObj != null)
            Destroy(firstTomeObj);
        if (secondTomeObj != null)
            Destroy(secondTomeObj);
        if (thirdTomeObj != null)
            Destroy(thirdTomeObj);


        Vector3 firstTomePosition = transform.position + new Vector3(-1f, 1f, 0f);
        firstTomeObj = Instantiate(tomePrefab, firstTomePosition, Quaternion.identity);
        firstTomeObj.GetComponent<Tome>().maxSpeed += Random.Range(-0.01f, 0.01f);

        Vector3 secondTomePosition = transform.position + new Vector3(-1.23f, 2.4f, 0f);
        secondTomeObj = Instantiate(tomePrefab, secondTomePosition, Quaternion.identity);
        secondTomeObj.GetComponent<Tome>().maxSpeed += Random.Range(-0.01f, 0.01f);

        Vector3 thirdTomePosition = transform.position + new Vector3(0.1f, 2.5f, 0f);
        thirdTomeObj = Instantiate(tomePrefab, thirdTomePosition, Quaternion.identity);
        thirdTomeObj.GetComponent<Tome>().maxSpeed += Random.Range(-0.01f, 0.01f);

    }
}
