using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

public class CoolingUI : Singleton<CoolingUI>
{
    [SerializeField] private Sprite[] Images;
    [SerializeField] private GameObject coolingPrefad;
    private Image[] backImages;
    public GameObject[] prefad;
    public Image[] coolingImage;
    void Start()
    {
        //���]�}�C�ƶq
        prefad = new GameObject[Images.Length];
        coolingImage = new Image[Images.Length];
        for (int i = 0; i < Images.Length; i++) 
        {
            //��Ҥƹw���
            prefad[i] = Instantiate(coolingPrefad, Vector3.zero, Quaternion.identity);
            //�]�w����
            prefad[i].transform.SetParent(gameObject.transform);
            //�����ޯ�ϥ�
            prefad[i].transform.Find("skill").GetComponent<Image>().sprite = Images[i];
            //���o�N�o�ɶ��Ϥ�
            coolingImage[i] = prefad[i].transform.Find("cooling").GetComponent<Image>();
            //�]�w���T���Y��j�p
            prefad[i].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }

        foreach (var i in coolingImage)
        {
            i.fillAmount = 0f;
        }

        foreach (var i in prefad)
        {
            i.SetActive(false);
        }
    }

    private bool _isFadeAnimator;
    // ReSharper disable Unity.PerformanceAnalysis
    public void CoolingTime(int num, float time , float fadeTime)
    {
        
        var back = prefad[num].transform.Find("back").GetComponent<Image>();
        var skill = prefad[num].transform.Find("skill").GetComponent<Image>();
        if (_isFadeAnimator)
        {
            DOTween.Kill(back);
            DOTween.Kill(skill);
            _isFadeAnimator = false;
        }
        back.color = new Color(1, 1, 1, 0);
        skill.color = new Color(1, 1, 1, 0);


        back.DOColor(Color.white, 0.1f);
        skill.DOColor(Color.white, 0.1f);
        prefad[num].SetActive(true);
        coolingImage[num].fillAmount = 1f;
        coolingImage[num].DOFillAmount(0f, time).OnComplete((() =>
        {
            coolingImage[num].fillAmount = 0f;
            _isFadeAnimator = true;
            back.DOColor(Color.clear,fadeTime);
            skill.DOColor(Color.clear,fadeTime).OnComplete((() =>
            {
                prefad[num].SetActive(false);
                _isFadeAnimator = false;
            }));
        }));
    }
}
/*
[CustomEditor(typeof(CoolingUI))]
public class CoolingUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CoolingUI coolingUI = (CoolingUI)target;

        if (GUILayout.Button("Trigger Cooling Time"))
        {
            coolingUI.CoolingTime(1,0.5f,0.3f);
        }
    }
}
*/
