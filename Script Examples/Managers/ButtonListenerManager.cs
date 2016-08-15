using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonListenerManager : MonoBehaviour {

    NetworkManagerC networkManager;

    public void LoadSinglePlayer()
    {
        networkManager.StartupHost();
        Application.LoadLevel(2);
    }

	// Use this for initialization
	void Start () {
        networkManager = GameObject.Find("Network").GetComponent<NetworkManagerC>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            networkManager = GameObject.Find("Network").GetComponent<NetworkManagerC>();
            networkManager.SetupMenuSceneButtons();
        }
        else
        {
            networkManager.SetupOtherSceneButtons();
        }
    }
}
