using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    [SyncVar]
    public bool atkBuff;
    [SyncVar]
    public bool atkDebuff;
    public float atkBuffMultiplier = 2f;
    public float atkDebuffMultiplier = .3f;
    public float modifierTimer = 10f;

    float timeOnBuff;
    float timeOnDebuff = -50;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    TurnManager turnManager;
    Transform gunEnd;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable", "Enemy", "Bomb");
        gunEnd = transform.FindChild("GunBarrelEnd");
        gunParticles = gunEnd.gameObject.GetComponent<ParticleSystem> ();
        gunLine = gunEnd.gameObject.GetComponent<LineRenderer> ();
        gunAudio = gunEnd.gameObject.GetComponent<AudioSource> ();
        gunLight = gunEnd.gameObject.GetComponent<Light> ();
        turnManager = gameObject.GetComponent<TurnManager>();        
    }


    void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (!turnManager.turn)
        {
            timer += Time.deltaTime;

            if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            {
                Shoot();
            }

            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects();
            }


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
            if (timeOnDebuff == -50 && atkDebuff)
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
        
    }

    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, gunEnd.position);

        shootRay.origin = gunEnd.position;
        shootRay.direction = gunEnd.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            if (shootHit.collider.tag == "Bomb")
            {
                BombExplode explodeScript = shootHit.collider.GetComponent<BombExplode>();
                explodeScript.Explode(false);
                gunLine.SetPosition(1, shootHit.point);
            }
            else
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    float damage = damagePerShot;
                    if (atkBuff)
                    {
                        damage *= atkBuffMultiplier;
                    }
                    if (atkDebuff)
                    {
                        damage *= atkDebuffMultiplier;
                    }
                    int scoreChange = enemyHealth.TakeDamage((int)damage, shootHit.point);
                    if (scoreChange != 0)
                    {
                        GetComponent<PlayerScore>().ChangeScore(scoreChange);
                    }
                }
                gunLine.SetPosition(1, shootHit.point);
            }
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }

    public void SetBuff()
    {
        CmdSetBuff(true);
        timeOnBuff = modifierTimer;
    }

    [Command]
    void CmdSetBuff(bool b)
    {
        atkBuff = b;
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
        GameObject.FindGameObjectWithTag(otherTag).GetComponent<PlayerShooting>().SetDebuff();
    }

    public void SetDebuff()
    {
        atkDebuff = true;
        timeOnDebuff = modifierTimer;
    }

    [Command]
    void CmdRemoveDebuff()
    {
        atkDebuff = false;
        timeOnDebuff = -50f;
    }
}
