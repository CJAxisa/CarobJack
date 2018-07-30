using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
    public enum PlayerStates
    {
        Empty,
        Idle,
        Walking,
        Rising,
        Falling,
        Casting,
        Floating
    }

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
    public FF_Animator playerAnimator;
    public Player playerScript;
    public SpriteRenderer playerSpriteRenderer;

    public PlayerStates currentState;
    private PlayerStates prevState;
    public bool facingRight;

    public TomeState firstTome;
    public TomeState secondTome;
    public TomeState thirdTome;
    public GameObject firstTomeObj;
    public GameObject secondTomeObj;
    public GameObject thirdTomeObj;
    public GameObject tomePrefab;

    void Start() {
        playerAnimator = GetComponent<FF_Animator>();
        playerScript = GetComponent<Player>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        currentState = PlayerStates.Idle;
        facingRight = true;

        firstTome = TomeState.Fireball;
        secondTome = TomeState.Dash;
        thirdTome = TomeState.Float;

        createTomes();
    }

    void Update() {
        CheckPlayerStates();
        CheckTomeStates();
    }

    void CheckPlayerStates() {
        // Used for checking if state has changed since last frame
        prevState = currentState;

        //animation stuff
        if (playerScript.velocity.x > 0)
            facingRight = true;
        else if (playerScript.velocity.x < 0)
            facingRight = false;

        if (playerScript.isGrounded && Mathf.Abs(playerScript.velocity.x) > 0)
            currentState = PlayerStates.Walking;
        if (playerScript.isGrounded && playerScript.velocity.x == 0)
            currentState = PlayerStates.Idle;
        if (!playerScript.isGrounded && playerScript.velocity.y < 0)
            currentState = PlayerStates.Falling;
        if (!playerScript.isGrounded && playerScript.velocity.y > 0)
            currentState = PlayerStates.Rising;
        if (playerScript.isFloating)
            currentState = PlayerStates.Floating;

        if (Input.GetButton("Cast3"))
            currentState = PlayerStates.Casting;

        if(playerAnimator != null && playerSpriteRenderer != null) {
            if (facingRight)
                //playerAnimator.SetFacing("right");
                playerSpriteRenderer.flipX = false;
            else
                //playerAnimator.SetFacing("left");
                playerSpriteRenderer.flipX = true;
        }

        if (prevState != currentState && playerAnimator != null)
        {
            //Debug.Log("STATE HAS CHANGED");
            switch (currentState)
            {
                case PlayerStates.Idle:
                    if(prevState == PlayerStates.Casting)
                        playerAnimator.SetAnimation("Idle", true, 0);
                    else
                        playerAnimator.SetAnimation("Idle", false, 0);
                    break;
                case PlayerStates.Walking:
                    playerAnimator.SetAnimation("Walking", false, 0);
                    break;
                case PlayerStates.Rising:
                    playerAnimator.SetAnimation("Rising", false, 0);
                    break;
                case PlayerStates.Falling:
                    playerAnimator.SetAnimation("Falling", false, 0);
                    break;
                case PlayerStates.Casting:
                    playerAnimator.SetAnimation("Casting", false, 0);
                    break;
                case PlayerStates.Floating:
                    playerAnimator.SetAnimation("Rising", false, 0);
                    break;
                default:
                    break;
            }
        }
    }

    void CheckTomeStates() {
        // Checks if the tomes are active and if they should be
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
