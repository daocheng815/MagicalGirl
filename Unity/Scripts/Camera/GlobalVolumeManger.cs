using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class GlobalVolumeManger : Singleton<GlobalVolumeManger>
{
    public AnimationCurve myCurve;
    Volume _myVolume;
    private ColorAdjustments _colorAdjustments;
    private MotionBlur _motionBlur;
    private void Start()
    {   
        _myVolume = GetComponent<Volume>();
        _myVolume.profile.TryGet(out _colorAdjustments);
        _myVolume.profile.TryGet(out _motionBlur);
    }
    public void NewSaturation(float defNum,float timeDelay)
    {
        StartCoroutine(Newsaturation(defNum, timeDelay));
        IEnumerator Newsaturation(float defNum, float timeDelay)
        {
            var timer = 0f;
            while (timer < timeDelay)
            {
                timer += Time.deltaTime;
                _colorAdjustments.saturation.Override(Mathf.Lerp(0f,defNum,myCurve.Evaluate(timer/timeDelay)));
                yield return null;
            }
        }
    }

    public void motionBlurSt(float var)
    {
        _motionBlur.intensity.Override(var);
    }
    
}
