using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// handles notebook UI
public class NotebookUIManager : MonoBehaviour
{
    [HideInInspector]
    public static bool IsOpen { get; private set; }

    public GameObject notebookPanel;       
    public Transform ebPanel;              
    public GameObject evidenceBlockPrefab; 
    public TMP_Text descriptionBox;        
    public KeyCode toggleKey = KeyCode.N;

    private List<GameObject> spawnedCards = new List<GameObject>();

    void Start()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;
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
        RefreshUI();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RefreshUI()
    {
        foreach (var go in spawnedCards) Destroy(go);
        spawnedCards.Clear();

        var blocks = GameStateManager.Instance.GetCollectedBlocks();
        foreach (var eb in blocks)
        {
            var go = Instantiate(evidenceBlockPrefab, ebPanel);
            go.transform.Find("Title").GetComponent<TMP_Text>().text = eb.title;
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                descriptionBox.text = eb.text;
            });
            spawnedCards.Add(go);
        }

        // Clear description if nothing is selected
        if (blocks.Count == 0)
            descriptionBox.text = "";
    }
}
