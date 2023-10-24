using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class NumValUIController : MonoBehaviour
{
    public PlayerController pc;
    [SerializeField]private bool timerBool = false;
    [SerializeField]private int timer = 0;
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
        Invoke("Fadeamimator",1f);
    }
    public void Fadeamimator()
    {
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0));
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1));
        // 計時器
        OnTimer();
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

    void OnTimer()
    {
        timerBool = true;
        InvokeRepeating("AnimatorTimer",0f,1f);
    }

    void ExitTimer()
    {
        CancelInvoke("AnimatorTimer");
        timerBool = false;
        timer = 0;
    }
    public void Update()
    {
        
        if (timer > timerDelay && timerBool)
        {
            ExitTimer();
        }
        if (!timerBool)
        {
            OnTimer();
        }
    }
}
