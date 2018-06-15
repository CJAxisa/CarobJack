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
        Stun
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

        //checks if the tomes are active and if they should be
        if (firstTome == Tomes.Empty)
            firstTomeObj.SetActive(false);
        else if(firstTome != Tomes.Empty && firstTomeObj.activeInHierarchy ==false)
            firstTomeObj.SetActive(true);

        if (secondTome == Tomes.Empty)
            secondTomeObj.SetActive(false);
        else if (secondTome != Tomes.Empty && secondTomeObj.activeInHierarchy == false)
            secondTomeObj.SetActive(true);

        if (thirdTome == Tomes.Empty)
            thirdTomeObj.SetActive(false);
        else if (thirdTome != Tomes.Empty && thirdTomeObj.activeInHierarchy == false)
            thirdTomeObj.SetActive(true);

        switch (firstTome)
        {
            case Tomes.Empty:
                break;
            case Tomes.Fireball:
                //gameObject.GetComponent<FireTome>().enabled = true;
                break;
            case Tomes.Lightning:
                break;
            case Tomes.HighJump:
                break;
            case Tomes.Float:
                break;
            case Tomes.MagicMissile:
                break;
            case Tomes.Lazer:
                break;
            case Tomes.Dash:
                break;
            case Tomes.SpeedUp:
                break;
            case Tomes.PowerUp:
                break;
            case Tomes.Stun:
                break;
            default:
                break;
        }
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


        //Instaniates tome game objects and slightly randomizes their following speed to make them seem more natural
        Vector3 firstTomePosition = transform.position + new Vector3(-1f, 1f, 0f);
        firstTomeObj = Instantiate(tomePrefab, firstTomePosition, Quaternion.identity);
        firstTomeObj.GetComponent<TomeFollower>().maxSpeed += Random.Range(-0.001f, 0.001f);

        Vector3 secondTomePosition = transform.position + new Vector3(-1.23f, 2.4f, 0f);
        secondTomeObj = Instantiate(tomePrefab, secondTomePosition, Quaternion.identity);
        secondTomeObj.GetComponent<TomeFollower>().maxSpeed += Random.Range(-0.001f, 0.001f);

        Vector3 thirdTomePosition = transform.position + new Vector3(0.1f, 2.5f, 0f);
        thirdTomeObj = Instantiate(tomePrefab, thirdTomePosition, Quaternion.identity);
        thirdTomeObj.GetComponent<TomeFollower>().maxSpeed += Random.Range(-0.010f, 0.010f);

    }
}
