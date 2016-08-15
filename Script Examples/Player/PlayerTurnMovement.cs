using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerTurnMovement : MonoBehaviour {

    //public enum ActionType { _Move, _Melee, _Shoot, _Bomb };

    //public struct Action
    //{
    //    public ActionType type;
    //    public Vector3 movement;

    //    public string ToString()
    //    {
    //        return "Type: " + type + "\n" + "Position: " + movement.ToString();
    //    }
    //}

    public float moveActionCost;
    public float speed = 6f;
//    public int maxActions;

    public bool isSecond;
    GameObject player;
//    List<Action> actions;
    int numActions;
    Vector3 tempMovement;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    public TurnManager turnManager;
    float currentCost;
    Camera camera;
//    bool firstMove = true;

    void Awake()
    {
//        player = GameObject.FindGameObjectWithTag("Player");
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
//        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        //string tag;
        //if (isSecond)
        //{
        //    tag = "Player2";
        //    camera = GameObject.FindGameObjectWithTag("Camera2").GetComponent<Camera>();
        //}
        //else
        //{
        //    tag = "Player";
        //    camera = GameObject.FindGameObjectWithTag("Camera1").GetComponent<Camera>();
        //}
//        turnManager = GameObject.FindGameObjectWithTag(tag).GetComponent<TurnManager>();
        tempMovement = new Vector3(0f, 0f, 0f);
        movement = new Vector3(0f, 0f, 0f);
        currentCost = 0;
//        actions = new List<Action>();
    }

    void FixedUpdate()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}
        if (turnManager.executing == TurnManager.Executing._True) { return; }
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (turnManager.turn)
        {
            Move(h, v);
            Turning();
            Animating(h, v);
        }

    }

    void Move(float h, float v)
    {
        tempMovement.Set(h, 0f, v);
        
        // create new action whenever ghost changes direction. set the type to a move action and give final position
        if (movement.normalized != tempMovement.normalized && movement.magnitude != 0f)
        {
//            Debug.Log("Making new action");
            turnManager.AddActionToList(TurnManager.ActionType._Move, transform.position, currentCost);
            currentCost = 0;
        }
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        float tempCost = currentCost + movement.magnitude * moveActionCost;

        float managerCost = turnManager.GetCurrentActionCost();
        // if this movement won't put the player over the maximum turn cost, move
        if (managerCost + tempCost <= turnManager.maxActions)
        {
            playerRigidbody.MovePosition(transform.position + movement);
            currentCost = tempCost;
            turnManager.DisplayActionCostSlider(currentCost);
        }
        //else
        //{
        //    // otherwise scale the movement to fit
        //    float scale = turnManager.maxActions - managerCost;
        //    playerRigidbody.MovePosition(transform.position + movement.normalized * scale);
        //}
        
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

    //public List<Action> GetActions()
    //{
    //    return actions;
    //}
}
