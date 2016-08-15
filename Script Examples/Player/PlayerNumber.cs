using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNumber : NetworkBehaviour {

    public void SetPlayerNumber(bool isSecond)
    {
        if (isSecond)
        {
            gameObject.tag = "Player2";
            GetComponent<PlayerHealth>().secondPlayer = true;
            GetComponent<TurnManager>().secondPlayer = true;
            GetComponent<CameraFollow>().secondPlayer = true;
            GetComponent<PlayerModifierButtons>().isSecond = true;
            transform.position = new Vector3(75, 0, -10);
            if (isLocalPlayer)
            {
                GameObject c = Camera.main.gameObject;
                c.transform.position = new Vector3(75, 15, -34);
                GetComponent<CameraFollow>().SetCamera(c);
                GetComponent<PlayerMovement>().camera = c.GetComponent<Camera>();
            }

//            GetComponent<PlayerMovement>().camera = GameObject.Find("Camera2").GetComponent<Camera>();
            //GameObject.Find("ZombunnySpawns").GetComponent<EnemyManager>().Stop();
            //GameObject.Find("ZomBearSpawns").GetComponent<EnemyManager>().Stop();
            //GameObject.Find("HellephantSpawns").GetComponent<EnemyManager>().Stop();

//            GameObject.Find("Camera1").GetComponent<Camera>().enabled = false;
//            GameObject.Find("Camera2").GetComponent<Camera>().enabled = true;


//            transform.position = new Vector3(75, 0, -10);
        }
        else
        {
            gameObject.tag = "Player";
            GetComponent<PlayerHealth>().secondPlayer = false;
            GetComponent<TurnManager>().secondPlayer = false;
            GetComponent<CameraFollow>().secondPlayer = false;

            if (isLocalPlayer)
            {
                GameObject c = Camera.main.gameObject;
                GetComponent<CameraFollow>().SetCamera(c);
                GetComponent<PlayerMovement>().camera = c.GetComponent<Camera>();
            }



            //GameObject.Find("ZombunnySpawns2").GetComponent<EnemyManager>().Stop();
            //GameObject.Find("ZomBearSpawns2").GetComponent<EnemyManager>().Stop();
            //GameObject.Find("HellephantSpawns2").GetComponent<EnemyManager>().Stop();

//            GameObject.Find("Camera2").GetComponent<Camera>().enabled = false;
//            GameObject.Find("Camera1").GetComponent<Camera>().enabled = true;


            transform.position = Vector3.zero;
        }
    }

	// Use this for initialization
	void Awake () {
//        SetPlayerNumber(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
