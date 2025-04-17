using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class FirebaseController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject loginScreen;
    public GameObject mainMenuScreen;
    public InputField emailInputField, passwordInputField;
    [SerializeField] public TextMeshProUGUI errorText;

    public void LoginUser()
    {
        if(string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            Debug.Log("Email or Password is empty");
            DisplayError("Email or Password is empty");
            return;
        }
    }

    public void SignUpUser()
    {
        if(string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            Debug.Log("Email or Password is empty");
            DisplayError("Email or Password is empty");
            return;
        }
    }

    public void DisplayError(string errorMessage)
    {
        errorText.text = errorMessage;
        errorText.gameObject.SetActive(true);
        StartCoroutine(HideErrorTextAfterDelay(3f)); // Hide after 3 seconds
    }

    private IEnumerator HideErrorTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        errorText.gameObject.SetActive(false);
    }
}
