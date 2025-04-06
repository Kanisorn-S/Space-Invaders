using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public int playerHealth;
    // public Text scoreText;
    [SerializeField] public TextMeshProUGUI scoreText; 
    public GameObject player;
    // public Text playerHealthText;
    public AudioSource src;
    public AudioClip gameSountrack;
    public AudioClip gameOverSoundtrack;
    [SerializeField] public TextMeshProUGUI timerText;
    float elapsedTime;

    void Start()
    {
        src.clip = gameSountrack;
        src.loop = true;
        src.volume = 0.2f;
        src.Play();
        playerHealth = player.GetComponent<PlayerController>().health;
        // playerHealthText.text = playerHealth.ToString();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
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

        // playerHealthText.text = playerHealth.ToString();
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
