using System;
using Events;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class NumValUIController : MonoBehaviour
{
    public PlayerController pc;
    public RectTransform[] rtList;
    private Vector3[] _rtShowPosList = new Vector3[3];
    private Vector3[] _rtHidePosList = new Vector3[3];
    public int movepos;
    private Tween[] _showTweens = new Tween[3];
    private Tween[] _hideTweens = new Tween[3];
    public enum FadeMode { Show,Hide }
    [FormerlySerializedAs("_fadeMode")] [SerializeField]private FadeMode fadeMode;

    public float fadeTime;
    public float hideTime;
    private float hideTimeDelay;
    
    public void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            var position = rtList[i].position;
            _rtShowPosList[i] = position;
            position = new Vector3(position.x, position.y + movepos, position.z);
            rtList[i].position = position;
            _rtHidePosList[i] = position;
        }
        fadeMode = FadeMode.Hide;
        //每1秒判定玩家是否正在運動
        InvokeRepeating("GlobalTimer",0f,0.1f);
        ShowValUi();
    }
    
    // 全域計時器，用來判定玩家是否移動的延遲判定
    private int GlobalTimerdelay;
    [SerializeField] private bool isplaymove;
    void GlobalTimer()
    {
        //如果玩家移動了就將bool所在鎖在True狀態2S，判定精度是0.1s
        if (isplaymove && GlobalTimerdelay < 20)
        {
            if(pc.rb.velocity.x != 0 || pc.rb.velocity.y != 0 )
                GlobalTimerdelay = 0;
            GlobalTimerdelay++;
        }
        else
        {
            GlobalTimerdelay = 0;
            isplaymove = (pc.rb.velocity.x != 0 || pc.rb.velocity.y != 0 );
        }
    }
    public void Update()
    {
        if (isplaymove)
        {
            ShowValUi();
        }
        switch (fadeMode)
        {
            case FadeMode.Show:
                hideTimeDelay += Time.deltaTime;
                if (hideTimeDelay >= hideTime)
                {
                    hideTimeDelay = 0;
                    HideValUi();
                }
                break;
            case FadeMode.Hide:
                break;
        }
        
    }

    private void OnEnable()
    {
        UiEvents.ShowValUi += ShowValUi;
        UiEvents.HideValUi += HideValUi;
    }

    private void OnDisable()
    {
        UiEvents.ShowValUi -= ShowValUi;
        UiEvents.HideValUi -= HideValUi;
    }

    public void HideValUi()
    {
        foreach (var t in _showTweens) { t.Kill(); }
        foreach (var t in _hideTweens) { t.Kill(); }
        for (int i = 0; i < 3; i++)
        {
            _hideTweens[i] = rtList[i].DOMove(_rtHidePosList[i], fadeTime * 2).SetEase(Ease.OutQuad);
        }
        fadeMode = FadeMode.Hide;
    }
    
    public void ShowValUi()
    {
        foreach (var t in _showTweens) { t.Kill(); }
        foreach (var t in _hideTweens) { t.Kill(); }
        hideTimeDelay = 0;
        for (int i = 0; i < 3; i++)
        {
            _showTweens[i] = rtList[i].DOMove(_rtShowPosList[i], fadeTime ).SetEase(Ease.OutQuad);
        }
        fadeMode = FadeMode.Show;
    }
}
