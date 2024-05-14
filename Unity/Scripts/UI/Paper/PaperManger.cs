using System;
using Events;
using TMPro;
using UnityEngine;

public class PaperManger : Singleton<PaperManger>
{
    public TextMeshProUGUI text;

    public GameObject paper;
    
    public Paper ap;

    private void Start()
    {
        //OpenPaper(ap);
    }
    public void OpenPaper(Paper p)
    {
        text.text = p.tileText + "\n" + p.textInfo;
        paper.SetActive(true);
    }
}
