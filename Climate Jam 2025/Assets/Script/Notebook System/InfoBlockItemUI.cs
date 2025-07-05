using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InfoBlockItemUI : MonoBehaviour
{
    //public Image icon;
    public TMP_Text title;
    private EvidenceBlock block;
    private Action<EvidenceBlock> onClick;

    public void Setup(EvidenceBlock block, Action<EvidenceBlock> clickCallback)
    {
        this.block = block;
        //icon.sprite = block.info.icon;
        title.text = block.info.title;
        onClick = clickCallback;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke(block));

    }

    public void OnButtonClick()
    {
        onClick?.Invoke(block);
    }
}
