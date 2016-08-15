using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerHealth : NetworkBehaviour
{
    public int startingHealth = 100;
    [SyncVar]
    public int currentHealth;
    [SyncVar]
    public int numDeaths;
    Slider healthSlider;
    RawImage damageImage;
    public AudioClip deathClip;
    public AudioClip hurtClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public bool secondPlayer;
    [SyncVar]
    public bool defBuff;
    [SyncVar]
    public bool defDebuff;
    public float defBuffMultiplier = .5f;
    public float defDebuffMultiplier = 3f;

    public float modifierTimer = 10f;
    public float respawnTimer = 3.5f;

    public bool gameOver = false;

    float timeOnBuff;
    float timeOnDebuff = -50;
    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    TurnManager turnManager;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
        turnManager = GetComponent<TurnManager>();
        //string tagHealth;
        //string tagImage;
        //if (secondPlayer)
        //{
        //    tagHealth = "HealthSlider2";
        //    tagImage = "DamageImage2";

        //}
        //else
        //{
        //    tagHealth = "HealthSlider";
        //    tagImage = "DamageImage";
        //}
        healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
        damageImage = GameObject.FindGameObjectWithTag("DamageImage").GetComponent<RawImage>();
    }


    void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        if (!turnManager.turn)
        {
            //buff timer
            if (timeOnBuff > 0)
            {
                timeOnBuff -= Time.deltaTime;
            }
            else
            {
                CmdSetBuff(false);
            }

            //debuff timer
            if (timeOnDebuff == -50 && defDebuff)
            {
                // special case when enemy called debuff on you. timer will not be active but bool is true
                timeOnDebuff = modifierTimer;
            }
            else if (timeOnDebuff > 0)
            {
                timeOnDebuff -= Time.deltaTime;
            }
            else
            {
                CmdRemoveDebuff();
            }
        }
        //Debug.Log("Health is: " + currentHealth);
        //Debug.Log("Dead is: " + isDead);

        //if (currentHealth <= 0 && !isDead)
        //{
        //    Death();
        //}
    }


    public void TakeDamage (int amount)
    {
        if (gameOver) { return; }
        damaged = true;
        float a = amount;
        if (defBuff)
        {
            a *= defBuffMultiplier;
        }
        if (defDebuff)
        {
            a *= defDebuffMultiplier;
        }
        float tempHealth = currentHealth - a;
        CmdSetHealth(currentHealth - (int)a);
//        currentHealth -= amount;
//        CmdDamage(amount);

//        healthSlider.value = currentHealth;

        playerAudio.Play ();

        if (tempHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void GainHealth(int amount)
    {
        if (gameOver) { return; }
        CmdSetHealth(currentHealth + amount);
    }

    [Command]
    void CmdSetHealth(int amount)
    {
        currentHealth = amount;
    }

    [Command]
    void CmdAddDeath()
    {
        numDeaths++;
    }

    //[Command]
    //void CmdDamage(int amount)
    //{
    //    currentHealth -= amount;
    //}


    void Death ()
    {
        Debug.Log("Death");
        isDead = true;

//        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
        GetComponent<PlayerBombing>().enabled = false;
        GetComponent<TurnManager>().enabled = false;

        // get ready for respawn - destroy all existing enemies and reset player
        CmdAddDeath();
        GameObject[] enemies = FindGameObjectsWithLayer(LayerMask.NameToLayer("Enemy"));
        foreach (GameObject e in enemies)
        {
            if (e.GetComponent<EnemyHealth>() != null)
            {
                e.GetComponent<EnemyHealth>().DestroyHealthBar();
            }

            GameObject.Destroy(e);
        }

//        while ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1 && anim.IsInTransition(0))) { }
//        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Player.Death")) {}

        if (numDeaths < 3)
        {
            StartCoroutine("Respawn");
        }
        


    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTimer);


        if (!secondPlayer)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            transform.position = new Vector3(75, 0, -10);
        }

        playerShooting.enabled = true;
        playerMovement.enabled = true;
        GetComponent<PlayerBombing>().enabled = true;
        GetComponent<TurnManager>().enabled = true;

        playerAudio.clip = hurtClip;
        CmdSetHealth(startingHealth);
        isDead = false;
    }


    public void RestartLevel()
    {
        Debug.Log("Restart level playerhealth");
//        Application.LoadLevel(Application.loadedLevel);
    }

    public void SetBuff()
    {
        CmdSetBuff(true);
        timeOnBuff = modifierTimer;
    }

    [Command]
    void CmdSetBuff(bool b)
    {
        defBuff = b;
    }

    [Command]
    public void CmdGiveDebuff()
    {
        string otherTag;
        if (gameObject.tag == "Player")
        {
            otherTag = "Player2";
        } 
        else
        {
            otherTag = "Player";
        }
        GameObject.FindGameObjectWithTag(otherTag).GetComponent<PlayerHealth>().SetDebuff();
    }

    //public void GiveDebuff()
    //{
    //    CmdGiveDebuff();
    //}

    public void SetDebuff()
    {
        defDebuff = true;
        timeOnDebuff = modifierTimer;
    }

    [Command]
    void CmdRemoveDebuff()
    {
        defDebuff = false;
        timeOnDebuff = -50f;
    }

    GameObject[] FindGameObjectsWithLayer(int layer)
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        Debug.Log("Num objects: " + goArray.Length);
        List<GameObject> goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer) { goList.Add(goArray[i]); }
        }
        if (goList.Count == 0) { return null; }
        return goList.ToArray();
    }
}
