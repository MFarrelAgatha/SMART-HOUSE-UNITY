using UnityEngine;
using TMPro;
using Firebase.Auth;
using System;

public class ProfileInfoUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dateText;

    void Start()
    {
    }

    public void LoadProfileInfo(FirebaseUser user)
    {


        if (user == null)
        {
            Debug.LogWarning("❌ No user logged in");
            return;
        }

        // ================= NAME =================
        nameText.text = user.Email;

        // ================= DATE =================
        // Firebase timestamp = milliseconds UTC
        long creationTimeMs = (long)user.Metadata.CreationTimestamp;

        DateTime creationDate = DateTimeOffset
            .FromUnixTimeMilliseconds(creationTimeMs)
            .LocalDateTime;

        dateText.text = FormatDate(creationDate);

        Debug.Log("👤 Profile Loaded");
    }

    // ================= UTIL =================

    string FormatDate(DateTime date)
    {
        // Example: 07 January 2026
        return $"Joined: {date:dd MMMM yyyy}";
    }
}
