using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerBombing : NetworkBehaviour {

    public GameObject bomb;
    public float timeBetweenBombs;

    float timer;
    TurnManager turnManager;


	// Use this for initialization
	void Awake () {
//        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        turnManager = gameObject.GetComponentInChildren<TurnManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        if (!turnManager.turn)
        {
            timer += Time.deltaTime;

            if (Input.GetButton("Fire2") && timer >= timeBetweenBombs && Time.timeScale != 0)
            {
                PlaceBomb();
            }

            //if (timer >= timeBetweenBullets * effectsDisplayTime)
            //{
            //    DisableEffects();
            //}
        }
	}

    public void PlaceBomb()
    {
        // need to adjust for y position
        timer = 0f;
        Vector3 position = transform.position;
        position.y = .45f;
        GameObject b = Instantiate(bomb, position, transform.rotation) as GameObject;
        b.GetComponent<BombExplode>().player = gameObject;
    }
}
