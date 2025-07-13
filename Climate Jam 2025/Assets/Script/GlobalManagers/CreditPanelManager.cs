using UnityEngine;

public class CreditPanelManager : MonoBehaviour
{
    public GameObject creditPanel;

    public void ShowPanel()
    {
        creditPanel.SetActive(true);
    }

    public void HidePanel()
    {
        creditPanel.SetActive(false);
    }
}
