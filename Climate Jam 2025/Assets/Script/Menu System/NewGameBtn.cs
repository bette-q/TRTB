using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameBtn : MonoBehaviour
{
    [SerializeField] private string introKnotName = "0_0_0_Intro";
    [SerializeField] private string mainSceneName = "Prototype Test";
    [SerializeField] private GameObject menuPanel; // Drag your MenuPanel here in Inspector!
    [SerializeField] private GameObject terrain; // Drag your MenuPanel here in Inspector!
    [SerializeField] private GameObject water; // Drag your MenuPanel here in Inspector!

    void Start()
    {
        InkManager.Instance.OnDialogueEnd += OnIntroDialogueComplete;
    }

    public void OnClick()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
        if (terrain != null)
            terrain.SetActive(false);
        if (water != null)
            water.SetActive(false);

        InkManager.Instance.StartDialogue(introKnotName);
    }

    private void OnIntroDialogueComplete()
    {
        InkManager.Instance.OnDialogueEnd -= OnIntroDialogueComplete;
        SceneManager.LoadScene(mainSceneName);
    }
}
