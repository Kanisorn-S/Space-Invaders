using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public static int startingWave;
    public static int highScore;
    public static string idToken;
    public static string localId;
    public static string username;
    public static GameObject loginScreen;
    public static GameObject mainMenuScreen;
    public static List<(string username, int score)> leaderboardData; 

    public void FetchLeaderboard()
    {
        LeaderBoardManager.Instance.FetchLeaderboard();
    }
}
