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
        //����l�ư}�C�����ޭ�
        rtV3 = new Vector3[rtList.Length];
        //�M���}�C���Ҧ���
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
        // �p�ɾ�
        OnTimer();
    }
    // ���a�ƭȱ��ʵe�ĪG
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
        Debug.Log("������{");
    }
    // �p�ɾ��A������
    //���U�ӭn�h���P�w���a���ʮɱҰʭp�ɾ��A�M��@�q�ɶ���N�NUI�Y�W�ӡA���p�G�b�o�~���S��M�D�J�ˮ`�άO���ʤΧ����A�N�NUI��U�ӡC
    //���ӷ|�ϥ�NumValAnimator��fadeMod
    void AnimatorTimer()
    {
        timer++;
        //Debug.Log("�p�ɤ�:" + timer);
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
