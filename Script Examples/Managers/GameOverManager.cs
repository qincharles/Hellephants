using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public bool gameOver = false;

    public bool isSinglePlayer;
    public bool secondPlayer;
    PlayerHealth player1;
    PlayerHealth player2;
    bool found;
    LobbyManager lobbyManager;
    Text gameOverText;
    GameObject back;

    Animator anim;

    public enum GameOverState { _Player1Win, _Player2Win, _Tie }

    // false for player 1 victory, true for player 2 victory
    public void GameOver(GameOverState winner)
    {
        if (isSinglePlayer)
        {
            gameOverText.text = "Game Over!";
        }
        else
        {
            if (winner == GameOverState._Player2Win)
            {
                if (player2.isLocalPlayer)
                {
                    gameOverText.text = "You win!";
                }
                else
                {
                    gameOverText.text = "You Lose...";
                }
            }
            else if (winner == GameOverState._Player1Win)
            {
                if (player1.isLocalPlayer)
                {
                    gameOverText.text = "You win!";
                }
                else
                {
                    gameOverText.text = "You Lose...";
                }
            }
            else
            {
                gameOverText.text = "You both died! It's a tie!";
            }

            player2.gameOver = true;

        }
        anim.SetTrigger("GameOver");
        player1.gameOver = true;
        back.SetActive(true);
        gameOver = true;
    }

    void Awake()
    {
        found = false;
        anim = GetComponent<Animator>();
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        back = GameObject.Find("ButtonBackToMenu");
    }

    void GetPlayers()
    {
        if (player1 == null)
        {
            GameObject p1 = GameObject.FindGameObjectWithTag("Player");
            if (p1 != null)
            {
                player1 = p1.GetComponent<PlayerHealth>();
            }
        }

        if (player2 == null)
        {
            GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
            if (p2 != null)
            {
                player2 = p2.GetComponent<PlayerHealth>();
            }
        }

        if (player1 != null && player2 != null) { found = true; }


    }

    void Update()
    {
        if (lobbyManager.done && !found)
        {
            if (isSinglePlayer)
            {
                player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
                found = true;
            }
            else
            {
                GetPlayers();
            }

        }
    }
}
