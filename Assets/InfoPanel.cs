using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class InfoPanel : MonoBehaviour
{
    [Header("Firestore Identity")]
    [SerializeField] string deviceId = "home1";
    [SerializeField] string fieldKey = "Lamp1";

    [Header("State")]
    bool isOn = false;
    bool initialized = false;

    [Header("Texts")]
    public TextMeshProUGUI onOffText;
    public TextMeshProUGUI panelText1;
    public TextMeshProUGUI panelText2;

    [Header("Images")]
    public Image shadowImage;
    public Image panelImage;

    [Header("Button Animator")]
    public Animator buttonAnimator;

    [Header("Settings")]
    public float transitionSpeed = 3f;

    Coroutine transitionRoutine;

    FirebaseFirestore db;
    ListenerRegistration listener;

    void Start()
    {
        // ❗ Set UI ke OFF dulu biar gak kosong
        ApplyState(true);

        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.Result != DependencyStatus.Available)
                {
                    Debug.LogError("Firebase not ready");
                    return;
                }

                db = FirebaseFirestore.DefaultInstance;
                StartListening();
            });
    }

    // ================= FIRESTORE =================

    void StartListening()
    {
        listener = db.Collection("devices")
                     .Document(deviceId)
                     .Listen(snapshot =>
                     {
                         if (!snapshot.Exists || !snapshot.ContainsField(fieldKey))
                             return;

                         bool remoteState = snapshot.GetValue<bool>(fieldKey);

                         // 🔥 INIT FIRST TIME
                         if (!initialized)
                         {
                             initialized = true;
                             isOn = remoteState;
                             ApplyState(true); // instant, no anim
                             return;
                         }

                         // 🔄 UPDATE FROM OTHER DEVICE
                         if (remoteState != isOn)
                         {
                             isOn = remoteState;
                             ApplyState(false); // animate
                         }
                     });
    }

    // ================= BUTTON =================

    public void Toggle()
    {
        // ⚡ OPTIMISTIC UI (langsung animasi)
        isOn = !isOn;
        ApplyState(false);

        var update = new Dictionary<string, object>
        {
            { fieldKey, isOn }
        };

        db.Collection("devices")
          .Document(deviceId)
          .UpdateAsync(update);
    }

    // ================= UI STATE =================

    void ApplyState(bool immediate)
    {
        if (buttonAnimator != null)
        {
            buttonAnimator.ResetTrigger("On");
            buttonAnimator.ResetTrigger("Off");
            buttonAnimator.SetTrigger(isOn ? "On" : "Off");
        }

        onOffText.text = isOn ? "ON" : "OFF";
        onOffText.color = isOn
            ? Color.white
            : RGBAFromHex("A2A2A2", 186);

        Color panelTextColor = isOn
            ? RGBAFromHex("A2A2A2", 186)
            : Color.black;

        panelText1.color = panelTextColor;
        panelText2.color = panelTextColor;

        float shadowAlpha = isOn ? 224f / 255f : 0f;
        Color panelTargetColor = isOn
            ? HexToColor("959595")
            : HexToColor("F3F3F3");

        if (immediate)
            SetImmediate(shadowAlpha, panelTargetColor);
        else
            StartSmooth(shadowAlpha, panelTargetColor);
    }

    // ================= TRANSITIONS =================

    void StartSmooth(float shadowAlpha, Color panelColor)
    {
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(
            SmoothTransition(shadowAlpha, panelColor)
        );
    }

    IEnumerator SmoothTransition(float shadowTargetAlpha, Color panelTargetColor)
    {
        Color shadowStart = shadowImage.color;
        Color panelStart = panelImage.color;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;

            Color s = shadowStart;
            s.a = Mathf.Lerp(shadowStart.a, shadowTargetAlpha, t);
            shadowImage.color = s;

            panelImage.color = Color.Lerp(panelStart, panelTargetColor, t);

            yield return null;
        }
    }

    void SetImmediate(float shadowAlpha, Color panelColor)
    {
        Color s = shadowImage.color;
        s.a = shadowAlpha;
        shadowImage.color = s;

        panelImage.color = panelColor;
    }

    // ================= UTIL =================

    Color HexToColor(string hex)
    {
        Color c;
        ColorUtility.TryParseHtmlString("#" + hex, out c);
        return c;
    }

    Color RGBAFromHex(string hex, int alpha255)
    {
        Color c = HexToColor(hex);
        c.a = alpha255 / 255f;
        return c;
    }

    void OnDestroy()
    {
        listener?.Stop();
    }


public void ForceRefreshVisual()
{
    // paksa ulang state visual
    //ApplyState(true);

    if (buttonAnimator != null)
    {
        buttonAnimator.Play(isOn ? "ON" : "OFF", 0, 1f);
    }
}


}
