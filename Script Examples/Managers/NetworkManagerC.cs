using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManagerC : NetworkManager {

//    void Awake()
//    {
////        networkText = GameObject.FindGameObjectWithTag("NetworkText").GetComponent<Text>();
//    }

    private bool hosting = false;

    public void StartupHost()
    {
        Debug.Log("host start");
        if (!hosting)
        {
            SetPort();
            NetworkManager.singleton.StartHost();
            hosting = true;
        }

    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 4444;
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.FindChild("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            hosting = false;
            SetupMenuSceneButtons();
        }
        else
        {
            SetupOtherSceneButtons();
        }
    }

    public void SetupMenuSceneButtons()
    {
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);

    }

    public void SetupOtherSceneButtons()
    {
        Debug.Log("setup other scene");
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(LoadMenu);

        GameObject back = GameObject.Find("ButtonBackToMenu");
        back.GetComponent<Button>().onClick.AddListener(LoadMenu);
        back.SetActive(false);


    }

    public void LoadMenu()
    {
        NetworkManager.singleton.StopHost();
        Application.LoadLevel(0);
    }

}
