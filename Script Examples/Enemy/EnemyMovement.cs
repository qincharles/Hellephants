using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public bool isSecond;
    public bool ready = false;

    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
    TurnManager turnManager;

    Vector3 speedHolder = Vector3.zero;
    bool resetAfterTurn = false;

    void Awake ()
    {
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }

    void GetPlayerComponents()
    {
        if (isSecond)
        {
            player = GameObject.FindGameObjectWithTag("Player2").transform;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        playerHealth = player.GetComponent<PlayerHealth>();
        turnManager = player.GetComponent<TurnManager>();
    }


    void Update ()
    {
        if (player == null)
        {
            if (ready)
            {
               GetPlayerComponents();
            }
            return;
        }
        if (!turnManager.turn)
        {
            if (resetAfterTurn)
            {
                nav.enabled = true;
                nav.velocity = speedHolder;
                speedHolder = Vector3.zero;
                resetAfterTurn = false;
            }
            if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
                nav.SetDestination(player.position);
            }
            else
            {
                nav.enabled = false;
            }
        }
        else
        {
            if (speedHolder == Vector3.zero)
            {
                speedHolder = nav.velocity;
                resetAfterTurn = true;
            }

            nav.enabled = false;
        }
    }
}
