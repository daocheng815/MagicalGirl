using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class coolingItmeFadeUI : MonoBehaviour
{
    public GameObject cooling;
    public Image coolingColor;
    public Image coolingBar;
    public int slotID;
    public bool isFade;
    private void Awake()
    {
        cooling.SetActive(false);
        coolingColor.color = new Color(0, 0, 0, 0);
        coolingBar.fillAmount = 1f;
        coolingBar.color = new Color(0.3f, 0.3f, 0.3f, 0);
    }
    
    public void Fade(float delaytime)
    {
        if(!isFade)
        {
            isFade = true;
            Debug.Log("動畫開始" + delaytime);
            cooling.SetActive(true);
            coolingColor.color = new Color(0, 0, 0, 0.12f);
            coolingBar.color = new Color(0.3f, 0.3f, 0.3f, 0);
            coolingBar.fillAmount = 1f;
            DOTween.To(() => coolingBar.color, x => coolingBar.color = x, new Color(0.1f, 0.1f, 0.1f, 0.7f), 0.033f)
                .SetEase(Ease.InQuad);
            DOTween.To(() => coolingBar.fillAmount, x => coolingBar.fillAmount = x, 0.11f, delaytime).OnComplete(() =>
            {
                //coolingBar.DOFade(0, delaytime/10).SetEase(Ease.InQuad);
                coolingColor.color = new Color(0, 0, 0, 0);
                cooling.SetActive(false);
                Debug.Log("動畫結束");
                isFade = false;
            });
        }
    }
}
//標記
#if UNITY_EDITOR
[CustomEditor(typeof(coolingItmeFadeUI))]
public class coolingItmeFadeUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        coolingItmeFadeUI ci = (coolingItmeFadeUI)target;
        if (GUILayout.Button("Fade"))
        {
            ci.Fade(10f);
        }
    }
}    
#endif