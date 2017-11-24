using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIBehaviour : MonoBehaviour
{
    public Text timer;
    public Text player1Score;
    public Text player2Score;
    public GameObject gameOverPanel;
    public Text gameOverText;

    public void UpdateUITimer(int timeLeft)
    {
        timer.text = "Time left: " + timeLeft + " s";
    }

    public void UpdateUIPlayer1Score(string playerName, int score)
    {
        player1Score.text = playerName + ": " + score;
    }

    public void UpdateUIPlayer2Score(string playerName, int score)
    {
        player2Score.text = playerName + ": " + score;
    }

    public void UpdateUIGameOver(string winnerName)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = winnerName + " has won!";
    }

    public void UpdateUIGameOverDraw()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "It's a draw!";
    }

    public void Rematch()
    {
        GetComponent<GameRoundManagerController>().InitGameElementsAndUI();
    }

    public void BackToMainMenu(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }
}