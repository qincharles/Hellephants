using UnityEngine;
using System.Collections;

public class BombExplode : MonoBehaviour {

    public GameObject explosion;
    public float damRadius; // radius of aoe damage effect
    public int hitDamage;   // damage done by aoe
    public float waitTime;  // time to wait after getting hit
    public int healAmount; // amount healed on bomb kill

    bool hit;               // is the bomb in the process of exploding
    public GameObject player;
    AudioSource explosionAudio;
    PlayerShooting playerShooting;
    PlayerHealth playerHealth;
    float damageMultiplier = 1;

//    public float minRadius; // minimum distance to trigger other bomb

	// Use this for initialization
	void Awake () {
        hit = false;
        explosionAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerShooting == null)
        {
            playerShooting = player.GetComponent<PlayerShooting>();
        }
        if (playerHealth == null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        
	}

    IEnumerator Explosion()
    {
        float stepVal = .05f;

        GameObject exp = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
//        MeshRenderer mr = exp.GetComponent<MeshRenderer>() as MeshRenderer;
//        Debug.Log("meshrenderer shared material is: " + mr.sharedMaterial.ToString());
        ExplosionMat expmat = exp.GetComponent<ExplosionMat>();
//        float tempalpha = expmat._alpha;
        explosionAudio.Play();
        while (expmat._alpha > 0)
        {
//            Debug.Log(expmat._alpha);
//            float tempcolor =  exp.GetComponent<Renderer>().material.color.a;
            expmat.SetAlpha(Mathf.MoveTowards(expmat._alpha, 0, stepVal));
  //          exp.GetComponent<Renderer>().material.color = tempcolor;
            yield return null;
        }
//        transform.gameObject.SetActive(false);
        Destroy(transform.gameObject, .01f);
        Destroy(exp, .1f);
    }

    public void Explode(bool wait)
    {
        // find any object that takes damage from the explosion

        // show the explosion
//        GameObject exp = Instantiate(explosion, transform.position, transform.rotation) as GameObject;

        // destroy bomb and explosion
        //Destroy(transform.gameObject, 2f);

        // don't do anything if already hit
        if (hit) { return; }

        hit = true;
        StartCoroutine("WaitExplode", wait);
//        DoDamage();
//        StartCoroutine("Explosion");
//        transform.gameObject.SetActive(false);
    }

    IEnumerator WaitExplode(bool wait)
    {
        if (wait)
        {
            yield return new WaitForSeconds(waitTime);
        }
        DoDamage();
        StartCoroutine("Explosion");
    }

    void DoDamage()
    {
        // check all living enemies for their distance
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // damage all enemies inside the damage radius
        foreach (GameObject e in enemies)
        {
            // grab distance from bomb
            float distance = (e.transform.position - transform.position).magnitude;
            if (distance <= damRadius && e != null)
            {
                float damage = hitDamage;
                if (playerShooting.atkBuff)
                {
                    damage *= playerShooting.atkBuffMultiplier;
                }
                if (playerShooting.atkDebuff)
                {
                    damage *= playerShooting.atkDebuffMultiplier;
                }
                int scoreChange = e.GetComponent<EnemyHealth>().TakeDamage((int)damage, e.transform.position);
                if (scoreChange != 0)
                {
                    player.GetComponent<PlayerScore>().ChangeScore(scoreChange);
                    playerHealth.GainHealth(healAmount);
                }
            }
        }

        // check all living bombs for their distance
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

        // trigger explosion for all bombs inside the damage radius
        foreach (GameObject b in bombs)
        {
            // ignore the current bomb
            if (b != transform.gameObject)
            {
                // grab distance
                float distance = (b.transform.position - transform.position).magnitude;
                if (distance <= (damRadius * 2.05f))
                {
                    if (b != null)
                    {
                        b.GetComponent<BombExplode>().Explode(true);
                    }
                   
                }
            }
        }
    }
}
