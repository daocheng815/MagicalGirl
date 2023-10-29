using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class NumValUIController : MonoBehaviour
{
    public PlayerController pc;
    [SerializeField]private bool isplaymove;
    [SerializeField]private bool timerBool = false;
    [SerializeField]private int timer = 0;
    private bool isExitUi = false;
    private bool ison = false;
    public int timerDelay = 5;
    public float waitTime = 1f;
    public float animatorValNum = 100f;
    public AnimationCurve animatorcurve;
    public RectTransform[] rtList;
    private Vector3[] rtV3;
    public void Start()
    {
        //先初始化陣列的索引值
        rtV3 = new Vector3[rtList.Length];
        //遍歷陣列內所有值
        for (int i = 0; i < rtList.Length; i++)
        {
            rtV3[i] = rtList[i].position;
            rtList[i].position = new Vector3(rtV3[i].x,rtV3[i].y+animatorValNum,rtV3[i].z);
        }
        //每1秒判定玩家是否正在運動
        InvokeRepeating("GlobalTimer",0f,0.1f);
        //延遲一秒再開始所有計時器與UI判定
        Invoke("Fadeamimator",1f);
    }
    public void Fadeamimator()
    {
        ison = true;
        isExitUi = false;
        OnTimer();
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0));
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1));
    }
    // 玩家數值條動畫效果
    public IEnumerator NumValAnimator(float val = 50f,float timeDelay = 1f,int NumVal = 0,bool fadeMod = true)
    {
        var timer = 0f;
        while (timer < timeDelay)
        {
            timer += Time.deltaTime;
            rtList[NumVal].position = new Vector3
            ( 
                rtList[NumVal].position.x, 
                Mathf.Lerp( 
                                fadeMod ? rtV3[NumVal].y + val : rtV3[NumVal].y,
                                fadeMod ? rtV3[NumVal].y : rtV3[NumVal].y + val,
                                animatorcurve.Evaluate(timer/timeDelay)
                            ), 
                rtList[NumVal].position.z
            );
            yield return null;
        }
    }

    public IEnumerator AnimatorTimer_(float timedelay)
    {
        var timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            //if (pc.canMove)
            //    break;
            yield return null;
        }
        Debug.Log("結束協程");
    }
    // 計時器，未完成
    //接下來要去做判定當玩家不動時啟動計時器，然後一段時間後就將UI縮上來，但如果在這途中又突然遭遇傷害或是移動及攻擊，就將UI放下來。
    //應該會使用NumValAnimator的fadeMod
    void AnimatorTimer()
    {
        timer++;
        //Debug.Log("計時中:" + timer);
    }
    //開啟UI計時器
    void OnTimer()
    {
        timerBool = true;
        InvokeRepeating("AnimatorTimer",0f,1f);
    }
    //關閉UI計時器
    void ExitTimer()
    {
        CancelInvoke("AnimatorTimer");
        timerBool = false;
        timer = 0;
    }
    // 全域計時器，用來判定玩家是否移動的延遲判定
    private int GlobalTimerdelay;
    void GlobalTimer()
    {
        //如果玩家移動了就將bool所在鎖在True狀態2S，判定精度是0.1s
        if (isplaymove && GlobalTimerdelay < 20)
        {
            if(pc.rb.velocity.x != 0 || pc.rb.velocity.y != 0 )
                GlobalTimerdelay = 0;
            GlobalTimerdelay++;
            isplaymove = isplaymove;
        }
        else
        {
            GlobalTimerdelay = 0;
            isplaymove = (pc.rb.velocity.x != 0 || pc.rb.velocity.y != 0 );
        }
    }
    public void Update()
    {
        //全域延遲
        if (ison)
        {
            //如果計時器時間到了，且計時器開啟著就關閉計時器
            if (timer > timerDelay && timerBool)
            {
                ExitTimer();
            }
            //時間到了就關閉UI
            if (!timerBool && !isExitUi)
            {
                if (!isplaymove)
                {
                    isExitUi = true;
                    StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0,false));
                    StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1,false));
                }
            }
            //如果UI已經關閉，且玩家有在移動
            if (!timerBool && isExitUi && isplaymove)
            {
                isExitUi = false;
                OnTimer();
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0,true));
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1,true));
            }
            //如果玩家死亡也要縮上去
            if (!pc.IsAlive)
            {
                isExitUi = true;
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0,false));
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1,false));
            }
        }
    }
}
