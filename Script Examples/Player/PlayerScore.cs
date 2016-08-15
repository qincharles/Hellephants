using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour {

    [SyncVar]
    public int score;

    public void ChangeScore(int v)
    {
        CmdScore(score + v);
//        score += v;
    }

    [Command]
    void CmdScore(int v)
    {
        score = v;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
