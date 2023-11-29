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
        //����l�ư}�C�����ޭ�
        rtV3 = new Vector3[rtList.Length];
        //�M���}�C���Ҧ���
        for (int i = 0; i < rtList.Length; i++)
        {
            rtV3[i] = rtList[i].position;
            rtList[i].position = new Vector3(rtV3[i].x,rtV3[i].y+animatorValNum,rtV3[i].z);
        }
        //�C1��P�w���a�O�_���b�B��
        InvokeRepeating("GlobalTimer",0f,0.1f);
        //����@��A�}�l�Ҧ��p�ɾ��PUI�P�w
        Invoke("Fadeamimator",1f);
    }
    public void Fadeamimator()
    {
        ison = true;
        isExitUi = false;
        OnTimer();
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0));
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1));
        StartCoroutine(NumValAnimator(animatorValNum, waitTime, 2));
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
    //�}��UI�p�ɾ�
    void OnTimer()
    {
        timerBool = true;
        InvokeRepeating("AnimatorTimer",0f,1f);
    }
    //����UI�p�ɾ�
    void ExitTimer()
    {
        CancelInvoke("AnimatorTimer");
        timerBool = false;
        timer = 0;
    }
    // ����p�ɾ��A�ΨӧP�w���a�O�_���ʪ�����P�w
    private int GlobalTimerdelay;
    void GlobalTimer()
    {
        //�p�G���a���ʤF�N�Nbool�Ҧb��bTrue���A2S�A�P�w��׬O0.1s
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
        //���쩵��
        if (ison)
        {
            //�p�G�p�ɾ��ɶ���F�A�B�p�ɾ��}�ҵ۴N�����p�ɾ�
            if (timer > timerDelay && timerBool)
            {
                ExitTimer();
            }
            //�ɶ���F�N����UI
            if (!timerBool && !isExitUi)
            {
                if (!isplaymove)
                {
                    isExitUi = true;
                    StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0,false));
                    StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1,false));
                    StartCoroutine(NumValAnimator(animatorValNum, waitTime, 2,false));
                }
            }
            //�p�GUI�w�g�����A�B���a���b����
            if (!timerBool && isExitUi && isplaymove)
            {
                isExitUi = false;
                OnTimer();
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0,true));
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1,true));
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 2,true));
            }
            //�p�G���a���`�]�n�Y�W�h
            if (!pc.IsAlive)
            {
                isExitUi = true;
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 0,false));
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 1,false));
                StartCoroutine(NumValAnimator(animatorValNum, waitTime, 2,false));
            }
        }
    }
}
