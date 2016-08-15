using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 6f;
    public float turnSpeed = 24f;
    [SyncVar]
    public bool spdBuff;
    [SyncVar]
    public bool spdDebuff;
    public float spdBuffMultiplier = 2f;
    public float spdDebuffMultiplier = .3f;
    public float modifierTimer = 10f;

    float timeOnBuff;
    float timeOnDebuff = -50;

    bool isSecond;
    bool turnExecute = false;
    Vector3 turnTarget;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 10000f;
    TurnManager turnManager;
    public Camera camera;


    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
//        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        turnManager = gameObject.GetComponentInChildren<TurnManager>();
        //if (gameObject.tag == "Player") {
        //    camera = GameObject.FindGameObjectWithTag("Camera1").GetComponent<Camera>();
        //} else if (gameObject.tag == "Player2") {
        //    camera = GameObject.FindGameObjectWithTag("Camera2").GetComponent<Camera>();
        //}
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (turnExecute)
        {
            
            //// if h and v are ~0, finished with command execution
            //if (h < .01f && v < .01f)
            //{
            //    turnExecute = false;
            //    // tell turnManager that it's finished
            //    turnManager.CommandComplete();
            //}
            MoveTarget(turnTarget);
            float h = turnTarget.x - transform.position.x;
            float v = turnTarget.z - transform.position.z;
            if (Mathf.Abs(h) < .2f && Mathf.Abs(v) < .2f)
            {
                turnExecute = false;
                turnManager.CommandComplete();
            }
        }

        if (!turnManager.turn)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            //if (Time.timeScale == 1)
            //{
            Move(h, v);
            Turning();
            Animating(h, v);
            //        }


            //buff timer
            if (timeOnBuff > 0)
            {
                timeOnBuff -= Time.deltaTime;
            }
            else
            {
                CmdSetBuff(false);
            }

            //debuff timer
            if (timeOnDebuff == -50 && spdDebuff)
            {
                // special case when enemy called debuff on you. timer will not be active but bool is true
                timeOnDebuff = modifierTimer;
            }
            else if (timeOnDebuff > 0)
            {
                timeOnDebuff -= Time.deltaTime;
            }
            else
            {
                CmdRemoveDebuff();
            }
        }
    }

    public void TurnMove(Vector3 target)
    {
//        Debug.Log("TurnMove: " + target.ToString());
        turnExecute = true;
        turnTarget = target;
    }

    void MoveTarget(Vector3 target)
    {
//        Debug.Log("MoveTarget: " + target.ToString());
        float step = turnSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(Vector3.MoveTowards(playerRigidbody.position, target, step));
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        if (spdBuff)
        {
            movement *= spdBuffMultiplier;
        }
        if (spdDebuff)
        {
            movement *= spdDebuffMultiplier;
        }
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        Ray camRay = camera.ScreenPointToRay(Input.mousePosition);

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

    public void SetBuff()
    {
        CmdSetBuff(true);
        timeOnBuff = modifierTimer;
    }

    [Command]
    void CmdSetBuff(bool b)
    {
        spdBuff = b;
    }

    [Command]
    public void CmdGiveDebuff()
    {
        string otherTag;
        if (gameObject.tag == "Player")
        {
            otherTag = "Player2";
        }
        else
        {
            otherTag = "Player";
        }
        GameObject.FindGameObjectWithTag(otherTag).GetComponent<PlayerMovement>().SetDebuff();
    }

    public void SetDebuff()
    {
        spdDebuff = true;
        timeOnDebuff = modifierTimer;
    }

    [Command]
    void CmdRemoveDebuff()
    {
        spdDebuff = false;
        timeOnDebuff = -50f;
    }
}
