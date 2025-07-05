using UnityEngine;
using UnityEngine.UI;

public class NotebookUIController : MonoBehaviour
{
    [HideInInspector]
    public static bool IsOpen { get; private set; }

    [Header("Notebook")]
    public GameObject notebookPanel;
    public KeyCode toggleKey = KeyCode.N;

    [Header("Tab Panels")]
    public GameObject infoPage;
    public GameObject deductionPage;
    public GameObject reportPage;

    [Header("Tab Buttons")]
    public Button tabInfo;
    public Button tabDeduction;
    public Button tabReport;

    [Header("Page Managers")]
    public InfoReportUIManager infoPageManager;
    public DeductionUIManager deductionPageManager;
    public InfoReportUIManager reportPageManager;

    public enum NotebookTab { Info, Deduction, Report }
    public NotebookTab currentTab = NotebookTab.Info;

    void Start()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;

        tabInfo.onClick.AddListener(() => SwitchTab(NotebookTab.Info));
        tabDeduction.onClick.AddListener(() => SwitchTab(NotebookTab.Deduction));
        tabReport.onClick.AddListener(() => SwitchTab(NotebookTab.Report));
        SwitchTab(currentTab); // Default to deduction
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (!notebookPanel.activeSelf)
                Open();
            else
                Close();
        }
    }

    public void Open()
    {
        notebookPanel.SetActive(true);
        IsOpen = true;
        SwitchTab(currentTab); // Refresh current tab/page on open
    }

    public void Close()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;
    }

    void SwitchTab(NotebookTab tab)
    {
        infoPage.SetActive(tab == NotebookTab.Info);
        deductionPage.SetActive(tab == NotebookTab.Deduction);
        reportPage.SetActive(tab == NotebookTab.Report);
        currentTab = tab;

        if (tab == NotebookTab.Info)
            infoPageManager.SetBlocks(GameStateManager.Instance.GetAvailableBlocks());
        if (tab == NotebookTab.Deduction)
            deductionPageManager.RefreshPage(); 
        else if (tab == NotebookTab.Report && reportPageManager != null)
            reportPageManager.SetBlocks(GameStateManager.Instance.GetAvailableBlocks());
    }
}
