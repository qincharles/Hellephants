using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{

    public bool isSinglePlayer;

    bool found = false;
    PlayerScore player1;
    PlayerScore player2;
//    LobbyManager lobbyManager;


    Text text;

    //void Start()
    //{
    //    lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
    //}

    // Update is called once per frame
    void Update()
    {
        if (!found) 
        {
            if (isSinglePlayer)
            {
                player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScore>();
                found = true;
            }
            else
            {
                GetPlayers(); 
            }

        }
        else
        {
            SetSliders();
        }
    }

    // get references to both playerhealth components -- these have synced health values
    void GetPlayers()
    {
        if (player1 == null)
        {
            GameObject p1 = GameObject.FindGameObjectWithTag("Player");
            if (p1 != null)
            {
                player1 = p1.GetComponent<PlayerScore>();
            }
        }

        if (player2 == null)
        {
            GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
            if (p2 != null)
            {
                player2 = p2.GetComponent<PlayerScore>();
            }
        }

        if (player1 != null && player2 != null) { found = true; }


    }

    // set the green health slider to the local player's health value and the red to the other's
    void SetSliders()
    {
        if (isSinglePlayer)
        {
            GameObject.Find("ScoreTextYou").GetComponent<Text>().text = "" + player1.score;
        }
        else
        {
            string tag1;
            string tag2;
            if (player1.isLocalPlayer)
            {
                tag1 = "ScoreTextYou";
                tag2 = "ScoreTextEnemy";
            }
            else
            {
                tag1 = "ScoreTextEnemy";
                tag2 = "ScoreTextYou";
            }

            GameObject.Find(tag1).GetComponent<Text>().text = "" + player1.score;
            GameObject.Find(tag2).GetComponent<Text>().text = "" + player2.score;
        }

    }
}
