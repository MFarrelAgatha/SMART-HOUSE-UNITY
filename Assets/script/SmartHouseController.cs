using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmartHouseController : MonoBehaviour
{
    // Assign these in the Unity Inspector
    public Light[] lights = new Light[3]; // Your 3 light components
    public Animator[] doors = new Animator[3]; // Your 3 door animators (assume they have a bool parameter named "IsOpen" for animation control)

    // UI elements for lights
    [Header("LIGHT ELEMENT")]
    public TMP_Dropdown lightDropdown; // Dropdown to select which light to control
    public Button lightOnButton; // Button to turn light on
    public Button lightOffButton; // Button to turn light off
    public Image lightStatusPanel; // Image panel for status color (red = off, green = on)
    public TextMeshProUGUI lightStatusText; // Text to show "On" or "Off"

    // UI elements for doors (I improvised by adding a similar setup for doors to make it consistent and universal)
    [Header("DOOR ELEMENT")]
    public TMP_Dropdown doorDropdown; // Dropdown to select which door to control
    public Button doorOpenButton; // Button to open door
    public Button doorCloseButton; // Button to close door
    public Image doorStatusPanel; // Image panel for door status color (red = closed, green = open)
    public TextMeshProUGUI doorStatusText; // Text to show "Open" or "Closed"

    void Start()
    {
        // Populate light dropdown with options
        lightDropdown.options.Clear();
        for (int i = 0; i < lights.Length; i++)
        {
            lightDropdown.options.Add(new TMP_Dropdown.OptionData(lights[i].gameObject.name));
        }
        lightDropdown.value = 0;
        lightDropdown.RefreshShownValue();
        lightDropdown.onValueChanged.AddListener(OnLightSelected);

        // Assign button listeners for lights
        lightOnButton.onClick.AddListener(TurnOnLight);
        lightOffButton.onClick.AddListener(TurnOffLight);

        // Populate door dropdown with options
        doorDropdown.options.Clear();
        for (int i = 0; i < doors.Length; i++)
        {
            doorDropdown.options.Add(new TMP_Dropdown.OptionData(doors[i].gameObject.name));
        }
        doorDropdown.value = 0;
        doorDropdown.RefreshShownValue();
        doorDropdown.onValueChanged.AddListener(OnDoorSelected);

        // Assign button listeners for doors
        doorOpenButton.onClick.AddListener(OpenDoor);
        doorCloseButton.onClick.AddListener(CloseDoor);

        // Initial status updates
        OnLightSelected(0);
        OnDoorSelected(0);
    }

    // Called when light dropdown selection changes
    void OnLightSelected(int index)
    {
        UpdateLightStatus(index);
    }

    // Turn on the selected light
    void TurnOnLight()
    {
        int index = lightDropdown.value;
        if (index >= 0 && index < lights.Length && lights[index] != null)
        {
            lights[index].enabled = true;
            UpdateLightStatus(index);
        }
    }

    // Turn off the selected light
    void TurnOffLight()
    {
        int index = lightDropdown.value;
        if (index >= 0 && index < lights.Length && lights[index] != null)
        {
            lights[index].enabled = false;
            UpdateLightStatus(index);
        }
    }

    // Update the light status panel and text
    void UpdateLightStatus(int index)
    {
        if (index >= 0 && index < lights.Length && lights[index] != null)
        {
            bool isOn = lights[index].enabled;
            lightStatusPanel.color = isOn ? Color.green : Color.red;
            lightStatusText.text = isOn ? "On" : "Off";
        }
    }

    // Called when door dropdown selection changes
    void OnDoorSelected(int index)
    {
        UpdateDoorStatus(index);
    }

    // Open the selected door (plays animation via animator)
    void OpenDoor()
    {
        int index = doorDropdown.value;
        if (index >= 0 && index < doors.Length && doors[index] != null)
        {
            doors[index].SetBool("IsOpen", true); // Assumes your door animator has a bool parameter "IsOpen" to trigger open animation
            UpdateDoorStatus(index);
        }
    }

    // Close the selected door
    void CloseDoor()
    {
        int index = doorDropdown.value;
        if (index >= 0 && index < doors.Length && doors[index] != null)
        {
            doors[index].SetBool("IsOpen", false); // Triggers close animation
            UpdateDoorStatus(index);
        }
    }

    // Update the door status panel and text
    void UpdateDoorStatus(int index)
    {
        if (index >= 0 && index < doors.Length && doors[index] != null)
        {
            bool isOpen = doors[index].GetBool("IsOpen");
            doorStatusPanel.color = isOpen ? Color.green : Color.red;
            doorStatusText.text = isOpen ? "Open" : "Closed";
        }
    }
}