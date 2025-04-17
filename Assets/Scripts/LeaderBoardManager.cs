using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance { get; private set; }
    private string databaseURL = "https://d-space-invaders-default-rtdb.asia-southeast1.firebasedatabase.app/"; 
    private const string usersNode = "users";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(FetchLeaderboardData());
        Awake();
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

        string url = $"{databaseURL}/leaderboard.json?auth={GameData.idToken}&orderBy=\"score\"&limitToLast=3";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch leaderboard: " + request.error);
            yield break;
        }

        // Parse and sort the JSON
        JSONNode rawJson = JSON.Parse(request.downloadHandler.text);
        List<(string username, int score)> topScores = new List<(string, int)>();

        foreach (KeyValuePair<string, JSONNode> entry in rawJson)
        {
            string name = entry.Value["username"];
            int score = entry.Value["score"];
            topScores.Add((name, score));
        }

        // Sort in descending order
        topScores.Sort((a, b) => b.score.CompareTo(a.score));

        Debug.Log("🏆 Top 3 Leaderboard:");
        for (int i = 0; i < Mathf.Min(3, topScores.Count); i++)
        {
            Debug.Log($"{i + 1}. {topScores[i].username} - {topScores[i].score}");
        }

        // Fetch current user's score
        string userUrl = $"{databaseURL}/leaderboard/{GameData.localId}/score.json?auth={GameData.idToken}";
        UnityWebRequest userRequest = UnityWebRequest.Get(userUrl);
        yield return userRequest.SendWebRequest();

        if (userRequest.result == UnityWebRequest.Result.Success)
        {
            int userScore = int.Parse(userRequest.downloadHandler.text);
            GameData.highScore = userScore; // Update GameData with the user's score
            Debug.Log($"{GameData.username}'s High Score: {userScore}");
        }
        else
        {
            Debug.LogWarning("Could not fetch current user's score: " + userRequest.error);
        }
    }
}
