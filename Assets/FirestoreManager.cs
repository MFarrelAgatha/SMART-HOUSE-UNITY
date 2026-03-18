using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using System.Collections.Generic;

public class FirestoreLiveText : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;
    public TextMeshProUGUI liveText;

    FirebaseFirestore db;
    ListenerRegistration listener;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.Result != DependencyStatus.Available)
                {
                    Debug.LogError("Firebase dependency error: " + task.Result);
                    return;
                }

                Debug.Log("🔥 Firebase Ready (Firestore)");

                db = FirebaseFirestore.DefaultInstance;
                StartListening();
            });
    }

    public void SendToFirestore()
    {
        if (string.IsNullOrEmpty(inputField.text))
            return;

        var data = new Dictionary<string, object>
        {
            { "value", inputField.text }
        };

        db.Collection("live")
          .Document("display")
          .SetAsync(data);

        Debug.Log("⬆️ Sent: " + inputField.text);
    }

    void StartListening()
    {
        listener = db.Collection("live")
                     .Document("display")
                     .Listen(snapshot =>
                     {
                         if (snapshot.Exists && snapshot.ContainsField("value"))
                         {
                             string value = snapshot.GetValue<string>("value");
                             liveText.text = value;

                             Debug.Log("⬇️ Live update: " + value);
                         }
                     });
    }

    void OnDestroy()
    {
        listener?.Stop();
    }
}
