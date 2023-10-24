using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class NumValUIController : MonoBehaviour
{
    public PlayerController pc;
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

        StartCoroutine(AnimatorTimer(5f));
        Invoke("Fadeamimator",1f);
    }
    public void Fadeamimator()
    {
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0));
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1));
    }
    // 動畫效果
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

    public IEnumerator AnimatorTimer(float timedelay)
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

    public void Update()
    {
        
    }
}
