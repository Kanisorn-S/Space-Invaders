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
    public static bool isGameOver = false;
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
    public BaseAlienSpawner[] alienSpawners;
    private bool readyToLevelUp = false;
    [SerializeField] public TextMeshProUGUI waveText;
    [SerializeField] public TextMeshProUGUI waveNumberText;
    private int displayedWave = 1;

    private (string, int)[] powerUps = new (string, int)[]
    {
        ("Shields", 10),
        ("Laser", 10),
        ("Lightning", 10),
        ("Star", 10),
        ("Time", 10)
    };

    public int powerUpIndex = -1;
    public LuckyBoxSpawner luckyBoxSpawner;

    void Start()
    {
        waveNumberText.color = new Color(22f / 255f, 253f / 255f, 0f / 255f, 1f);
        StartCoroutine(DisplayWaveText());
        wave = GameData.startingWave;
        bossTimer = 0;
        LogicScript.isGameOver = false;
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
        if (LogicScript.isGameOver)
            return;
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        if (wave % 5 == 4)
        {
            Debug.Log("Boss Wave");
            Debug.Log("Boss Timer: " + bossTimer);
        if (seconds != previousSecond || renewed)
        {
            renewed = false;
            previousSecond = seconds;
            bossTimer++;
        }
        if (bossTimer == 15 && !isBossSpawned)
        {
            isBossSpawned = true;
            spawnBoss();
        }

        }
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        // Debug.Log(minutes + ":" + seconds);

        if (wave % 5 != 4)
        {
            foreach (BaseAlienSpawner spawner in alienSpawners)
            {
                if (!spawner.cleared)
                {
                    return;
                }
            }
            nextWave();
        } else {
            foreach (BaseAlienSpawner spawner in alienSpawners)
            {
                if (!spawner.cleared)
                {
                    return;
                }
            }
            if (readyToLevelUp)
            {
                nextWave();
            }
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
        if (wave % 5 != 4)
        {
            return;
        }
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
        readyToLevelUp = true;
        // wave++;
        // foreach (BaseAlienSpawner spawner in alienSpawners)
        // {
        //     spawner.goToWave(wave);
        // }
    }

    public void nextWave()
    {
        wave++;
        displayedWave++;
        Debug.Log("Wave: " + wave);
        foreach (BaseAlienSpawner spawner in alienSpawners)
        {
            spawner.goToWave(wave);
        }
        luckyBoxSpawner.goToWave(wave);
        waveNumberText.text = (displayedWave).ToString();
        if (wave % 5 == 4)
        {
            waveNumberText.color = Color.red;
        } else {
            waveNumberText.color = new Color(22f / 255f, 253f / 255f, 0f / 255f, 1f);
        }
        StartCoroutine(DisplayWaveText());
        bossTimer = 0;
        readyToLevelUp = false;
    }

    private IEnumerator DisplayWaveText()
    {
        waveText.gameObject.SetActive(true);
        waveNumberText.gameObject.SetActive(true);

        CanvasGroup waveCanvasGroup = waveText.GetComponent<CanvasGroup>();
        CanvasGroup waveTextCanvasGroup = waveNumberText.GetComponent<CanvasGroup>();

        // Fade in
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            waveCanvasGroup.alpha = t;
            waveTextCanvasGroup.alpha = t;
            yield return null;
        }

        waveCanvasGroup.alpha = 1;
        waveTextCanvasGroup.alpha = 1;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

        // Fade out
        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            waveCanvasGroup.alpha = t;
            waveTextCanvasGroup.alpha = t;
            yield return null;
        }

        waveCanvasGroup.alpha = 0;
        waveTextCanvasGroup.alpha = 0;

        waveText.gameObject.SetActive(false);
        waveNumberText.gameObject.SetActive(false);
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
                break;
            case "Lightning":
                animator.SetTrigger("Lightning");
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
        if (powerUpIndex == 2)
        {
            player.moveSpeed = player.moveSpeed * 2;
            player.tiltSpeed = player.tiltSpeed * 2;
        } else if (powerUpIndex == 1)
            laser.SetActive(true);
        {
        }
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
        Debug.Log("Game Over!");
        Debug.Log("PowerUp Index: " + powerUpIndex);
        if (powerUpIndex == 0)
        {
            return;
        }
        src.Stop();
        src.volume = 0.2f;
        src.loop = false;
        src.clip = gameOverSoundtrack;
        src.Play();
        LogicScript.isGameOver = true;
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
