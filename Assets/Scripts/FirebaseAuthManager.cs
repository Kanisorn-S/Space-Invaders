using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

public class FirebaseAuthManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;
    public GameObject errorPopup;
    public GameObject mainMenuScreen;
    public GameObject loginScreen;
    [SerializeField] public TextMeshProUGUI usernameText;
    public GameObject leaderboardManager;

    private const string API_KEY = "AIzaSyDsuvRfL2aNmHwg9OlS13-KADmysyt1wrY"; 

    public void OnSignUp()
    {
        string username = usernameInput.text;
        Debug.Log(username);
        string email = username + "@mygame.com";
        string password = passwordInput.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorMessage("Please enter both email/username and password.");
            return;
        }
        StartCoroutine(SignUp(email, password, username));
    }

    public void OnLogin()
    {
        string username = usernameInput.text;
        string email = username + "@mygame.com";
        string password = passwordInput.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorMessage("Please enter both email/username and password.");
            return;
        }
        StartCoroutine(Login(email, password, username));
    }

    IEnumerator SignUp(string email, string password, string username)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={API_KEY}";

        string json = JsonUtility.ToJson(new SignUpData(email, password));
        Debug.Log(json);
        var request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var result = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
            GameData.idToken = result.idToken;
            GameData.localId = result.localId;
            GameData.username = username;
            Debug.Log("Sign-up successful!");
            mainMenuScreen.SetActive(true);
            loginScreen.SetActive(false); // Hide the sign-up screen
            Debug.Log("User ID: " + GameData.localId);
            Debug.Log("Token: " + GameData.idToken);
            Debug.Log("Username: " + GameData.username);
            usernameText.text = GameData.username;
            leaderboardManager.SetActive(true); // Activate the leaderboard manager
        }
        else
        {
            Debug.LogError("Signup failed: " + request.downloadHandler.text);
            // Also helpful:
            Debug.LogError("Status Code: " + request.responseCode);
            var responseJson = JSON.Parse(request.downloadHandler.text);
            string errorMessage = responseJson["error"]["message"];
            ErrorMessage("Sign-up failed: " + errorMessage);
        }
    }

    IEnumerator Login(string email, string password, string username)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={API_KEY}";

        string json = JsonUtility.ToJson(new LoginData(email, password));
        Debug.Log(json);
        var request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var result = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
            GameData.idToken = result.idToken;
            GameData.localId = result.localId;
            GameData.username = username;
            Debug.Log("Login successful!");
            mainMenuScreen.SetActive(true);
            loginScreen.SetActive(false); // Hide the login screen
            Debug.Log("User ID: " + GameData.localId);
            Debug.Log("Token: " + GameData.idToken);
            Debug.Log("Username: " + GameData.username);
            usernameText.text = GameData.username;
            leaderboardManager.SetActive(true); // Activate the leaderboard manager
        }
        else
        {
            Debug.LogError("Login failed: " + request.downloadHandler.text);
            // Also helpful:
            Debug.LogError("Status Code: " + request.responseCode);
            var responseJson = JSON.Parse(request.downloadHandler.text);
            string errorMessage = responseJson["error"]["message"];
            ErrorMessage("Login failed: " + errorMessage);
        }
    }

    private void ErrorMessage(string message)
    {
        errorText.text = message;
        errorPopup.SetActive(true);
        StartCoroutine(HideFeedbackTextAfterDelay(3f)); // Hide after 3 seconds
    }

    private IEnumerator HideFeedbackTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        errorPopup.SetActive(false);
    }

    [System.Serializable]
    public class AuthResponse
    {
        public string idToken;
        public string localId;
    }

    public class SignUpData
    {
        public string email;
        public string password;
        public bool returnSecureToken = true;

        public SignUpData(string email, string password) {
        this.email = email;
        this.password = password;
        }
    }
    public class LoginData
    {
        public string email;
        public string password;
        public bool returnSecureToken = true;

        public LoginData(string email, string password) {
        this.email = email;
        this.password = password;
        }
    }

}
