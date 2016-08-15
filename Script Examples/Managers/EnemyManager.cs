using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public bool secondPlayer;
    PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public float initialSpawnTime;
    public Transform[] spawnPoints;
    public int maxNum;
    public string name;
    public TimerManager timerManager;

    float timer;
    float initialTimer;
    TurnManager turnManager;
    bool found;
    bool spawning;
    bool starting;
    Transform player;
    LobbyManager lobbyManager;

    //public void Stop()
    //{
    //    StopCoroutine("Timer");
    //}

    void Start ()
    {
        found = false;
        spawning = false;
//        starting = false;
        timer = spawnTime;
        initialTimer = initialSpawnTime;
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
//        StartCoroutine("Timer");
    }

    void Update()
    {
        if (!found)
        {
            if (lobbyManager.done)
            {
                if (lobbyManager.isSinglePlayer)
                {
                    player = GameObject.FindGameObjectWithTag("Player").transform;
                }
                found = CheckForPlayer();
                //                if (found)
                //                {           
                ////                    DisableIrrelevantManager();
                //                    if (enabled)
                //                    {
                //                        if (!starting)
                //                        {
                //                            StartCoroutine("StartTimer");
                //                            starting = true;
                //                        }
                //                        Debug.Log("Spawning is: " + spawning);
                //                        Debug.Log("Starting is: " + starting);
                //                        if (!spawning && starting)
                //                        {
                //                            Debug.Log("Initial spawn timer for: " + enemy.ToString());
                //                            StartCoroutine("Timer");
                //                            spawning = true;
                //                        }

                //                    }

                //                }
            }

        }
        else
        {
            if (enabled)
            {
                if (timerManager.timer >= initialSpawnTime && !spawning)
                {
                    timer = 0;
                    StartCoroutine("Timer");
                    spawning = true;
                }
            }
        }
        //        if (!starting)
        //        {
        //            StartCoroutine("StartTimer");
        //            starting = true;
        //        }
        //        Debug.Log("Spawning is: " + spawning);
        //        Debug.Log("Starting is: " + starting);
        //        if (!spawning && starting)
        //        {
        //            Debug.Log("Initial spawn timer for: " + enemy.ToString());
        //            StartCoroutine("Timer");
        //            spawning = true;
        //        }

        //    }
        //}
    }

//    IEnumerator StartTimer()
//    {
//        while (initialTimer > 0)
//        {
//            if (!turnManager.turn)
//            {
//                initialTimer -= Time.deltaTime;
////                Debug.Log("Initial Timer decreased: " + initialTimer);
//            }
//            yield return null;
//        }
//        spawning = false;
//    }

    void DisableIrrelevantManager()
    {
        if (secondPlayer)
        {
            if (!GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerHealth>().isLocalPlayer)
            {
                enabled = false;
            }
        }
        else if (!secondPlayer)
        {
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().isLocalPlayer)
            {
                enabled = false;
            }
        }
    }

    bool CheckForPlayer()
    {
        string tag;
        if (secondPlayer)
        {
            tag = "Player2";
        }
        else
        {
            tag = "Player";
        }
        player = GameObject.FindGameObjectWithTag(tag).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            return false;
        }
        else
        {
            if (secondPlayer)
            {
                turnManager = GameObject.FindGameObjectWithTag(tag).GetComponent<TurnManager>();
            }
            else
            {
               turnManager = GameObject.FindGameObjectWithTag(tag).GetComponent<TurnManager>();
            }
            DisableIrrelevantManager();
            return true;
        }
    }

    IEnumerator Timer()
    {
        while (true)
        {
            if (timer > 0)
            {
                if (!turnManager.turn)
                {
                    timer -= Time.deltaTime;
                }
                yield return null;
                continue;
            }
            else 
            {
                Spawn();
                yield return null;
                continue;
            }
        }
    }    

    void Spawn ()
    {
        Debug.Log("Spawn timer for: " + enemy.ToString());
        int numEnemies = 0;
        GameObject[] enemyNumber = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyNumber != null)
        {
            foreach (GameObject e in enemyNumber)
            {
                if (e.name == name) { numEnemies++; }
            }
        }
        //// changes here
        while (numEnemies < maxNum)
        {
            if(playerHealth.currentHealth <= 0f)
            {
                timer = spawnTime;
                return;
            }

            int spawnPointIndex = Random.Range (0, spawnPoints.Length);

            GameObject e = Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation) as GameObject;
            e.name = name;
            e.GetComponent<EnemyMovement>().isSecond = secondPlayer;
            e.GetComponent<EnemyAttack>().isSecond = secondPlayer;
            e.GetComponent<EnemyMovement>().ready = true;
            e.GetComponent<EnemyAttack>().ready = true;
            numEnemies++;
        }
        timer = spawnTime;
    }

    public void EnemyDied()
    {
        //numEnemies--;
        //Debug.Log("Enemy died. Number left: " + numEnemies);
    }
}
