using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TestInfo : MonoBehaviour
{
    
    public TextMeshPro gText;
    public TextMeshPro hText;
    public TextMeshPro fText;
    public SpriteRenderer sr;

    public void ChangeColor(string hc)
    { 
        Color c;
        ColorUtility.TryParseHtmlString(hc, out c);
        sr.color = c;
    }
}
