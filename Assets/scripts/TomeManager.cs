using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using Tomes;
using UnityEngine;

[RequireComponent (typeof (AudioManager))]
public class TomeManager : MonoBehaviour {
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

    public TomesStates firstTome;
    public TomeStates secondTome;
    public TomeStates thirdTome;

    public GameObject tomePrefab;

    public GameObject firstTomeObj;
    public GameObject secondTomeObj;
    public GameObject thirdTomeObj;

    GameObject player;

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
            thirdTomeObj.SetActive(true);

        switch (firstTome)
        {
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
