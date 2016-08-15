using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour {

    public bool isSinglePlayer;

    bool found = false;
    PlayerHealth player1Health;
    PlayerHealth player2Health;
    LobbyManager lobbyManager;

    //RawImage enemyDeath1;
    //RawImage enemyDeath2;
    //RawImage enemyDeath3;
    //RawImage playerDeath1;
    //RawImage playerDeath2;
    //RawImage playerDeath3;

	// Use this for initialization
	void Start () {
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
//        GetImages();
	}
	
	// Update is called once per frame
	void Update () {
        if (!found)
        {
            if (isSinglePlayer)
            {
                GameObject p1 = GameObject.FindGameObjectWithTag("Player");
                player1Health = p1.GetComponent<PlayerHealth>();
                found = true;
            }
            else
            {
                GetPlayers();
            }
        }
        else
        {
            SetModifierImages();
        }
	}

    //void GetImages()
    //{
    //    if (!isSinglePlayer)
    //    {
    //        enemyDeath1 = GameObject.Find("EnemyDeath1").GetComponent<RawImage>();
    //        enemyDeath2 = GameObject.Find("EnemyDeath2").GetComponent<RawImage>();
    //        enemyDeath3 = GameObject.Find("EnemyDeath3").GetComponent<RawImage>();
    //    }

    //    playerDeath1 = GameObject.Find("PlayerDeath1").GetComponent<RawImage>();
    //    playerDeath2 = GameObject.Find("PlayerDeath2").GetComponent<RawImage>();
    //    playerDeath3 = GameObject.Find("PlayerDeath3").GetComponent<RawImage>();
    //}

    // get references to both playerhealth components -- these have synced health values
    void GetPlayers()
    {
        if (player1Health == null)
        {
            GameObject p1 = GameObject.FindGameObjectWithTag("Player");
            if (p1 != null)
            {
                player1Health = p1.GetComponent<PlayerHealth>();
            }
        }

        if (player2Health == null)
        {
            GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
            if (p2 != null)
            {
                player2Health = p2.GetComponent<PlayerHealth>();
            }
        }

        if (player1Health != null && player2Health != null)
        {
            found = true;
        }


    }

    // set alpha of active modifiers to 255, inactive to 20
    void SetModifierImages()
    {
        string tag1;
        string tag2;
        string tag3;

        string tag7;
        string tag8;
        string tag9;

        if (player1Health.isLocalPlayer)
        {
            tag1 = "EnemyDeath1";
            tag2 = "EnemyDeath2";
            tag3 = "EnemyDeath3";

            tag7 = "PlayerDeath1";
            tag8 = "PlayerDeath2";
            tag9 = "PlayerDeath3";
        }
        else
        {
            tag1 = "PlayerDeath1";
            tag2 = "PlayerDeath2";
            tag3 = "PlayerDeath3";

            tag7 = "EnemyDeath1";
            tag8 = "EnemyDeath2";
            tag9 = "EnemyDeath3";
        }

        if (!isSinglePlayer)
        {
            // player 2 deaths
            if (player2Health.numDeaths >= 1)
            {
                GameObject.Find(tag1).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag1).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }

            if (player2Health.numDeaths >= 2)
            {
                GameObject.Find(tag2).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag2).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }

            if (player2Health.numDeaths >= 3)
            {
                GameObject.Find(tag3).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag3).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }

        }






        /////////// Player 1 //////////////////

        // player 1 deaths
        if (player1Health.numDeaths >= 1)
        {
            GameObject.Find(tag7).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag7).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }

        if (player1Health.numDeaths >= 2)
        {
            GameObject.Find(tag8).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag8).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }

        if (player1Health.numDeaths >= 3)
        {
            GameObject.Find(tag9).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag9).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }


    }
}
