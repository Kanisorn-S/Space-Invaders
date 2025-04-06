using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenLevel(int levelIndex)
    {
        GameData.startingWave = levelIndex;
        SceneManager.LoadScene(1);
    }
}
