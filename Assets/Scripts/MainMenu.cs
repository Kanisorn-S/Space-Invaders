using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenLevel(int levelIndex)
    {
        LogicScript.isGameOver = false; // Reset game over state
        GameData.startingWave = levelIndex;
        SceneManager.LoadScene(1);
    }
}
