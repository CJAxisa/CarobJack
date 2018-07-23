using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeManager : MonoBehaviour {
    //change to tome state
    public enum TomeState
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
    public TomeState firstTome;
    public TomeState secondTome;
    public TomeState thirdTome;

    //add three public vars of class tome

    public TomeState[,] tomePresets;
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
        firstTome = TomeState.Fireball;
        secondTome = TomeState.Lightning;
        thirdTome = TomeState.Float;

        currentPreset = 0;
        tomePresets = new TomeState[5,3];

        for (int i = 0; i < tomePresets.GetUpperBound(0); i++)
        {
            for (int j = 0; j < tomePresets.GetUpperBound(1); j++)
            {
                tomePresets[i, j] = TomeState.Empty;
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
        if (firstTome == TomeState.Empty)
            firstTomeObj.SetActive(false);
        else if(firstTome != TomeState.Empty && firstTomeObj.activeInHierarchy ==false)
            firstTomeObj.SetActive(true);

        if (secondTome == TomeState.Empty)
            secondTomeObj.SetActive(false);
        else if (secondTome != TomeState.Empty && secondTomeObj.activeInHierarchy == false)
            secondTomeObj.SetActive(true);

        if (thirdTome == TomeState.Empty)
            thirdTomeObj.SetActive(false);
        else if (thirdTome != TomeState.Empty && thirdTomeObj.activeInHierarchy == false)
            thirdTomeObj.SetActive(true);


        switch (firstTome)
        {
            case TomeState.Empty:
                break;
            case TomeState.Fireball:
                //gameObject.GetComponent<FireTome>().enabled = true;
                break;
            case TomeState.Lightning:
                break;
            case TomeState.HighJump:
                break;
            case TomeState.Float:
                break;
            case TomeState.MagicMissile:
                break;
            case TomeState.Lazer:
                break;
            case TomeState.Dash:
                break;
            case TomeState.SpeedUp:
                break;
            case TomeState.PowerUp:
                break;
            case TomeState.Stun:
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
            case TomeState.Empty:
                break;
            case TomeState.Fireball:
                GUI.DrawTexture(firstTomePos, fireEmblem);
                break;
            case TomeState.Lightning:
                GUI.DrawTexture(firstTomePos, lightningEmblem);
                break;
            case TomeState.HighJump:
                break;
            case TomeState.Float:
                GUI.DrawTexture(firstTomePos, floatEmblem);
                break;
            case TomeState.MagicMissile:
                break;
            case TomeState.Lazer:
                break;
            case TomeState.Dash:
                break;
            case TomeState.SpeedUp:
                break;
            case TomeState.PowerUp:
                break;
            case TomeState.Stun:
                break;
            default:
                break;
        }

        switch (tomePresets[currentPreset, 1])
        {
            case TomeState.Empty:
                break;
            case TomeState.Fireball:
                GUI.DrawTexture(secondTomePos, fireEmblem);
                break;
            case TomeState.Lightning:
                GUI.DrawTexture(secondTomePos, lightningEmblem);
                break;
            case TomeState.HighJump:
                break;
            case TomeState.Float:
                GUI.DrawTexture(secondTomePos, floatEmblem);
                break;
            case TomeState.MagicMissile:
                break;
            case TomeState.Lazer:
                break;
            case TomeState.Dash:
                break;
            case TomeState.SpeedUp:
                break;
            case TomeState.PowerUp:
                break;
            case TomeState.Stun:
                break;
            default:
                break;
        }

        switch (tomePresets[currentPreset, 2])
        {
            case TomeState.Empty:
                break;
            case TomeState.Fireball:
                GUI.DrawTexture(thirdTomePos, fireEmblem);
                break;
            case TomeState.Lightning:
                GUI.DrawTexture(thirdTomePos, lightningEmblem);
                break;
            case TomeState.HighJump:
                break;
            case TomeState.Float:
                GUI.DrawTexture(thirdTomePos, floatEmblem);
                break;
            case TomeState.MagicMissile:
                break;
            case TomeState.Lazer:
                break;
            case TomeState.Dash:
                break;
            case TomeState.SpeedUp:
                break;
            case TomeState.PowerUp:
                break;
            case TomeState.Stun:
                break;
            default:
                break;
        }
    }
}
