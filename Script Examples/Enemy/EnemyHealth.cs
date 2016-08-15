using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Transform hudCanvas;
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    public float timeBetweenBombs;
    public GameObject healthBarPrefab;

    GameObject healthBar;
    float timer;
    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;

        hudCanvas = GameObject.Find("HUDCanvas").transform;

        healthBar = Instantiate(healthBarPrefab) as GameObject;
        healthBar.transform.SetParent(hudCanvas, false);

        healthBar.GetComponent<Slider>().maxValue = startingHealth;
//        healthBar.GetComponent<Slider>().value = currentHealth;
    }

    //void OnGUI()
    //{
    //    Vector3 wantedPos = Camera.main.WorldToViewportPoint(transform.position);
    //    healthBar.transform.position = wantedPos;
    //}

    void Update ()
    {
        timer += Time.deltaTime;
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }

        // change healthbar
        Vector3 wantedPos = Camera.main.WorldToScreenPoint(transform.position);
        wantedPos.y += 50f;
        wantedPos.x += 5f;
        wantedPos.z = 0;
        if (healthBar != null)
        {
            healthBar.transform.position = wantedPos;

            healthBar.GetComponent<Slider>().value = currentHealth;
        }

    }

    // returns scorevalue if enemy is killed by this damage
    public int TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return 0;   // already dead, not killed by this damage

        if (hitPoint == transform.position)
        {
            // if the hitpoint is at the center, then it's a bomb. start the timer to protect against multiple bomb hits
            if (timer < timeBetweenBombs)
            {
                return 0;
            }
            // if timer is greater than timebetweenbombs, take the hit and restart the timer
            timer = 0f;
        }

        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
            return scoreValue;
        }
        return 0;
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();

        GameObject.Find(transform.name + "Spawns").GetComponent<EnemyManager>().EnemyDied();
    }

    public void DestroyHealthBar()
    {
        if (healthBar != null)
        {
            Destroy(healthBar, .1f);
        }
    }

    public void StartSinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
//        ScoreManager.score += scoreValue;
        Destroy(healthBar, .1f);
        Destroy (gameObject, 2f);
    }
}
