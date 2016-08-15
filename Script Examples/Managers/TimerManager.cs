using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TimerManager : NetworkBehaviour {

    [SyncVar]
    public float timer = 0;

    //public float bunnyInitialSpawn;
    //public float bearInitialSpawn;
    //public float hellephantInitialSpawn;

    public LobbyManager lobbyManager;
    public Text timerText;
    public GameOverManager gameOverManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (lobbyManager.done)
        {
            if (isServer && !gameOverManager.gameOver) { CmdChangeTime(); }
            timerText.text = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);
        }
	}

    void CmdChangeTime()
    {
        timer += Time.deltaTime;
    }
}
