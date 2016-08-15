using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TurnManager : NetworkBehaviour {

    public enum ActionType { _Move, _Melee, _Shoot, _Bomb };

    public struct Action
    {
        public ActionType type;
        public Vector3 target;
        public float cost;

        public string ToString()
        {
            return "Type: " + type + "\n" + "Position: " + target.ToString();
        }
    }

    Image Fill;
    public GameObject ghost;
    public float speed = 24f;
    public float wait = .1f;
    public float actionCharge;
    public bool turn = false;
    public float maxActions;
    public float actionDrain;
    public bool secondPlayer;
    Slider actionSlider;

    public enum Executing { _False, _True, _Complete };
    public Executing executing = Executing._False;
    bool waiting = false;
    bool released = true;
//    bool paused = false;
    GameObject g;
    PlayerBombing bombing;
    PlayerTurnBombing turnBombing;
    PlayerMovement movement;
    List<Action> actions;
    Rigidbody playerRigidbody;
    List<Vector3> lastCommandPosition;
    List<GameObject> movementLines;
    float currentTurnActionDrain = 0;

	// Use this for initialization
	void Awake () {
        bombing = GetComponent<PlayerBombing>();
        movement = GetComponent<PlayerMovement>();
        playerRigidbody = GetComponent<Rigidbody>();
//        movementLine = GetComponent<LineRenderer>();
//        movementLine = gameObject.AddComponent<LineRenderer>();
        actions = new List<Action>();
        movementLines = new List<GameObject>();
        lastCommandPosition = new List<Vector3>();
        //string tag;
        //if (secondPlayer)
        //{
        //    tag = "ActionSlider2";
        //}
        //else
        //{
        //    tag = "ActionSlider";
        //}
        actionSlider = GameObject.FindGameObjectWithTag("ActionSlider").GetComponent<Slider>();
        actionSlider.value = maxActions;
        Fill = GameObject.FindGameObjectWithTag("ActionSliderFill").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
//        Debug.Log("Action value is: " + actionSlider.value);
//        Debug.Log("Executing value is: " + executing.ToString());

        // drain action bar when in planning phase of turn
        if (turn && executing != Executing._True)
        {
//            Debug.Log("drain");
            float actionVal = actionSlider.value;
            float drainAmt = actionDrain * Time.deltaTime;
            actionVal -= drainAmt;
            if (actionVal <= 0)
            {
                drainAmt += actionVal;  // should be reducing drainAmt to the value that takes actionVal to 0
                actionVal = 0;
//                executing = Executing._True;
//                Debug.Log("Am i stupid");
                currentTurnActionDrain += drainAmt;
                DisplayActionCostSlider(0);
                StartExecution();
            }
            else
            {
                currentTurnActionDrain += drainAmt;
                DisplayActionCostSlider(0);
            }

            //actionSlider.value = actionVal;

        }

        // Execution of turn commands is complete, exit turn phase
        if (executing == Executing._Complete)
        {
            turn = false;
            turnBombing.DestroyBombs();
            Destroy(g, .01f);
            executing = Executing._False;
            DisplayActionCostSlider(0);
            actions.Clear();
            DestroyMoveLines();
            currentTurnActionDrain = 0;
//            movementLine.enabled = false;
        }
        // if executing commands, do nothing until finished
        else if (executing == Executing._True) { return; }
        else if (executing == Executing._False && !turn)
        {
            float actionVal = actionSlider.value;
            actionVal += Time.deltaTime * actionCharge;
            if (actionVal >= maxActions)
            {
                actionVal = maxActions;
                Fill.color = Color.green;
            }
            actionSlider.value = actionVal;
        }

        // when the cancel command button is pressed, remove the last command
        if (Input.GetButtonDown("CancelTurnCommand"))
        {
//            Debug.Log("Command Cancel");
            DeleteLastAction();
            g.transform.position = lastCommandPosition[lastCommandPosition.Count - 1];
            if (lastCommandPosition.Count > 1)
            {
                if (actions.Count != 0)
                {
                    if (actions[actions.Count - 1].type == ActionType._Move)
                    {
                        lastCommandPosition.RemoveAt(lastCommandPosition.Count - 1);
                    }
                }  
            }
            StartCoroutine("DrawLine");
           
        }

        // start turn phase if not currently on turn phase. if on turn phase, this button confirms execution
        // or does exits turn phase if there are no commands
        if (Input.GetButtonDown("Turn"))
        {

            if (turn)
            {
                StartExecution();
            }
            else if (!turn && actionSlider.value == maxActions)
            {
                turn = true;
                currentTurnActionDrain = 0;
                Fill.color = Color.white;
                g = Instantiate(ghost, transform.position, transform.rotation) as GameObject;
                turnBombing = g.GetComponent<PlayerTurnBombing>();
                turnBombing.isSecond = secondPlayer;
                turnBombing.turnManager = this;
                g.GetComponent<PlayerTurnMovement>().isSecond = secondPlayer;
                g.GetComponent<PlayerTurnMovement>().turnManager = this;
                lastCommandPosition.Add(transform.position);
            }
        }
        
	}

    void StartExecution()
    {
        if (g == null) { return; }
        g.transform.position = transform.position;
        if (actions.Count != 0)
        {
            executing = Executing._True;
            StartCoroutine("Execute");
        }
        else
        {
            turn = false;
            Destroy(g, .01f);
        }
        Destroy(g, .5f);
    }

    // coroutine for drawing the line showing the player's movement commands
    IEnumerator DrawLine()
    {
//        movementLine.enabled = false;
        if (actions.Count == 0) { return null; }
//        movementLine.SetVertexCount(actions.Count + 1);     ///
//        movementLine.SetPosition(0, player.transform.position);
        Vector3 prevPos = transform.position;
        Material redDiffuseMat = Resources.Load("Materials/LineMoveMaterial") as Material;

        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].type == ActionType._Move)
            {
                GameObject child = new GameObject();
                child.transform.parent = transform;
                LineRenderer lr = child.AddComponent<LineRenderer>();
                lr.material = redDiffuseMat;
                lr.SetVertexCount(2);
                lr.SetWidth(.2f, .2f);
                lr.SetPosition(0, prevPos);
                lr.SetPosition(1, actions[i].target);
                prevPos = actions[i].target;
                lr.enabled = true;
                movementLines.Add(child);

//                movementLine.SetPosition(i + 1, actions[i].target);
            }
        }
