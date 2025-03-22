using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public Text scoreText;

    public void addScore(int score)
    {
        playerScore += score;
        scoreText.text = playerScore.ToString();
    }
}
