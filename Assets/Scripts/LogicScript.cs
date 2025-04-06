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
    public AudioClip bossSoundtrack;
    [SerializeField] public TextMeshProUGUI timerText;
    float elapsedTime;
    public GameObject bossPrefab;
    private bool isBossSpawned = false;
    private bool isGameOver = false;
    public int wave = 1;
    private int bossTimer = 0;
    private int previousSecond = 0;
    private bool renewed = false;

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
        if (isGameOver)
            return;
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        if (seconds != previousSecond || renewed)
        {
            renewed = false;
            previousSecond = seconds;
            bossTimer++;
            Debug.Log("Boss Timer: " + bossTimer);
        }
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        // Debug.Log(minutes + ":" + seconds);
        if (bossTimer == 30 && !isBossSpawned)
        {
            isBossSpawned = true;
            spawnBoss();
        }
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

    public void spawnBoss()
    {
        // Debug.Log(Time.deltaTime);
        src.Stop();
        src.volume = 0.2f;
        src.loop = true;
        src.clip = bossSoundtrack;
        src.Play();
        Instantiate(bossPrefab, new Vector3(0, -4.64f, 21.9f), Quaternion.identity);
    }

    public void BossDefeated()
    {
        Debug.Log("Boss defeated!");
        src.Stop();
        src.volume = 0.2f;
        src.loop = true;
        src.clip = gameSountrack;
        src.Play();
        isBossSpawned = false;
        renewed = true;
        bossTimer = 0;
    }
    public void gameOver()
    {
        src.Stop();
        src.volume = 0.2f;
        src.loop = false;
        src.clip = gameOverSoundtrack;
        src.Play();
        isGameOver = true;
        Destroy(player);
    }
}
