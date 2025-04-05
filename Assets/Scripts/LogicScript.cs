using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public int playerHealth;
    public Text scoreText;
    public GameObject player;
    public Text playerHealthText;

    void Start()
    {
        playerHealth = player.GetComponent<PlayerController>().health;
        playerHealthText.text = playerHealth.ToString();
    }
    public void addScore(int score)
    {
        playerScore += score;
        scoreText.text = playerScore.ToString();
    }

    public void updatePlayerHealth(bool add, int health)
    {
        if (add)
            playerHealth += health;
        else
            playerHealth -= health;

        playerHealthText.text = playerHealth.ToString();
    }

    public void gameOver()
    {
        Destroy(player);
    }
}
