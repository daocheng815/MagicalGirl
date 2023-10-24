using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Test : Singleton<UI_Test>
{
    TextMeshProUGUI TM;

    public string text;
    protected override void Awake()
    {
        base.Awake();
        TM = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        TM.text = text;
    }

}
