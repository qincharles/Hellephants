using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerModifierButtons : NetworkBehaviour {


    // debuffs only available past minute 3

    public TimerManager timerManager;
    public int debuffTime = 180;

    public int atkBuffCost;
    public int atkDebuffCost;

    public int defBuffCost;
    public int defDebuffCost;

    public int spdBuffCost;
    public int spdDebuffCost;

    public bool isSecond;

    PlayerScore playerScore;

	// Use this for initialization
	void Start () {
        playerScore = GetComponent<PlayerScore>();
        timerManager = GameObject.Find("TimerManager").GetComponent<TimerManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) {
            return;
        }


        // debuffs only available past given time
        if (timerManager.timer >= debuffTime)
        {
            //// Enemy Attack Debuff
            if (Input.GetButton("EnemyAttack"))
            {
                if (playerScore.score >= atkDebuffCost)
                {
                    playerScore.score -= atkDebuffCost;
                    if (isSecond)
                    {
                        GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerShooting>().CmdGiveDebuff();
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>().CmdGiveDebuff();
                    }
                }
            }


            // Enemy Defense Debuff
            if (Input.GetButton("EnemyDefense"))
            {
                if (playerScore.score >= defDebuffCost)
                {
                    playerScore.score -= defDebuffCost;
                    if (isSecond)
                    {
                        GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerHealth>().CmdGiveDebuff();
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().CmdGiveDebuff();
                    }
                }
            }



            // Enemy Speed Debuff
            if (Input.GetButton("EnemySpeed"))
            {
                if (playerScore.score >= spdDebuffCost)
                {
                    playerScore.score -= spdDebuffCost;
                    if (isSecond)
                    {
                        GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>().CmdGiveDebuff();
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().CmdGiveDebuff();
                    }
                }
            }
        }




        // Player Attack Buff
        if (Input.GetButton("PlayerAttack"))
        {
            if (playerScore.score >= atkBuffCost)
            {
                playerScore.score -= atkBuffCost;
                if (isSecond)
                {
                    GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerShooting>().SetBuff();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>().SetBuff();
                }
            }
        }



        // Player Defense Buff
        if (Input.GetButton("PlayerDefense"))
        {
            if (playerScore.score >= defBuffCost)
            {
                playerScore.score -= defBuffCost;
                if (isSecond)
                {
                    GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerHealth>().SetBuff();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().SetBuff();
                }
            }
        }


        // Player speed buff
        if (Input.GetButton("PlayerSpeed"))
        {
            Debug.Log("PlayerSpeedButton");
            if (playerScore.score >= spdBuffCost)
            {
                playerScore.score -= spdBuffCost;
                if (isSecond)
                {
                    GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>().SetBuff();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SetBuff();
                }
            }
        }
	}
}
