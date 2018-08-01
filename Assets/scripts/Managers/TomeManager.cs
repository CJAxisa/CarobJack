using System.Collections;
using System.Collections.Generic;
using Tomes;
using UnityEngine;
/* TO USE TOME:
** - Press '1', '2', or '3' on keyboard
** - 'LT', 'RT', 'RB' on XBOX ONE controller
*/
[RequireComponent (typeof (AudioManager))]
public class TomeManager : MonoBehaviour {
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

    private AudioSource audioSource;
    private AudioManager audioManager;
    private float playSoundTimer = 0f;
    private bool playSound = true;

    // Treat tomePresets as a two dimensional array (i.e. tomePresets[currentPreset][currentTomeIndex])
    [HideInInspector] public Tome currentTome;
    private List <TomeState[]> tomePresets;
    private int currentTomeIndex;
    private int currentPreset;

    // *NEW* Checklist of tomes the player has *NEW*
    // These will determine what tomes the player can choose when creating or editing a preset
    // These will therefore be used to AID THE GUI ONLY
    private bool hasFireTome;
    private bool hasFloatTome;
    private bool hasLightningTome;
    // etc..

    // Floating tome objects
    public GameObject firstTomeObj;
    public GameObject secondTomeObj;
    public GameObject thirdTomeObj;
    public GameObject tomePrefab;

    // GUI variables
    public Texture2D tomeSelect;
    public Texture2D fireEmblem;
    public Texture2D floatEmblem;
    public Texture2D lightningEmblem;
    Rect tomeSelectPos;
    Rect firstTomePos;
    Rect secondTomePos;
    Rect thirdTomePos;

    void Awake() {
      audioManager = GetComponent<AudioManager>();
    }

    void Start () {
        hasFireTome = false;
        hasFloatTome = true;
        hasLightningTome = false;
        InitializeTomePresets();
        CreateFloatingTomes();
        InitializeTomeSelectGUI();
    }

	void Update () {
        UseTome();
        TogglePreset();
        CheckFloatingTomeStates();
    }

    public void InitializeTomePresets() {
        tomePresets = new List<TomeState[]>();

        // ------------- For playtesting purposes -------------------
        // You can change what tomes are currently in the preset HERE
        AddPreset(TomeState.Float, TomeState.Float, TomeState.Float);
        AddPreset(TomeState.Float, TomeState.Fireball, TomeState.Lightning);
        currentPreset = 0;
    }

    // Note: We shall provide the player with a GUI that displays ONLY the tomes he or she has obtained
    // Note: LT, RT, & RB/1, 2, & 3 are associated with 1st, 2nd, & 3rd param
    void AddPreset(TomeState newTomeState1, TomeState newTomeState2, TomeState newTomeState3) {
        tomePresets.Add(new TomeState [] {newTomeState1, newTomeState2, newTomeState3});
        currentPreset = tomePresets.Count - 1;
    }

    // Note: We shall provide the player with a GUI that displays ONLY the tomes he or she has obtained
    void EditPreset(int currentPreset, int currentTomeIndex, TomeState newTomeState) {
        tomePresets[currentPreset][currentTomeIndex] = newTomeState;
    }

    void InitializeTomeSelectGUI() {
        tomeSelectPos = new Rect(Screen.width / 2f - 97f, Screen.height - 64f, 193f, 64f);
        firstTomePos = new Rect(Screen.width / 2f - 95f, Screen.height - 64f, 62f, 62f);
        secondTomePos = new Rect(Screen.width / 2f - 95f +62f, Screen.height - 64f, 62f, 62f);
        thirdTomePos = new Rect(Screen.width / 2f + 29f, Screen.height - 64f, 62f, 62f);
    }

    public void UseTome() {
        if(tomePresets.Count > 0) {
            bool inUse = false;

            if(Input.GetAxisRaw("Cast1") > 0 || Input.GetButton("Cast1")) {
                currentTomeIndex = 0;
                inUse = true;
            }
            if(Input.GetAxisRaw("Cast2") > 0 || Input.GetButton("Cast2")) {
                currentTomeIndex = 1;
                inUse = true;
            }
            if(Input.GetButton("Cast3")) {
                currentTomeIndex = 2;
                inUse = true;
            }

            if(inUse) {
                // Here we set our Tome object 'currentTome' to the desired tome
                switch(tomePresets[currentPreset][currentTomeIndex]) {
                    case TomeState.Empty:
                        currentTome = null;
                        break;
                    case TomeState.Fireball:
                        //currentTome = GetComponent<FireballTome>();
                        break;
                    case TomeState.Lightning:
                        //currentTome = GetComponent<LightningTome>();
                        break;
                    case TomeState.HighJump:
                        //currentTome = GetComponent<HighJump>();
                        break;
                    case TomeState.Float:
                        currentTome = GetComponent<FloatTome>();
                        break;
                    case TomeState.MagicMissile:
                        //currentTome = GetComponent<MagicMissile>();
                        break;
                    case TomeState.Lazer:
                        //currentTome = GetComponent<Lazer>();
                        break;
                    case TomeState.Dash:
                        // TODO: Remove this case in all switch statements
                        break;
                    case TomeState.SpeedUp:
                        // currentTome = GetComponent<SpeedUpTome>();
                        break;
                    case TomeState.PowerUp:
                        // currentTome = GetComponent<PowerUpTome>();
                        break;
                    case TomeState.Stun:
                        //currentTome = GetComponent<StunTome>();
                        break;
                    default:
                        break;
                }
            }
            if(currentTome != null) {
                currentTome.use(inUse);
                currentTome.playSound(inUse);
                // TODO: Remove the if statement below once you have added every single tome in this switch block
                if(!inUse) {
                    currentTome = null;
                }
            }
        }
    }

