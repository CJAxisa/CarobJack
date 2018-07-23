using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using Tomes;
using UnityEngine;

[RequireComponent (typeof (AudioManager))]
public class TomeManager : MonoBehaviour {
<<<<<<< HEAD
    //change to tome state
    public enum TomeState
=======
    // NICK CODE -------------------
    public Tome currentTome;
    private List <Tome> inventory; // May change this to a List of Tome arrays and rename it 'presets'

    private AudioSource audioSource;
    private AudioManager audioManager;
    private float playSoundTimer = 0f;
    private bool playSound = true;
    // CJ CODE ---------------------
    // CHANGED TO TOMESTATES TO PREVENT CONFLICT WITH NAMESPACE 'Tomes'
    public enum TomeStates
>>>>>>> master
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

<<<<<<< HEAD
    //all of these are public for the time being for the ease of playtesting
    public TomeState firstTome;
    public TomeState secondTome;
    public TomeState thirdTome;

    //add three public vars of class tome

    public TomeState[,] tomePresets;
    public int currentPreset;           //starts at 0
=======
    public TomesStates firstTome;
    public TomeStates secondTome;
    public TomeStates thirdTome;
>>>>>>> master

    public GameObject tomePrefab;

    public GameObject firstTomeObj;
    public GameObject secondTomeObj;
    public GameObject thirdTomeObj;

    GameObject player;

<<<<<<< HEAD
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
=======
    // NICK CODE -----------------
    void Awake() {
      audioManager = GetComponent<AudioManager>();
    }


    // Use this for initialization
    void Start () {
        // NICK CODE -------------------------
        currentTome = GetComponent<FloatTome>();
        inventory = new List<Tome>();
        // CJ CODE ---------------------------
        firstTome = TomeStates.Fireball;
        secondTome = TomeStates.Dash;
        thirdTome = TomeStates.Float;

        createTomes();
    }

