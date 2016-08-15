using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    public bool isSecond;
    public bool ready = false;
    public float attackWait = 1.5f;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
    TurnManager turnManager;


    void Awake ()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }

    void GetPlayerComponents()
    {
        if (isSecond)
        {
            player = GameObject.FindGameObjectWithTag("Player2");
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerHealth = player.GetComponent<PlayerHealth>();
        turnManager = player.GetComponent<TurnManager>();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject == player)
    //    {
    //        StartCoroutine("AttackWait");
    //    }
    //}

    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
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
            timer += Time.deltaTime;

            if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
            {
                Attack();
            }

            if (playerHealth.currentHealth <= 0)
            {
                anim.SetTrigger("PlayerDead");
            }
        }
    }

    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
        StartCoroutine("AttackWait");
    }

    IEnumerator AttackWait()
    {
        NavMeshAgent nvAgent = GetComponent<NavMeshAgent>();
        float oldSpeed = nvAgent.speed;
        float oldAngSpeed = nvAgent.angularSpeed;
        nvAgent.speed = 0f;
        nvAgent.velocity = Vector3.zero;
        nvAgent.angularSpeed = 0f;
        yield return new WaitForSeconds(attackWait);
        nvAgent.speed = oldSpeed;
        nvAgent.angularSpeed = oldAngSpeed;
    }
}
