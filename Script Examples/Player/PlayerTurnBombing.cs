using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerTurnBombing : MonoBehaviour
{

    public GameObject bomb;
    public float timeBetweenBombs;
    public float bombActionCost;
    public bool isSecond;

    float timer;
    public TurnManager turnManager;
    List<GameObject> ghostBombs;
    


    // Use this for initialization
    void Awake()
    {
        string tag;
        if (isSecond)
        {
            tag = "Player2";
        }
        else
        {
            tag = "Player";
        }
        turnManager = GameObject.FindGameObjectWithTag(tag).GetComponent<TurnManager>();
        ghostBombs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}
        if (turnManager.executing == TurnManager.Executing._True) { return; }
        if (turnManager.turn)
        {
            timer += Time.deltaTime;

            if (Input.GetButton("Fire2") && timer >= timeBetweenBombs && Time.timeScale != 0)
            {
                //PlaceBomb();
                turnManager.AddActionToList(TurnManager.ActionType._Bomb, transform.position, bombActionCost);
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
        ghostBombs.Add(Instantiate(bomb, position, transform.rotation) as GameObject);
        if (isSecond)
        {
            ghostBombs[ghostBombs.Count - 1].GetComponent<BombExplode>().player = GameObject.FindGameObjectWithTag("Player2");
        }
        else
        {
            ghostBombs[ghostBombs.Count - 1].GetComponent<BombExplode>().player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void DestroyBombs()
    {
        foreach (GameObject b in ghostBombs)
        {
            Destroy(b, .05f);
        }
//        Debug.Log("Ghost bombs destroyed");
    }

    public void RemoveLast()
    {
        GameObject b = ghostBombs[ghostBombs.Count - 1];
        ghostBombs.RemoveAt(ghostBombs.Count - 1);
        Destroy(b, .05f);
    }
}

