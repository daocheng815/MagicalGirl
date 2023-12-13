using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BuffUIPrefab : MonoBehaviour
{
    public Image buffImage;
    public Image backGround;
    public Image timeBar;
    public TextMeshProUGUI buffOverlayNum;
    public GameObject buffOverlayNumGo;

    /// <summary>
    /// 開始Buff動畫
    /// </summary>
    /// <param name="time">Fade時間</param>
    /// <param name="buff">如名</param>
    /// <param name="be">buffEffect腳本</param>
    public void StartPlayerFadeAnime(float time,Buff buff,buffEffect be)
    {
        timeBar.fillAmount = 0f;
        timeBar.color = new Color(timeBar.color.r,timeBar.color.g,timeBar.color.b,0);//Alpha To 0.7
        backGround.color = new Color(backGround.color.r,backGround.color.g,backGround.color.b,0);//Alpha To 1
        buffImage.color = new Color(buffImage.color.r,buffImage.color.g,buffImage.color.b,0);//Alpha To 1

        backGround.sprite = buff.buffBackGround;
        buffImage.sprite = buff.buffImage;
        
        if(buff.buffOverlay)
        {
            //更新疊層
            buffOverlayNum.text = be.buffOverlayNum[buff.buffID].ToString();
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