using Cinemachine;
using System.Collections;
using UnityEngine;

public class VC2C : Singleton<VC2C>
{
    public CinemachineVirtualCamera CV;
    public CinemachineFramingTransposer CFT;
    [Range(0f, 20f)]
    public float O_YD = 2f;
    public bool SYD_IE = false;
    public AnimationCurve MyCyrve;
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
        
        yield return StartCoroutine(CameraShock(0.05f,Random.Range(0.5f,1f),true, true));
        yield return StartCoroutine(CameraShock(0.05f, Random.Range(-1f, 0f), true, true));
        yield return StartCoroutine(CameraShock(0.05f, Random.Range(0.5f, 1f), true, true));
        yield return StartCoroutine(CameraShock(0.05f, Random.Range(-1f, 0f), true, true));
        yield return StartCoroutine(CameraShock(0.05f, Random.Range(0.5f, 1f), true, true));
        yield return StartCoroutine(CameraShock(0.05f, Random.Range(-1f, 0f), true, true));
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
        yield return StartCoroutine(CameraSize(Random.Range(6.5f,7f),0.3f));
        yield return StartCoroutine(CameraSize(7f, 0.2f));
        yield return StartCoroutine(CameraSize(Random.Range(6.5f, 7f), 0.2f));
        yield return StartCoroutine(CameraSize(7f, 0.3f));
    }
    //攝影機大小抖動
    public IEnumerator CameraSize(float size ,float delaytime)
    {
        float ord_size = CV.m_Lens.OrthographicSize;
        float timer = 0f;
        while(timer < delaytime)
        {
            float Osize = Mathf.Lerp(ord_size, size, (timer / delaytime));
            CV.m_Lens.OrthographicSize = Osize;
            timer += Time.deltaTime;
            yield return null;
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
            IEnumerator S_YDF(bool FD , float timedelay)
            {
                float timer = 0f;
                while (timer < timedelay)
                {
                    timer += Time.deltaTime;
                    if (FD)
                    {
                        CFT.m_YDamping = Mathf.Lerp(IN_OYD, OT_OYD, MyCyrve.Evaluate(timer / timedelay));
                    }
                    else
                    {
                        CFT.m_YDamping = Mathf.Lerp(OT_OYD, IN_OYD, MyCyrve.Evaluate(timer / timedelay));
                    }
                    yield return null;
                }
                SYD_IE = false;
            }
        }
    }
}
