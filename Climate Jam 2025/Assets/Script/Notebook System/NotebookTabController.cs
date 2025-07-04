using UnityEngine;
using UnityEngine.UI;

public class NotebookTabController : MonoBehaviour
{
    public GameObject infoPage;
    public GameObject deductionPage;
    public GameObject reportPage;

    public Button tabInfo;
    public Button tabDeduction;
    public Button tabReport;

    public enum NotebookTab { Info, Deduction, Report }
    public NotebookTab currentTab = NotebookTab.Deduction;

    void Start()
    {
        tabInfo.onClick.AddListener(() => SwitchTab(NotebookTab.Info));
        tabDeduction.onClick.AddListener(() => SwitchTab(NotebookTab.Deduction));
        tabReport.onClick.AddListener(() => SwitchTab(NotebookTab.Report));
        SwitchTab(currentTab); // Default to deduction
    }

    void SwitchTab(NotebookTab tab)
    {
        infoPage.SetActive(tab == NotebookTab.Info);
        deductionPage.SetActive(tab == NotebookTab.Deduction);
        reportPage.SetActive(tab == NotebookTab.Report);
        currentTab = tab;
        // Optionally, call your RefreshBlockList here.
    }
}
