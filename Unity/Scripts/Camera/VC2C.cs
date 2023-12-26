using Cinemachine;
using System.Collections;
using Events;
using UnityEngine;

public class VC2C : Singleton<VC2C>
{
    public CinemachineVirtualCamera CV;
    public CinemachineFramingTransposer CFT;
    [Range(0f, 20f)]
    public float O_YD = 2f;
    public bool SYD_IE = false;
    public AnimationCurve MyCyrve;

    public Vector3 cftMTof
    {
        get => CV.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        set => CV.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = value;
    }
    void Update()
    {
        //CFT.m_YDamping = 0;
    }
    void Start()
    {
        CFT = CV.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // ��v���_��
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
    //��v���j�p�ݰ�
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
    //��v������(�Ω�}��½��)
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
    /// ������v����YDamping�ȡA��Ȭ�Treu����IN_OYD TO 0f�A��False����0f TO N_OYD�C
    /// </summary>
    /// <param name="FD"></param>
    /// <param name="timedelay"></param>
    public void SYD(bool FD, float timedelay, float IN_OYD,float OT_OYD)
    {
        // �b������J�ɭn�P�ɧP�w���a�O�_���b�U���B�ϥ��b�����{�B�O�_�w�g��F����
        if(CFT.enabled)//CinemachineFramingTransposer�s�b�ɤ~����
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
