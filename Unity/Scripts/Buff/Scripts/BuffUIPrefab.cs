using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BuffUIPrefab : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image buffImage;
    public Image backGround;
    public Image timeBar;
    public TextMeshProUGUI buffOverlayNum;
    public GameObject buffOverlayNumGo;
    public int buffOverlayNumVar;
    public Buff myBuff;
    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        Debug.Log("進入");
        string info = myBuff.buffInfo[0];
        if (myBuff.buffOverlay )
        {
            if (myBuff.buffInfo.Length >= buffOverlayNumVar)
            {
                info = myBuff.buffInfo[buffOverlayNumVar - 1];
            }
            else
            {
                info = myBuff.buffInfo[^1];
            }
        }
        BuffEvents.BuffUIFloatingWindowOn.Invoke(info,rt.localPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("出去");
        BuffEvents.BuffUIFloatingWindowOff.Invoke();

    }
    /// <summary>
    /// 開始Buff動畫
    /// </summary>
    /// <param name="time">Fade時間</param>
    /// <param name="buff">如名</param>
    /// <param name="be">buffEffect腳本</param>
    public void StartPlayerFadeAnime(float time,Buff buff,buffEffect be)
    {
        myBuff = buff;
        timeBar.fillAmount = 0f;
        timeBar.color = new Color(timeBar.color.r,timeBar.color.g,timeBar.color.b,0);//Alpha To 0.7
        backGround.color = new Color(backGround.color.r,backGround.color.g,backGround.color.b,0);//Alpha To 1
        buffImage.color = new Color(buffImage.color.r,buffImage.color.g,buffImage.color.b,0);//Alpha To 1

        backGround.sprite = buff.buffBackGround;
        buffImage.sprite = buff.buffImage;
        
        if(buff.buffOverlay)
        {
            //更新疊層
            buffOverlayNumVar = be.buffOverlayNum[buff.buffID];
            buffOverlayNum.text = buffOverlayNumVar.ToString();
            buffOverlayNum.color = new Color(buffOverlayNum.color.r, buffOverlayNum.color.g, buffOverlayNum.color.b, 0);
            DOTween.To(() =>  buffOverlayNum.color, x =>  buffOverlayNum.color = x, new Color(buffOverlayNum.color.r, buffOverlayNum.color.g, buffOverlayNum.color.b, 1), 0.22f);
        }
        else
        {
            buffOverlayNumGo.SetActive(false);
        }
        
        
        DOTween.To(() => timeBar.color, x => timeBar.color = x, new Color(timeBar.color.r,timeBar.color.g,timeBar.color.b,0.7f), 0.22f);
        DOTween.To(() => backGround.color, x => backGround.color = x, new Color(backGround.color.r,backGround.color.g,backGround.color.b,1), 0.22f);
        DOTween.To(() => buffImage.color, x => buffImage.color = x, new Color(buffImage.color.r,buffImage.color.g,buffImage.color.b,1), 0.22f);
        DOTween.To(() => timeBar.fillAmount, x => timeBar.fillAmount = x, 0.965f, time).SetEase(Ease.Linear).OnComplete((() =>
        {
            if(buff.buffOverlay)
                DOTween.To(() =>  buffOverlayNum.color, x =>  buffOverlayNum.color = x, new Color(buffOverlayNum.color.r, buffOverlayNum.color.g, buffOverlayNum.color.b, 0), 0.33f);
            DOTween.To(() => timeBar.color, x => timeBar.color = x, new Color(timeBar.color.r,timeBar.color.g,timeBar.color.b,0), 0.33f);
            DOTween.To(() => backGround.color, x => backGround.color = x, new Color(backGround.color.r,backGround.color.g,backGround.color.b,0), 0.33f);
            DOTween.To(() => buffImage.color, x => buffImage.color = x, new Color(buffImage.color.r,buffImage.color.g,buffImage.color.b,0), 0.33f).OnComplete(() =>
                {
                    Destroy(gameObject);
                    BuffEvents.BuffUIFloatingWindowOff.Invoke();
                });
        }));
    }
    
}
#if UNITY_EDITOR
[CustomEditor(typeof(BuffUIPrefab))]
public class BuffUIPrefabEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BuffUIPrefab bi = (BuffUIPrefab)target;
        if (GUILayout.Button("播放動畫"))
        {
            //bi.StartFadeAnime(1f);
        }
    }
}
#endif