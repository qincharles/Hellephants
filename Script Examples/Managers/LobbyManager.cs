using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class LobbyManager : NetworkBehaviour {

    public bool isSinglePlayer;
    //[SyncVar]
    //public bool easy = false;

    //[SyncVar]
    //bool difficultySel = false;
    bool player1 = false;
    bool player2 = false;
    public bool done = false;
    int layerMask;
    GameObject[] players;

	// Use this for initialization
	void Awake () {
        layerMask = 15;
        PauseGame();
        //if (!isServer)
        //{
        //    GameObject.Find("PanelDifficultyCheck").SetActive(false);
        //}
        //if (isSinglePlayer)
        //{
        //    done = true;
        //    UnpauseGame();
        //}
	}
	
	// Update is called once per frame
	void Update () {
        if (!done)
        {
            if (isSinglePlayer)
            {
                /**GameObject[]**/ players = FindGameObjectsWithLayer(layerMask);
                players[0].GetComponent<PlayerNumber>().SetPlayerNumber(false);
                UnpauseGame();
                done = true;
            }
            else
            {
                /**GameObject[]**/ players = FindGameObjectsWithLayer(layerMask);
//                Debug.Log("Num players: " + players.Length);
                if (players == null) { return; }
                if (players.Length == 1)
                {
//                    Debug.Log("Waiting for second player");
                }
                else if (players.Length == 2/** && difficultySel**/)
                {
                    Debug.Log("Game starting");
                    players[0].GetComponent<PlayerNumber>().SetPlayerNumber(true);
                    players[1].GetComponent<PlayerNumber>().SetPlayerNumber(false);
                    //if (easy)
                    //{
                    //    SetDifficultyEasy();
                    //}
                    UnpauseGame();
                    done = true;
                }
                else if (players.Length == 0)
                {
                    Debug.Log("Something wrong with layermask");
                }
            }

        }
        
	}

    GameObject[] FindGameObjectsWithLayer(int layer) 
    { 
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
//        Debug.Log("Num objects: " + goArray.Length);
        List<GameObject> goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++) 
        { 
            if (goArray[i].layer == layer) { goList.Add(goArray[i]); } 
        } 
        if (goList.Count == 0) { return null; } 
        return goList.ToArray(); 
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    //void SetDifficultyEasy()
    //{
    //    if (easy)
    //    {
    //        players[0].GetComponent<PlayerMovement>().speed = 8;
    //        players[0].GetComponent<TurnManager>().actionCharge = 25;
    //        players[0].GetComponent<TurnManager>().actionDrain = 5;
    //        players[1].GetComponent<PlayerMovement>().speed = 8;
    //        players[1].GetComponent<TurnManager>().actionCharge = 25;
    //        players[1].GetComponent<TurnManager>().actionDrain = 5;
    //    }
    //}

    //public void SetDifficulty(bool e)
    //{
    //    CmdSetEasy(e);
    //    GameObject.Find("PanelDifficultyCheck").SetActive(false);
    //    CmdSetDifficulty(true);
    //}

    //[Command]
    //void CmdSetEasy(bool e)
    //{
    //    easy = e;
    //}

    //[Command]
    //void CmdSetDifficulty(bool t)
    //{
    //    difficultySel = t;
    //}

}
