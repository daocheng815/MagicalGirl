using System;
using UnityEngine;
using Events;
using TMPro;

public class BuffFloatingWindow : MonoBehaviour
{
    public GameObject buffUIFloatingWindow;
    public TextMeshProUGUI buffUIFloatingWindowText;
    public Vector2 offset;
    private void OnEnable()
    {
        BuffEvents.BuffUIFloatingWindowOn += BuffUIFloatingWindowOn;
        BuffEvents.BuffUIFloatingWindowOff += BuffUIFloatingWindowOff;
    }

    private void OnDisable()
    {
        BuffEvents.BuffUIFloatingWindowOn -= BuffUIFloatingWindowOn;
        BuffEvents.BuffUIFloatingWindowOff -= BuffUIFloatingWindowOff;
    }

    private void BuffUIFloatingWindowOn(string info ,Vector2 lp)
    {
        buffUIFloatingWindow.SetActive(true);
        buffUIFloatingWindow.GetComponent<RectTransform>().localPosition = lp + offset;
        buffUIFloatingWindowText.text = info;
    }
    
    private void BuffUIFloatingWindowOff()
    {
        buffUIFloatingWindow.SetActive(false);
    }
}
