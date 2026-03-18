using UnityEngine;

public class PanelParent : MonoBehaviour
{
    void OnEnable()
    {
        foreach (var panel in GetComponentsInChildren<InfoPanel>(true))
        {
            panel.ForceRefreshVisual();
        }
    }
}
