using UnityEngine;

public class SafeArea : MonoBehaviour
{
    void Awake()
    {
        Rect safe = Screen.safeArea;
        RectTransform rt = GetComponent<RectTransform>();

        Vector2 min = safe.position;
        Vector2 max = safe.position + safe.size;

        min.x /= Screen.width;
        min.y /= Screen.height;
        max.x /= Screen.width;
        max.y /= Screen.height;

        rt.anchorMin = min;
        rt.anchorMax = max;
    }
}
