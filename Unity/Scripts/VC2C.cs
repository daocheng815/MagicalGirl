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