//        movementLine.enabled = true;
        return null;
    }

    // coroutine that executes every command in the array
    IEnumerator Execute()
    {
        //actions = g.GetComponent<PlayerTurn>().GetActions();
        // iterate through actions list
        foreach(Action a in actions) {
            // if it's a movement command
            if (a.type == ActionType._Move)
            {
//                Debug.Log("Move command to position: " + a.target.ToString());
                waiting = true;
                movement.TurnMove(a.target);
//                StartCoroutine("ExecuteMovement", a.movement);
                while (waiting)
                {
                    yield return new WaitForSeconds(.01f);
                }
            }
            else if (a.type == ActionType._Bomb)
            {
                bombing.PlaceBomb();
            }
        }
//        Debug.Log("ExecuteComplete");
        executing = Executing._Complete;
//        Debug.Log("execute complete");
    }

    // lets the other scripts tell the manager when they are done executing commands
    public void CommandComplete()
    {
        waiting = false;
    }

    public void AddActionToList(ActionType type, Vector3 target, float cost)
    {
        // do nothing if action cost is too high
        if (GetCurrentActionCost() + cost > maxActions) { return; }
        Action newAction = new Action();
        newAction.type = type;
        newAction.target = target;
        newAction.cost = cost;
//        Debug.Log(newAction.ToString());
        actions.Add(newAction);
        DisplayActionCostSlider(0);
        if (type == ActionType._Move)
        {
            StartCoroutine("DrawLine");
        }
        else if (type == ActionType._Bomb)
        {
            turnBombing.PlaceBomb();
            lastCommandPosition.Add(target);
        }
    }

    // player can undo last action. undoing a movement action will undo all movement actions until the previous non-move action
    void DeleteLastAction()
    {
        if (actions.Count == 0) { return; }
//        Debug.Log("Delete Action");
        Action a = actions[actions.Count - 1];
        actions.Remove(a);
        if (a.type == ActionType._Move)
        {
            if (actions.Count == 0)
            {
                DestroyMoveLines();
                StartCoroutine("DrawLine");
                return;
            }
            Action b = actions[actions.Count - 1];
            
        
            if (b.type == ActionType._Move)
            {
                DeleteLastAction();
            }
            else
            {
                DestroyMoveLines();
                StartCoroutine("DrawLine");
            }
        }
        else if (a.type == ActionType._Bomb)
        {
            turnBombing.RemoveLast();
        }
    }

    void DestroyMoveLines()
    {
//        Debug.Log("destroymovelines");
        foreach (GameObject lr in movementLines)
        {
            lr.GetComponent<LineRenderer>().enabled = false;
            Destroy(lr, .05f);
        }
        movementLines.Clear();
    }

    public float GetCurrentActionCost()
    {
        float cost = 0;
        foreach (Action a in actions)
        {
            cost += a.cost;
        }
        return cost;
    }

    public void DisplayActionCostSlider(float f)
    {
        actionSlider.value = maxActions - GetCurrentActionCost() - f - currentTurnActionDrain;
    }

    //IEnumerator ExecuteMovement(Vector3 target)
    //{
    //    Debug.Log("ExecuteMovement");
    //    Vector3 tempMovement = target - player.transform.position;
    //    tempMovement = tempMovement.normalized * speed * Time.deltaTime;
    //    while (playerRigidbody.position.x != target.x || playerRigidbody.position.z != target.z)
    //    {
    //        playerRigidbody.MovePosition(transform.position + tempMovement);
    //        yield return new WaitForSeconds(wait);
    //    }
    //    Debug.Log("MovementCompleted");
    //    waiting = false;
    //}
}
