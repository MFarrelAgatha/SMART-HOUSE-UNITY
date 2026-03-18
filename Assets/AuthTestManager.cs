using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;

public class AuthTestManager : MonoBehaviour
{
    [Header("LOGIN UI")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;
    public TextMeshProUGUI loginStatusText;

    [Header("REGISTER UI")]
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;
    public TextMeshProUGUI registerStatusText;
    [Header("GAMEOBJECT")]
    public GameObject gameobject1;
    public GameObject gameobject2;
    public GameObject gameobject3;
    public GameObject gameobject4;

    public GameObject gameobject5;
    public ProfileInfoUI infoUI;


    FirebaseAuth auth;

    // ================= INIT =================

    void Start()
    {
        loginStatusText.text = "Initializing...";
        registerStatusText.text = "Initializing...";

        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.Result != DependencyStatus.Available)
                {
                    loginStatusText.text = "Firebase error";
                    registerStatusText.text = "Firebase error";
                    Debug.LogError("Firebase not ready");
                    return;
                }

                auth = FirebaseAuth.DefaultInstance;

                loginStatusText.text = "Ready";
                registerStatusText.text = "Ready";

                Debug.Log("🔥 Firebase Auth Ready");
            });
    }

    // ================= REGISTER =================

    public void Register()
    {
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;

        if (email == "" || password == "")
        {
            registerStatusText.text = "Email / Password empty";
            return;
        }

        registerStatusText.text = "Registering...";

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    registerStatusText.text = "Register failed";
                    Debug.LogError(task.Exception);
                    return;
                }

                FirebaseUser user = auth.CurrentUser;

                registerStatusText.text = "Registered";
                Debug.Log($"✅ REGISTER SUCCESS | UID: {user.UserId}");

                OnRegisterSuccess(user);
            });
    }

    // ================= LOGIN =================

    public void Login()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        if (email == "" || password == "")
        {
            loginStatusText.text = "Email / Password empty";
            return;
        }

        loginStatusText.text = "Logging in...";

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    loginStatusText.text = "Login failed";
                    Debug.LogError(task.Exception);
                    return;
                }

                FirebaseUser user = auth.CurrentUser;

                loginStatusText.text = "Logged in";
                Debug.Log($"✅ LOGIN SUCCESS | UID: {user.UserId}");

                OnLoginSuccess(user);
            });
    }

    // ================= CALLBACKS =================
    // 🔥 INI YANG LO MAU BUAT LANJUT LOGIC

    public void OnLoginSuccess(FirebaseUser user)
    {
        Debug.Log("🚀 OnLoginSuccess called");
        gameobject1.SetActive(false);
        gameobject3.SetActive(true);
        
        gameobject4.SetActive(true);
        gameobject5.SetActive(true);
        infoUI.LoadProfileInfo(user);

        

        // contoh:
        // SceneManager.LoadScene("MainScene");
        // InitFirestore(user.UserId);
        // EnableControlPanel();
    }

    public void OnRegisterSuccess(FirebaseUser user)
    {
        Debug.Log("🚀 OnRegisterSuccess called");
        infoUI.LoadProfileInfo(user);
        gameobject2.SetActive(false);
        gameobject3.SetActive(true);
        gameobject4.SetActive(true);
        gameobject5.SetActive(true);
        
        // contoh:
        // Auto login?
        // Create user document in Firestore
    }

    // ================= UTIL =================

    public bool IsLoggedIn()
    {
        return auth != null && auth.CurrentUser != null;
    }

    public string GetUserId()
    {
        if (!IsLoggedIn()) return "";
        return auth.CurrentUser.UserId;
    }
}
