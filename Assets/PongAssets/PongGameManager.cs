using TMPro;
using UnityEngine;

public class PongGameManager : MonoBehaviour
{
    [Header("Ball")]
    public GameObject ball;

    [Header("Player 1")]
    public GameObject player1Paddle;
    public GameObject player1Goal;

    [Header("Player 2")]
    public GameObject player2Paddle;
    public GameObject player2Goal;

    [Header("Score UI")]
    public GameObject Player1Text;
    public GameObject Player2Text;

    public int scoreLimit = 5;

    public int Player1Score;
    public int Player2Score;

    public void Player1Scored()
    {
        Player1Score++;
        Player1Text.GetComponent<TextMeshProUGUI>().text = Player1Score.ToString();
        if (Player1Score >= scoreLimit)
        {
            EndGame("Player 1");
        }
        else
        {
            ResetPosition();
        }
    }

    public void Player2Scored()
    {
        Player2Score++;
        Player2Text.GetComponent<TextMeshProUGUI>().text = Player2Score.ToString();
        if (Player2Score >= scoreLimit)
        {
            EndGame("Player 2");
        }
        else
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        ball.GetComponent<Ball>().Reset();
        player1Paddle.GetComponent<Paddle>().Reset();
        player2Paddle.GetComponent<Paddle>().Reset();
    }

    private void EndGame(string winner)
    {
        Debug.Log(winner + " wins the game!");
    }
}
