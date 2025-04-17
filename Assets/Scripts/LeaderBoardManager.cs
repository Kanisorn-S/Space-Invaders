using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance { get; private set; }
    private string databaseURL = "https://d-space-invaders-default-rtdb.asia-southeast1.firebasedatabase.app/"; 
    private const string usersNode = "users";
    public List<TextMeshProUGUI> leaderboardTexts; // UI elements to display leaderboard data
    [SerializeField] public TextMeshProUGUI currentHighText; // UI element to display current high score

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
    SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        StartCoroutine(FetchLeaderboardData());
        Awake();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    // Reassign references to the TMP Texts
    leaderboardTexts[0] = GameObject.Find("FirstText")?.GetComponent<TextMeshProUGUI>();
    leaderboardTexts[1] = GameObject.Find("SecondText")?.GetComponent<TextMeshProUGUI>();
    leaderboardTexts[2] = GameObject.Find("ThirdText")?.GetComponent<TextMeshProUGUI>();

    }
    public void FetchLeaderboard()
    {
        leaderboardTexts[0] = GameObject.FindGameObjectWithTag("FirstText")?.GetComponent<TextMeshProUGUI>();
        leaderboardTexts[1] = GameObject.FindGameObjectWithTag("SecondText")?.GetComponent<TextMeshProUGUI>();
        leaderboardTexts[2] = GameObject.FindGameObjectWithTag("ThirdText")?.GetComponent<TextMeshProUGUI>();
        currentHighText = GameObject.FindGameObjectWithTag("CurrentHigh")?.GetComponent<TextMeshProUGUI>();
        Debug.Log("Fetching leaderboard data...");
        Debug.Log(leaderboardTexts[0]);
        StartCoroutine(FetchLeaderboardData());
    }

    public void SubmitScore(int newScore)
    {
        StartCoroutine(UpdateHighScoreRoutine(newScore));
    }

    private IEnumerator UpdateHighScoreRoutine(int newScore)
    {
        string userUrl = $"{databaseURL}{usersNode}/{GameData.localId}.json?auth={GameData.idToken}";

        // Get current high score
        UnityWebRequest getRequest = UnityWebRequest.Get(userUrl);
        yield return getRequest.SendWebRequest();

        if (getRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get score error: " + getRequest.error);
            yield break;
        }

        int currentHighScore = 0;
        var json = JSON.Parse(getRequest.downloadHandler.text);
        if (json != null && json["highscore"] != null)
        {
            currentHighScore = json["highscore"].AsInt;
        }

        if (newScore > currentHighScore)
        {
            // Update highscore
            string jsonBody = $"{{\"username\":\"{GameData.username}\",\"highscore\":{newScore}}}";

            UnityWebRequest putRequest = UnityWebRequest.Put(userUrl, jsonBody);
            putRequest.method = UnityWebRequest.kHttpVerbPUT;
            putRequest.SetRequestHeader("Content-Type", "application/json");

            yield return putRequest.SendWebRequest();

            if (putRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("High score updated!");
            }
            else
            {
                Debug.LogError("Failed to update score: " + putRequest.error);
            }
        }
        else
        {
            Debug.Log("New score is not higher than current high score. Not updating.");
        }
    }

    IEnumerator FetchLeaderboardData()
    {
        if (string.IsNullOrEmpty(GameData.idToken) || string.IsNullOrEmpty(GameData.localId))
        {
            Debug.LogError("Missing Firebase user credentials (idToken or localId)");
            yield break;
        }

        string url = $"{databaseURL}/users.json?auth={GameData.idToken}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch leaderboard: " + request.error);
            Debug.LogError("Fetch failed: " + request.downloadHandler.text);
            // Also helpful:
            Debug.LogError("Status Code: " + request.responseCode);
            yield break;
        }

        // Parse and sort the JSON
        JSONNode rawJson = JSON.Parse(request.downloadHandler.text);
        List<(string username, int highscore)> topScores = new List<(string, int)>();

        foreach (KeyValuePair<string, JSONNode> entry in rawJson)
        {
            string name = entry.Value["username"];
            int highscore = entry.Value["highscore"];
            topScores.Add((name, highscore));
        }

        // Sort in descending order
        topScores.Sort((a, b) => b.highscore.CompareTo(a.highscore));
        GameData.leaderboardData = topScores; // Update GameData with the leaderboard data

        Debug.Log("🏆 Top 3 Leaderboard:");
        for (int i = 0; i < Mathf.Min(3, topScores.Count); i++)
        {
            Debug.Log($"{i + 1}. {topScores[i].username} - {topScores[i].highscore}");
            leaderboardTexts[i].text = $"{topScores[i].username}: {topScores[i].highscore} PTS"; // Update UI elements
        }

        // Fetch current user's score
        string userUrl = $"{databaseURL}/users/{GameData.localId}/highscore.json?auth={GameData.idToken}";
        UnityWebRequest userRequest = UnityWebRequest.Get(userUrl);
        yield return userRequest.SendWebRequest();

        if (userRequest.result == UnityWebRequest.Result.Success)
        {
            int userScore = int.Parse(userRequest.downloadHandler.text);
            GameData.highScore = userScore; // Update GameData with the user's score
            currentHighText.text = userScore.ToString(); // Update UI element
            Debug.Log($"{GameData.username}'s High Score: {userScore}");
        }
        else
        {
            Debug.LogWarning("Could not fetch current user's score: " + userRequest.error);
            Debug.LogError("Login failed: " + userRequest.downloadHandler.text);
            // Also helpful:
            Debug.LogError("Status Code: " + userRequest.responseCode);
        }
    }
}
