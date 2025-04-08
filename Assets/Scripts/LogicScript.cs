using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public int playerHealth;
    // public Text scoreText;
    [SerializeField] public TextMeshProUGUI scoreText; 
    public PlayerController player;
    // public Text playerHealthText;
    public AudioSource src;
    public AudioSource ontopSrc;
    public AudioSource[] audioSources;
    private int currentAudioSourceIndex = 0;
    public AudioClip gameSountrack;
    public AudioClip gameOverSoundtrack;
    public AudioClip bossSoundtrack;
    public AudioClip chanceBoxSound;
    public AudioClip alienDeathSound;
    [SerializeField] public TextMeshProUGUI timerText;
    float elapsedTime;
    public GameObject bossPrefab;
    private bool isBossSpawned = false;
    public bool isGameOver = false;
    public int wave = 1;
    private int bossTimer = 0;
    private int previousSecond = 0;
    private bool renewed = false;
    public Animator animator;
    public GameObject DeathScreen;
    [SerializeField] public TextMeshProUGUI DeathScoreText;
    [SerializeField] public TextMeshProUGUI DeathHighScoreText;
    public GameObject powerUpBanner;
    public GameObject laser;

    private (string, int)[] powerUps = new (string, int)[]
    {
        ("Shields", 10),
        ("Laser", 10),
        ("Lightning", 10),
        ("Star", 10),
        ("Time", 10)
    };

    public int powerUpIndex;

    void Start()
    {
        wave = GameData.startingWave;
        Debug.Log("Wave: " + wave);
        src.clip = gameSountrack;
        src.loop = true;
        src.volume = 0.2f;
        src.Play();
        playerHealth = player.GetComponent<PlayerController>().health;
        if (audioSources.Length == 0)
        {
            audioSources = new AudioSource[10];
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }
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
        if (powerUpIndex == 3)
        {
            score = score * 2;
        }
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

    public void bossDefeated()
    {
        src.Stop();
        src.volume = 0.2f;
        src.loop = true;
        src.clip = gameSountrack;
        src.Play();
        isBossSpawned = false;
        renewed = true;
        wave++;
        Debug.Log("Wave: " + wave);
        bossTimer = 0;
    }
    
    public void chanceBox()
    {
        powerUpIndex = Random.Range(0, powerUps.Length);
        powerUpBanner.SetActive(true);
        switch (powerUps[powerUpIndex].Item1)
        {
            case "Shields":
                animator.SetTrigger("Shields");
                break;
            case "Laser":
                animator.SetTrigger("Laser");
                laser.SetActive(true);
                break;
            case "Lightning":
                animator.SetTrigger("Lightning");
                player.moveSpeed = player.moveSpeed * 2;
                player.tiltSpeed = player.tiltSpeed * 2;
                break;
            case "Star":
                animator.SetTrigger("Star");
                break;
            case "Time":
                animator.SetTrigger("Time");
                break;
        }
        ontopSrc.Stop();
        ontopSrc.volume = 0.2f;
        ontopSrc.loop = false;
        ontopSrc.clip = chanceBoxSound;
        ontopSrc.time = 1f;
        ontopSrc.Play();
        float duration = powerUps[powerUpIndex].Item2;
        StartCoroutine(DeactivatePowerUpBanner(duration));
    }

    private IEnumerator DeactivatePowerUpBanner(float duration)
    {
        yield return new WaitForSeconds(duration);
        laser.SetActive(false);
        if (powerUpIndex == 2)
        {
            player.moveSpeed = player.moveSpeed / 2;
            player.tiltSpeed = player.tiltSpeed / 2;
        }
        powerUpBanner.SetActive(false);
        powerUpIndex = -1;
    }

    public void alienDeath()
    {
        AudioSource src = audioSources[currentAudioSourceIndex];
        src.clip = alienDeathSound;
        src.volume = 0.2f;
        src.Play();
        currentAudioSourceIndex = (currentAudioSourceIndex + 1) % audioSources.Length;
    }
    public void gameOver()
    {
        if (powerUpIndex == 0)
        {
            return;
        }
        src.Stop();
        src.volume = 0.2f;
        src.loop = false;
        src.clip = gameOverSoundtrack;
        src.Play();
        isGameOver = true;
        Destroy(player);
        DeathScoreText.text = playerScore.ToString();
        if (playerScore > GameData.highScore)
        {
            GameData.highScore = playerScore;
        }
        DeathHighScoreText.text = GameData.highScore.ToString();
        DeathScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
