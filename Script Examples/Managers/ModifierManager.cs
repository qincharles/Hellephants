using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModifierManager : MonoBehaviour {

    public bool isSinglePlayer;

    bool found = false;
    PlayerHealth player1Health;
    PlayerHealth player2Health;
    PlayerShooting player1Shooting;
    PlayerShooting player2Shooting;
    PlayerMovement player1Movement;
    PlayerMovement player2Movement;
    LobbyManager lobbyManager;


    //RawImage enemyAtkUp;
    //RawImage enemyAtkDown;
    //RawImage enemyDefUp;
    //RawImage enemyDefDown;
    //RawImage enemySpdUp;
    //RawImage enemySpdDown;
    //RawImage playerAtkUp;
    //RawImage playerAtkDown;
    //RawImage playerDefUp;
    //RawImage playerDefDown;
    //RawImage playerSpdUp;
    //RawImage playerSpdDown;

    void Start()
    {
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
//        GetImages();


    }

    void SetUpModifierButtons()
    {
        if (player1Health.isLocalPlayer) {
            if (!isSinglePlayer)
            {
                GameObject.Find("ButtonEnemyAttack").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("ButtonEnemyAttack").GetComponent<Button>().onClick.AddListener(player2Shooting.SetDebuff);

                GameObject.Find("ButtonEnemyDefense").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("ButtonEnemyDefense").GetComponent<Button>().onClick.AddListener(player1Health.CmdGiveDebuff);

                GameObject.Find("ButtonEnemySpeed").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("ButtonEnemySpeed").GetComponent<Button>().onClick.AddListener(player2Movement.SetDebuff);
            }


            GameObject.Find("ButtonPlayerAttack").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("ButtonPlayerAttack").GetComponent<Button>().onClick.AddListener(player1Shooting.SetBuff);

            GameObject.Find("ButtonPlayerDefense").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("ButtonPlayerDefense").GetComponent<Button>().onClick.AddListener(player1Health.SetBuff);

            GameObject.Find("ButtonPlayerSpeed").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("ButtonPlayerSpeed").GetComponent<Button>().onClick.AddListener(player1Movement.SetBuff);
        }
        else
        {
            if (!isSinglePlayer)
            {
                GameObject.Find("ButtonEnemyAttack").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("ButtonEnemyAttack").GetComponent<Button>().onClick.AddListener(player1Shooting.SetDebuff);

                GameObject.Find("ButtonEnemyDefense").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("ButtonEnemyDefense").GetComponent<Button>().onClick.AddListener(player2Health.CmdGiveDebuff);

                GameObject.Find("ButtonEnemySpeed").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("ButtonEnemySpeed").GetComponent<Button>().onClick.AddListener(player1Movement.SetDebuff);
            }


            GameObject.Find("ButtonPlayerAttack").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("ButtonPlayerAttack").GetComponent<Button>().onClick.AddListener(player2Shooting.SetBuff);

            GameObject.Find("ButtonPlayerDefense").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("ButtonPlayerDefense").GetComponent<Button>().onClick.AddListener(player2Health.SetBuff);

            GameObject.Find("ButtonPlayerSpeed").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("ButtonPlayerSpeed").GetComponent<Button>().onClick.AddListener(player2Movement.SetBuff);
        }
    }

    //void GetImages()
    //{
    //    if (!isSinglePlayer)
    //    {
    //        enemyAtkUp = GameObject.Find("EnemyAtkUp").GetComponent<RawImage>();
    //        enemyAtkDown = GameObject.Find("EnemyAtkDown").GetComponent<RawImage>();
    //        enemyDefUp = GameObject.Find("EnemyDefUp").GetComponent<RawImage>();
    //        enemyDefDown = GameObject.Find("EnemyDefDown").GetComponent<RawImage>();
    //        enemySpdUp = GameObject.Find("EnemySpdUp").GetComponent<RawImage>();
    //        enemySpdDown = GameObject.Find("EnemySpdDown").GetComponent<RawImage>();
    //    }

    //    playerAtkUp = GameObject.Find("PlayerAtkUp").GetComponent<RawImage>();
    //    playerAtkDown = GameObject.Find("PlayerAtkDown").GetComponent<RawImage>();
    //    playerDefUp = GameObject.Find("PlayerDefUp").GetComponent<RawImage>();
    //    playerDefDown = GameObject.Find("PlayerDefDown").GetComponent<RawImage>();
    //    playerSpdUp = GameObject.Find("PlayerSpdUp").GetComponent<RawImage>();
    //    playerSpdDown = GameObject.Find("PlayerSpdDown").GetComponent<RawImage>();
    //}

    // Update is called once per frame
    void Update()
    {
        if (!found)
        {
            if (isSinglePlayer)
            {
                GameObject p1 = GameObject.FindGameObjectWithTag("Player");
                player1Health = p1.GetComponent<PlayerHealth>();
                player1Movement = p1.GetComponent<PlayerMovement>();
                player1Shooting = p1.GetComponent<PlayerShooting>();
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

    // get references to both playerhealth components -- these have synced health values
    void GetPlayers()
    {
        if (player1Health == null)
        {
            GameObject p1 = GameObject.FindGameObjectWithTag("Player");
            if (p1 != null)
            {
                player1Health = p1.GetComponent<PlayerHealth>();
                player1Movement = p1.GetComponent<PlayerMovement>();
                player1Shooting = p1.GetComponent<PlayerShooting>();
            }
        }

        if (player2Health == null)
        {
            GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
            if (p2 != null)
            {
                player2Health = p2.GetComponent<PlayerHealth>();
                player2Movement = p2.GetComponent<PlayerMovement>();
                player2Shooting = p2.GetComponent<PlayerShooting>();
            }
        }

        if (player1Health != null && player2Health != null)
        { 
            found = true;
 //           SetUpModifierButtons();
        }


    }

    // set alpha of active modifiers to 255, inactive to 20
    void SetModifierImages()
    {
        string tag1;
        string tag2;
        string tag3;
        string tag4;
        string tag5;
        string tag6;
        string tag7;
        string tag8;
        string tag9;
        string tag10;
        string tag11;
        string tag12;
        if (player1Health.isLocalPlayer)
        {
            tag1 = "EnemyAtkUp";
            tag2 = "EnemyAtkDown";
            tag3 = "EnemyDefUp";
            tag4 = "EnemyDefDown";
            tag5 = "EnemySpdUp";
            tag6 = "EnemySpdDown";

            tag7 = "PlayerAtkUp";
            tag8 = "PlayerAtkDown";
            tag9 = "PlayerDefUp";
            tag10 = "PlayerDefDown";
            tag11 = "PlayerSpdUp";
            tag12 = "PlayerSpdDown";
        }
        else
        {
            tag1 = "PlayerAtkUp";
            tag2 = "PlayerAtkDown";
            tag3 = "PlayerDefUp";
            tag4 = "PlayerDefDown";
            tag5 = "PlayerSpdUp";
            tag6 = "PlayerSpdDown";

            tag7 = "EnemyAtkUp";
            tag8 = "EnemyAtkDown";
            tag9 = "EnemyDefUp";
            tag10 = "EnemyDefDown";
            tag11 = "EnemySpdUp";
            tag12 = "EnemySpdDown";
        }

        if (!isSinglePlayer)
        {
            // player 2 attack
            if (player2Shooting.atkBuff)
            {
                GameObject.Find(tag1).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag1).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }

            if (player2Shooting.atkDebuff)
            {
                GameObject.Find(tag2).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag2).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }


            // player2 defense

            if (player2Health.defBuff)
            {
                GameObject.Find(tag3).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag3).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }

            if (player2Health.defDebuff)
            {
                GameObject.Find(tag4).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag4).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }


            // player2 speed

            if (player2Movement.spdBuff)
            {
                GameObject.Find(tag5).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag5).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }

            if (player2Movement.spdDebuff)
            {
                GameObject.Find(tag6).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
            }
            else
            {
                GameObject.Find(tag6).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
            }
        }






        /////////// Player 1 //////////////////

        if (player1Shooting.atkBuff)
        {
            GameObject.Find(tag7).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag7).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }

        if (player1Shooting.atkDebuff)
        {
            GameObject.Find(tag8).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag8).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }



        // player 1 defense
        if (player1Health.defBuff)
        {
            GameObject.Find(tag9).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag9).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }

        if (player1Health.defDebuff)
        {
            GameObject.Find(tag10).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag10).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }




        // player 1 speed
        if (player1Movement.spdBuff)
        {
            GameObject.Find(tag11).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag11).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }

        if (player1Movement.spdDebuff)
        {
            GameObject.Find(tag12).GetComponent<RawImage>().CrossFadeAlpha(20f, .01f, true);
        }
        else
        {
            GameObject.Find(tag12).GetComponent<RawImage>().CrossFadeAlpha(.1f, .01f, true);
        }
    }
}
