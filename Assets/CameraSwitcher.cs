using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraSwitcher : MonoBehaviour
{
    // Array to hold your CCTV cameras (assign these in the Unity Inspector)
    public Camera[] cameras;

    // Array to hold your 4 buttons (assign these in the Unity Inspector)
    public Button[] switchButtons;

    void Start()
    {
        // Ensure we have exactly 4 cameras and buttons
        if (cameras.Length != 4 || switchButtons.Length != 4)
        {
            Debug.LogError("You must assign exactly 4 cameras and 4 buttons in the Inspector.");
            return;
        }

        // Set button text to camera GameObject names and set up listeners
        for (int i = 0; i < switchButtons.Length; i++)
        {
            if (cameras[i] != null && switchButtons[i] != null)
            {
                // Get the Text component of the button (or TextMeshProUGUI if using TMP)
                TextMeshProUGUI buttonText = switchButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = cameras[i].gameObject.name;
                }
                else
                {
                    Debug.LogWarning("Button " + (i + 1) + " does not have a Text component.");
                }

                // Set up button listener
                int index = i; // Capture the index for the lambda
                switchButtons[i].onClick.AddListener(() => SwitchToCamera(index));
            }
        }

        // Start with the first camera active
        SwitchToCamera(0);
    }

    void SwitchToCamera(int index)
    {
        // Disable all cameras
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = false;
        }

        // Enable the selected camera
        cameras[index].enabled = true;

        Debug.Log("Switched to camera: " + cameras[index].gameObject.name);
    }
}