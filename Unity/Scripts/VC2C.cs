using Cinemachine;
using System.Collections;
using UnityEngine;

public class VC2C : Singleton<VC2C>
{
    public CinemachineVirtualCamera CV;
    public CinemachineFramingTransposer CFT;
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
