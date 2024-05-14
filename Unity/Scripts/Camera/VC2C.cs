using Cinemachine;
using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Serialization;

public class VC2C : Singleton<VC2C>
{
    public CinemachineVirtualCamera CV;
    public CinemachineFramingTransposer CFT;

    public CinemachineVirtualCamera[] cvGroup;

    public int isCameraNum;
    
    [Range(0f, 20f)]
    public float O_YD = 2f;
    public bool SYD_IE = false;
    public AnimationCurve MyCyrve;

   
    public Vector3 cftMTof
    {
        get => CV.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        set => CV.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = value;
    }
    
    public void UpdateCameraNum()
    {
        for (int i = 0; i < cvGroup.Length-1; i++)
        {
            if (cvGroup[i] == CV)
            {
                isCameraNum = i;
                break;
            }
        }
    }
    void Update()
    {
        //CFT.m_YDamping = 0;
    }
    void Start()
    {
        CFT = CV.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // 攝影機震動
    public IEnumerator CameraShock_Num()
    {
        
        yield return StartCoroutine(CameraShock(0.05f,Random.Range(0.7f,1f),true, true));
        yield return StartCoroutine(CameraShock(0.05f, Random.Range(-0.7f, 0f), true, true));
        //yield return StartCoroutine(CameraShock(0.05f, Random.Range(0.5f, 1f), true, true));
        //yield return StartCoroutine(CameraShock(0.05f, Random.Range(-1f, 0f), true, true));
        //yield return StartCoroutine(CameraShock(0.05f, Random.Range(0.5f, 1f), true, true));
        //yield return StartCoroutine(CameraShock(0.05f, Random.Range(-1f, 0f), true, true));
        CFT.m_TrackedObjectOffset = Vector3.zero;
    }
    public IEnumerator CameraShock(float timedelay, float shockNum, bool X, bool Y)
    {
        float timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            float offsetx = CFT.m_TrackedObjectOffset.x;
            float offsety = CFT.m_TrackedObjectOffset.y;
            var ox = Mathf.Lerp(offsetx, shockNum, (timer / timedelay));
            var oy = Mathf.Lerp(offsety, shockNum, (timer / timedelay));
            if (X)
                CFT.m_TrackedObjectOffset = new Vector3(ox, offsety, 0f);
            if (Y)
                CFT.m_TrackedObjectOffset = new Vector3(offsetx, oy, 0f);
            yield return null;
        }
    }

    public IEnumerator CameraSize_Num()
    {
        yield return StartCoroutine(CameraSize(Random.Range(6.8f,7f),0.2f));
        yield return StartCoroutine(CameraSize(7f, 0.1f));
        yield return StartCoroutine(CameraSize(Random.Range(6.8f, 7f), 0.15f));
        yield return StartCoroutine(CameraSize(7f, 0.1f));
    }
    //攝影機大小抖動
    public IEnumerator CameraSize(float size ,float delaytime)
    {
        float ordSize = CV.m_Lens.OrthographicSize;
        float timer = 0f;
        while(timer < delaytime)
        {
            float osize = Mathf.Lerp(ordSize, size, (timer / delaytime));
            CV.m_Lens.OrthographicSize = osize;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    //攝影機偏移(用於腳色翻轉)
    public void CameraOffset(float offsetVar,float timeDelay)
    {
        if (!CFT.enabled) return;
        StartCoroutine(CameraOffsetSet(offsetVar, timeDelay));
        return;
        IEnumerator CameraOffsetSet(float offsetVarVar ,float timeDelayOld)
        {
            var timer = 0f;
            while (timer<timeDelayOld)
            {
                timer += Time.deltaTime;
                CFT.m_TrackedObjectOffset.x = Mathf.Lerp(0f, offsetVarVar, MyCyrve.Evaluate(timer / timeDelayOld));
                yield return null;
            }
        }
    }
    /// <summary>
    /// 控制攝影機的YDamping值，當值為Treu執行IN_OYD TO 0f，為False執行0f TO N_OYD。
    /// </summary>
    /// <param name="FD"></param>
    /// <param name="timedelay"></param>
    public void SYD(bool FD, float timedelay, float IN_OYD,float OT_OYD)
    {
        // 在執行但入時要同時判定玩家是否有在下降、使正在執行協程、是否已經到達極值
        if(CFT.enabled)//CinemachineFramingTransposer存在時才執行
        {
            SYD_IE = true;
            StartCoroutine(S_YDF(FD, timedelay));
            IEnumerator S_YDF(bool FD , float timeDelay)
            {
                float timer = 0f;
                while (timer < timeDelay)
                {
                    timer += Time.deltaTime;
                    if (FD)
                    {
                        CFT.m_YDamping = Mathf.Lerp(IN_OYD, OT_OYD, MyCyrve.Evaluate(timer / timeDelay));
                    }
                    else
                    {
                        CFT.m_YDamping = Mathf.Lerp(OT_OYD, IN_OYD, MyCyrve.Evaluate(timer / timeDelay));
                    }
                    yield return null;
                }
                SYD_IE = false;
            }
        }
    }
}
