using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePanelIndoorOOROutdoor : MonoBehaviour
{
    [Header("Panels (Always Active)")]
    public GameObject Indoor;
    public GameObject Outdoor;
    [Header("Active When Indoor")]
    public List<GameObject> IndoorActiveObjects;

    [Header("Active When Outdoor")]
    public List<GameObject> OutdoorActiveObjects;

    [Header("Doots")]
    public GameObject IndoorDoots;
    public GameObject OutdoorDoots;

    [Header("UI Images")]
    public Image IndoorImage;
    public Image OutdoorImage;

    [Header("Sprites")]
    public Sprite IndoorON;
    public Sprite IndoorOFF;
    public Sprite OutdoorON;
    public Sprite OutdoorOFF;

    public bool isIndoor = true;

    void Start()
    {
        // Pastikan panel selalu aktif
        Indoor.SetActive(true);
        Outdoor.SetActive(true);

        UpdateUI();
    }

    public void IndoorClick()
    {
        if (isIndoor) return;

        isIndoor = true;
        UpdateUI();
    }

    public void OutdoorClick()
    {
        if (!isIndoor) return;

        isIndoor = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        // ❗ Panel tidak disentuh sama sekali
        foreach (GameObject obj in IndoorActiveObjects)
        {
            if (obj != null)
                obj.SetActive(isIndoor);
        }

        // Outdoor group
        foreach (GameObject obj in OutdoorActiveObjects)
        {
            if (obj != null)
                obj.SetActive(!isIndoor);
        }
        // Doots
        IndoorDoots.SetActive(isIndoor);
        OutdoorDoots.SetActive(!isIndoor);

        // Sprite swap
        IndoorImage.sprite = isIndoor ? IndoorON : IndoorOFF;
        OutdoorImage.sprite = isIndoor ? OutdoorOFF : OutdoorON;
    }
    
}