	// Update is called once per frame
	void Update () {
        // NICK CODE ----------------------
        UseTome();
        // CJ CODE ------------------------
        //checks if the tomes are active and if they should be
        if (firstTome == TomeStates.Empty)
            firstTomeObj.SetActive(false);
        else if(firstTome != TomeStates.Empty && firstTomeObj.activeInHierarchy ==false)
            firstTomeObj.SetActive(true);

        if (secondTome == TomeStates.Empty)
            secondTomeObj.SetActive(false);
        else if (secondTome != TomeStates.Empty && secondTomeObj.activeInHierarchy == false)
            secondTomeObj.SetActive(true);

        if (thirdTome == TomeStates.Empty)
            thirdTomeObj.SetActive(false);
        else if (thirdTome != TomeStates.Empty && thirdTomeObj.activeInHierarchy == false)
>>>>>>> master
            thirdTomeObj.SetActive(true);


        switch (firstTome)
        {
<<<<<<< HEAD
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
=======
            case TomeStates.Empty:
                break;
            case TomeStates.Fireball:
                //gameObject.GetComponent<FireTome>().enabled = true;
                break;
            case TomeStates.Lightning:
                break;
            case TomeStates.HighJump:
                break;
            case TomeStates.Float:
                break;
            case TomeStates.MagicMissile:
                break;
            case TomeStates.Lazer:
                break;
            case TomeStates.Dash:
                break;
            case TomeStates.SpeedUp:
                break;
            case TomeStates.PowerUp:
                break;
            case TomeStates.Stun:
>>>>>>> master
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
    // NICK CODE ------------------------------
    // TODO: Going to heavily change this but leaving it in for now
    public void UseTome() {
        if(inventory.Count > 0 || currentTome != null) {
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown("l")) {
                currentTome.use(true);
                currentTome.playSound(true);
            }
            if(Input.GetMouseButtonUp(0) || Input.GetKeyUp("l")) {
                currentTome.use(false);
                currentTome.playSound(false);
            }
        }
        else {
            if(Input.GetMouseButtonDown(0)) {
                if(playSound) {
                    audioSource.PlayOneShot(audioManager.cannotUse, 0.4f);
                    playSound = false;
                }
            }
            if(!playSound) {
                playSoundTimer += 1.0f * Time.deltaTime;
                if(playSoundTimer > audioManager.cannotUseSoundDelay) {
                    playSound = true;
                    playSoundTimer = 0f;
                }
            }
        }
    }

    // TODO: May not be useful for now, but again leaving it in for now
    void AddTome(Tome newTome) {
        if(inventory.Count > 0) {
            currentTome.use(false);
        }
        inventory.Add(newTome);
        currentTome = newTome; //<----- This makes your new tome the newly acquired tome
        tomeIndex = inventory.Count - 1;
        Debug.Log("Added tome");
    }

    // Ask Nick about prupose of this section por favor
    void OnTriggerEnter2D(Collider2D other) {
        // This first check is here so that way I can just destroy the other.gameObject with one line of code at the bottom of this block o code
        if(other.CompareTag("Tome")) {
            /* We have collided with a tome object so lets add the tome based on the game objects name */
            Debug.Log("Collided with the: " + other.name);
            if(other.name == "FireTome") {
                //AddTome(gameObject.GetComponent<FireTome>());
            }
            else if(other.name == "StunTome") {
                //AddTome(gameObject.GetComponent<StunTome>());
            }
            else if(other.name == "FloatTome") {
                //AddTome(gameObject.GetComponent<FloatTome>());
            }
            /* Combat Tomes:
            else if(other.name == "LazerTome") {
                AddTome(gameObject.GetComponent<LazerTome>());
            }
            else if(other.name == "PunchTome") {
                AddTome(gameObject.GetComponent<PunchTome>());
            }
            else if(other.name == "IceTome") {
                AddTome(gameObject.GetComponent<IceTome>());
            }
            else if(other.name == "ShieldTome") {
                AddTome(gameObject.GetComponent<ShieldTome>());
            }
            else if(other.name == "SuplexTome") {
                AddTome(gameObject.GetComponent<SuplexTome>());
            }
            else if(other.name == "JumpAttackTome") {
                AddTome(gameObject.GetComponent<JumpAttackTome>());
            }
            ***Non-Combat Tomes:
            else if(other.name == "StealthTome") {
                AddTome(gameObject.GetComponent<StealthTome>());
            }
            else if(other.name == "FlyTome") {
                AddTome(gameObject.GetComponent<FlyTome>());
            }
            else if(other.name == "BargainingTome") {
                AddTome(gameObject.GetComponent<BargainingTome>());
            }
            else if(other.name == "SpeedBoostTome") {
                AddTome(gameObject.GetComponent<SpeedBoostTome>());
            }
            else if(other.name == "IntimidationTome") {
                AddTome(gameObject.GetComponent<IntimidationTome>());
            }
            else if(other.name == "DisguiseTome") {
                AddTome(gameObject.GetComponent<DisguiseTome>());
            }
            else if(other.name == "HealTome") {
                AddTome(gameObject.GetComponent<HealTome>());
            }
            else if(other.name == "GoofyTome") {
                AddTome(gameObject.GetComponent<GoofyTome>());
            }
            else if(other.name == "InvestigationTome") {
                AddTome(gameObject.GetComponent<InvestigationTome>());
            }
            else if(other.name == "TallTome") {
                AddTome(gameObject.GetComponent<TallTome>());
            }
            else if(other.name == "TinyTome") {
                AddTome(gameObject.GetComponent<TinyTome>());
            }
            else if(other.name == "TimeTome") {
                AddTome(gameObject.GetComponent<TimeTome>());
            }
            ***Summoning Tomes:
            else if(other.name == "DeadEnemiesTome") {
                AddTome(gameObject.GetComponent<DeadEnemiesTome>());
            }
            else if(other.name == "GodTome") {
                AddTome(gameObject.GetComponent<GodTome>());
            }
            */
            Destroy(other.gameObject);
            if(audioManager.collectedTome != null) {
                audioSource.PlayOneShot(audioManager.collectedTome, 0.4f);
            }
        }
    }
}