    void TogglePreset() {
        if(tomePresets.Count > 0) {
            if(Input.GetAxis("Mouse ScrollWheel") > 0) {
                if (currentPreset == tomePresets.Count - 1)
                    currentPreset = 0;
                else
                    currentPreset++;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                if (currentPreset == 0)
                    currentPreset = tomePresets.Count - 1;
                else
                    currentPreset--;
            }
        }
    }

    void CheckFloatingTomeStates() {
        // Checks if the tomes are active and if they should be
        if (tomePresets[currentPreset][0] == TomeState.Empty)
            firstTomeObj.SetActive(false);
        else if (tomePresets[currentPreset][0] != TomeState.Empty && firstTomeObj.activeInHierarchy == false)
            firstTomeObj.SetActive(true);

        if (tomePresets[currentPreset][1] == TomeState.Empty)
            secondTomeObj.SetActive(false);
        else if (tomePresets[currentPreset][1] != TomeState.Empty && secondTomeObj.activeInHierarchy == false)
            secondTomeObj.SetActive(true);

        if (tomePresets[currentPreset][2] == TomeState.Empty)
            thirdTomeObj.SetActive(false);
        else if (tomePresets[currentPreset][2] != TomeState.Empty && thirdTomeObj.activeInHierarchy == false)
            thirdTomeObj.SetActive(true);

        // CJ THIS IS YOUR CODE I LEFT IT HERE JUST IN CASE
        // switch (firstTome)
        // {
        //     case TomeState.Empty:
        //         break;
        //     case TomeState.Fireball:
        //         //gameObject.GetComponent<FireTome>().enabled = true;
        //         break;
        //     case TomeState.Lightning:
        //         break;
        //     case TomeState.HighJump:
        //         break;
        //     case TomeState.Float:
        //
        //         break;
        //     case TomeState.MagicMissile:
        //         break;
        //     case TomeState.Lazer:
        //         break;
        //     case TomeState.Dash:
        //         break;
        //     case TomeState.SpeedUp:
        //         break;
        //     case TomeState.PowerUp:
        //         break;
        //     case TomeState.Stun:
        //         break;
        //     default:
        //         break;
        // }
    }

    public void CreateFloatingTomes()
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

    // TODO: Make this a long switch block instead
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Tome")) {
            if(other.name == "FireTome") {
                hasFireTome = true;
            }
            else if(other.name == "StunTome") {
                //hasStunTome = true;
            }
            else if(other.name == "LazerTome") {
                //hasLazerTome = true;
            }
            else if(other.name == "PunchTome") {
                //hasPunchTome = true;
            }
            else if(other.name == "IceTome") {
                //hasIceTome = true;
            }
            else if(other.name == "ShieldTome") {
                //hasShieldTome = true;
            }
            else if(other.name == "SuplexTome") {
                //hasSuplexTome = true;
            }
            else if(other.name == "JumpAttackTome") {
                //hasJumpAttackTome = true;
            }
            //***Non-Combat Tomes:
            else if(other.name == "FloatTome") {
                hasFloatTome = true;
            }
            else if(other.name == "StealthTome") {
                //hasStealthTome = true;
            }
            else if(other.name == "FlyTome") {
                //hasFlyTome = true;
            }
            else if(other.name == "BargainingTome") {
                //hasBargainTome = true;
            }
            else if(other.name == "SpeedBoostTome") {
                //hasSpeedBoostTome = true;
            }
            else if(other.name == "IntimidationTome") {
                //hasIntimidationTome = true;
            }
            else if(other.name == "DisguiseTome") {
                //hasDisguiseTome = true;
            }
            else if(other.name == "HealTome") {
                //hasHealTome = true;
            }
            else if(other.name == "GoofyTome") {
                //hasGoofyTome = true;
            }
            else if(other.name == "InvestigationTome") {
                //hasInvestigationTome = true;
            }
            else if(other.name == "TallTome") {
                //hasTallTome = true;
            }
            else if(other.name == "TinyTome") {
                //hasTinyTome = true;
            }
            else if(other.name == "TimeTome") {
                //hasTimeTome = true;
            }
            //***Summoning Tomes:
            else if(other.name == "DeadEnemiesTome") {
                //hasZombieTome = true;
            }
            else if(other.name == "GodTome") {
                //hasGodTome = true;
            }
            Destroy(other.gameObject);
            if(audioManager.collectedTome != null) {
                audioSource.PlayOneShot(audioManager.collectedTome, 0.4f);
            }
        }
    }

    /* -------------------- BEGIN GUI -------------------- */
    private void OnGUI()
    {
        GUI.DrawTexture(tomeSelectPos,tomeSelect);

        switch (tomePresets[currentPreset][0])
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

        switch (tomePresets[currentPreset][1])
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

        switch (tomePresets[currentPreset][2])
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
    /* -------------------- END GUI -------------------- */
    }
