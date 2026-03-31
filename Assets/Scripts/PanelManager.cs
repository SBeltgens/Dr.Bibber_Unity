using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
