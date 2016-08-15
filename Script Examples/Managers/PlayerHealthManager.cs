using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour {

    public bool isSinglePlayer;

    bool found = false;
    PlayerHealth player1;
    PlayerHealth player2;
    GameOverManager gameOverManager;

	// Use this for initialization
	void Start () {
        gameOverManager = GameObject.Find("HUDCanvas").GetComponent<GameOverManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!found) 
        {
            if (isSinglePlayer)
            {
                player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
                found = true;
                //GameObject.Find("Camera1").GetComponent<Camera>().enabled = true;
                //GameObject.Find("Camera1").GetComponent<Camera>().GetComponent<AudioListener>().enabled = true;
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
                player1 = p1.GetComponent<PlayerHealth>();
            }
        }

        if (player2 == null)
        {
            GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
            if (p2 != null)
            {
                player2 = p2.GetComponent<PlayerHealth>();
            }
        }

        if (player1 != null && player2 != null)
        {
            found = true;
//            EnableCamera();
        }
        

    }

    //void EnableCamera()
    //{
    //    if (player1.isLocalPlayer)
    //    {
    //        GameObject.Find("Camera1").GetComponent<Camera>().enabled = true;
    //        GameObject.Find("Camera1").GetComponent<Camera>().GetComponent<AudioListener>().enabled = true;

    //        GameObject.Find("Camera2").GetComponent<Camera>().enabled = false;
    //        GameObject.Find("Camera2").GetComponent<Camera>().GetComponent<AudioListener>().enabled = false;
    //    }
    //    else
    //    {
    //        GameObject.Find("Camera1").GetComponent<Camera>().enabled = false;
    //        GameObject.Find("Camera1").GetComponent<Camera>().GetComponent<AudioListener>().enabled = false;

    //        GameObject.Find("Camera2").GetComponent<Camera>().enabled = true;
    //        GameObject.Find("Camera2").GetComponent<Camera>().GetComponent<AudioListener>().enabled = true;
    //    }
    //}

    // set the green health slider to the local player's health value and the red to the other's
    void SetSliders()
    {
        if (isSinglePlayer)
        {
            GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>().value = player1.currentHealth;
            if (player1.numDeaths >= 3)
            {
                Debug.Log("Game Over");
                gameOverManager.GameOver(GameOverManager.GameOverState._Player1Win);
            }
        }
        else
        {
            string tag1;
            string tag2;
            if (player1.isLocalPlayer)
            {
                tag1 = "HealthSlider";
                tag2 = "HealthSlider2";
            }
            else
            {
                tag1 = "HealthSlider2";
                tag2 = "HealthSlider";
            }

            GameObject.FindGameObjectWithTag(tag1).GetComponent<Slider>().value = player1.currentHealth;
            GameObject.FindGameObjectWithTag(tag2).GetComponent<Slider>().value = player2.currentHealth;

            //Debug.Log("Player1: " + player1.currentHealth);
            //Debug.Log("Player2: " + player2.currentHealth);

            // call gameovermanager if a player has died 3 times
            if (player1.numDeaths >= 3 && player2.numDeaths < 3)
            {
                Debug.Log("Player2 wins");
                gameOverManager.GameOver(GameOverManager.GameOverState._Player2Win);
            }
            else if (player2.numDeaths >= 3 && player1.numDeaths < 3)
            {
                Debug.Log("Player1 wins");
                gameOverManager.GameOver(GameOverManager.GameOverState._Player1Win);
            }
            else if (player1.numDeaths >= 3 && player2.numDeaths >= 3)
            {
                gameOverManager.GameOver(GameOverManager.GameOverState._Tie);
            }
        }

    }
}
