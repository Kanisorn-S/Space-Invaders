using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
