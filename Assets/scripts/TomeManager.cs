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

    //all of these are public for the time being for the ease of playtesting
    public Tomes firstTome;
    public Tomes secondTome;
    public Tomes thirdTome;

    public Tomes[,] tomePresets;
    public int currentPreset;           //starts at 0

    public GameObject tomePrefab;

    public GameObject firstTomeObj;
    public GameObject secondTomeObj;
    public GameObject thirdTomeObj;

    GameObject player;

    //gui stuff
    public Texture2D tomeSelect;
    public Texture2D fireEmblem;
    public Texture2D floatEmblem;
    public Texture2D lightningEmblem;

    Rect tomeSelectPos;
    Rect firstTomePos;
    Rect secondTomePos;
    Rect thirdTomePos;

    // Use this for initialization
    void Start () {
        firstTome = Tomes.Fireball;
        secondTome = Tomes.Lightning;
        thirdTome = Tomes.Float;

        currentPreset = 0;
        tomePresets = new Tomes[4,3];

        for (int i = 0; i < tomePresets.GetUpperBound(0); i++)
        {
            for (int j = 0; j < tomePresets.GetUpperBound(1); j++)
            {
                tomePresets[i, j] = Tomes.Empty;
            }
        }

        createTomes();
        
        tomeSelectPos = new Rect(Screen.width / 2f - 97f, Screen.height - 64f, 193f, 64f);
        firstTomePos = new Rect(Screen.width / 2f - 95f, Screen.height - 64f, 62f, 62f);
        secondTomePos = new Rect(Screen.width / 2f - 95f +62f, Screen.height - 64f, 62f, 62f);
        thirdTomePos = new Rect(Screen.width / 2f + 29f, Screen.height - 64f, 62f, 62f);

    }

    // Update is called once per frame
    void Update () {


        checkForInput();
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

    public void checkForInput()
    {
        if(Input.mouseScrollDelta.y>=1)
        {
            if (currentPreset == 0)
                currentPreset = tomePresets.GetUpperBound(0);
            else
                currentPreset--;
        }
        else if (Input.mouseScrollDelta.y <= -1)
        {
            if (currentPreset == tomePresets.GetUpperBound(0))
                currentPreset = 0;
            else
                currentPreset++;
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

    private void OnGUI()
    {
        GUI.DrawTexture(tomeSelectPos,tomeSelect);

        switch (tomePresets[currentPreset,0])
        {
            case Tomes.Empty:
                break;
            case Tomes.Fireball:
                GUI.DrawTexture(firstTomePos, fireEmblem);
                break;
            case Tomes.Lightning:
                GUI.DrawTexture(firstTomePos, lightningEmblem);
                break;
            case Tomes.HighJump:
                break;
            case Tomes.Float:
                GUI.DrawTexture(firstTomePos, floatEmblem);
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

        switch (tomePresets[currentPreset, 1])
        {
            case Tomes.Empty:
                break;
            case Tomes.Fireball:
                GUI.DrawTexture(secondTomePos, fireEmblem);
                break;
            case Tomes.Lightning:
                GUI.DrawTexture(secondTomePos, lightningEmblem);
                break;
            case Tomes.HighJump:
                break;
            case Tomes.Float:
                GUI.DrawTexture(secondTomePos, floatEmblem);
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

        switch (tomePresets[currentPreset, 2])
        {
            case Tomes.Empty:
                break;
            case Tomes.Fireball:
                GUI.DrawTexture(thirdTomePos, fireEmblem);
                break;
            case Tomes.Lightning:
                GUI.DrawTexture(thirdTomePos, lightningEmblem);
                break;
            case Tomes.HighJump:
                break;
            case Tomes.Float:
                GUI.DrawTexture(thirdTomePos, floatEmblem);
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
}
