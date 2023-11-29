using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VC2CT : MonoBehaviour
{
    public AnimationCurve MyCyrve;
    public float TimeDelay = 0.5f;
    public enum vertical_Direction { Up, Down};
    [SerializeField]
    private vertical_Direction _vertical_Direction;
    public float vertical_offset;
    private float _vo;

    public enum horizontal_Direction { Left, Right };
    [SerializeField]
    private horizontal_Direction _horizontal_Direction;
    public float horizontal_offset;
    private float _ho;
    //初始化
    //優化多餘的判定
    private void Start()
    {
        if(_vertical_Direction == vertical_Direction.Up)
        {
            _vo = vertical_offset;
        }
        else
        {
            _vo = -vertical_offset;
        }
        if(_horizontal_Direction == horizontal_Direction.Left)
        {
            _ho = horizontal_offset;
        }
        else
        {
            _ho = -horizontal_offset;
        }
    }
    //進入碰撞體
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //StartCoroutine(CFT_Toffset(_ho, _vo, TimeDelay, false));
        //DoTween的setter值是Lambda表達式 =>
        DOTween.To(() => VC2C.Instance.cftMTof, x => VC2C.Instance.cftMTof = x, new Vector3(_ho, _vo, 0f), TimeDelay).SetEase(Ease.InCubic);
    }
    /*
    private IEnumerator CFT_Toffset(float _hoc,float _voc,float timeDelay,bool fd)
    {
        float timer = 0f;
        while(timer < timeDelay)
        {
            timer += Time.deltaTime;
            if (fd)
            {
                
                VC2C.Instance.cftMTof = new Vector3(
                    Mathf.Lerp(_hoc, 0f, MyCyrve.Evaluate(timer / timeDelay)), 
                    Mathf.Lerp(_voc, 0f, MyCyrve.Evaluate(timer / timeDelay)), 0f);
            }
            else
            {
                VC2C.Instance.cftMTof = new Vector3(
                    Mathf.Lerp(0f, _hoc, MyCyrve.Evaluate(timer / timeDelay)),
                    Mathf.Lerp(0f, _voc, MyCyrve.Evaluate(timer / timeDelay)), 0f);
            }
            yield return null;
        }
    }
    */
    private void OnTriggerExit2D(Collider2D collision)
    {
        //StartCoroutine(CFT_Toffset(_ho, _vo, TimeDelay, true));
        DOTween.To(() => VC2C.Instance.cftMTof, x => VC2C.Instance.cftMTof = x, new Vector3(0f, 0f, 0f), TimeDelay).SetEase(Ease.OutCubic);
    }
}
