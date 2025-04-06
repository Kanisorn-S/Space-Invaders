using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public int playerHealth;
    public Text scoreText;
    public GameObject player;
    public Text playerHealthText;
    public AudioSource src;
    public AudioClip gameSountrack;
    public AudioClip gameOverSoundtrack;

    void Start()
    {
        src.clip = gameSountrack;
        src.loop = true;
        src.volume = 0.2f;
        src.Play();
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
        src.Stop();
        src.volume = 0.2f;
        src.loop = false;
        src.clip = gameOverSoundtrack;
        src.Play();
        Destroy(player);
    }
}
