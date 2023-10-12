using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class CVCamera_control : MonoBehaviour
{
    static CVCamera_control instance;
    public CinemachineVirtualCamera CV;
    static public CinemachineFramingTransposer CFT;
    static public bool cameraControlExecuted;
    private float CFT_O_YDamping;
    [SerializeField]
    private float CFT_O_ZDamping_Delay_time = 0.5f;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        //CFT.m_YDamping = 0;
    }
    void Start()
    {
        CFT = CV.GetCinemachineComponent<CinemachineFramingTransposer>();
        CFT_O_YDamping = CFT.m_YDamping;
    }
    //static public void CFT_YD(bool direction)
    //{
    //    if (direction)
    //    {
    //        instance.StartCoroutine(instance.Fade_CFT_YD(instance.CFT_O_ZDamping_Delay_time));
    //    }
    //    else
    //    {
    //        instance.CFT.m_YDamping = instance.CFT_O_YDamping;
    //    }
        
    //}
    //IEnumerator Fade_CFT_YD(float timedelay)
    //{
    //    float time = 0f;
    //    while (time < timedelay)
    //    {
    //        time += Time.deltaTime;
    //        Debug.Log(instance.CFT.m_YDamping);
    //        instance.CFT.m_YDamping = Mathf.Lerp(instance.CFT_O_YDamping, 0f, time / timedelay);
    //        CVCamera_control.cameraControlExecuted = false;
    //        yield return null;
    //    }
    //    CVCamera_control.cameraControlExecuted = true;
    //}

}
