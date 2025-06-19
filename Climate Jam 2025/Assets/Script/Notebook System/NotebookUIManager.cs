using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NotebookUIManager : MonoBehaviour
{
    public GameObject notebookPanel;       
    public Transform ebPanel;              
    public GameObject evidenceBlockPrefab; 
    public TMP_Text descriptionBox;        
    public KeyCode toggleKey = KeyCode.N;

    private List<GameObject> spawnedCards = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            bool open = !notebookPanel.activeSelf;
            notebookPanel.SetActive(open);
            if (open)
                RefreshUI(); // UI updates itself!
        }
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
