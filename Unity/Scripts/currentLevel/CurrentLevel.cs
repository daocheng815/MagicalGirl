using System.Collections;
using System.Collections.Generic;
using currentLevel;
using DG.Tweening;
using UnityEngine;
//�a�ϵ��Ū��A��
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

public class CurrentLevel : Singleton<CurrentLevel>
{
    public LevelState levelState = new LevelState();

    [SerializeField]private GameObject text;
    private TextMeshProUGUI textUGUI;
    private RectTransform textRI;
    
    [SerializeField]private GameObject image;
    private RectTransform imageRT;
    
    protected void Awake()
    {
        base.Awake();
        textUGUI = text.GetComponent<TextMeshProUGUI>();
        textRI = text.GetComponent<RectTransform>();
        imageRT = image.GetComponent<RectTransform>();
    }

    private void Start()
    {
        text.SetActive(false);
        image.SetActive(false);
    }

    protected bool isanimator = false;
    public void OnUiShow(bool index = false)
    {
        textUGUI.text = levelState.LevelName[levelState.CurrentState];
        if (isanimator)
        {
            DOTween.Kill(imageRT);
        }
        isanimator = true;
        if (index)
        {
            
            image.SetActive(true);
            imageRT.localScale = new Vector3(0, 0, 1f);
            imageRT.DOScaleX(1, 0.3f).OnComplete(() =>
            {
                imageRT.DOScaleY(1, 1f).OnUpdate((() =>
                {
                    if (imageRT.localScale.y >= 0.9f)
                    {
                        text.SetActive(true);
                    }
                }));
            });
        }
        else
        {
            imageRT.localScale = new Vector3(1f, 1f, 1f);
            imageRT.DOScaleY(0, 0.3f).OnUpdate(() =>
            {
                if (imageRT.localScale.y <= 0.6f)
                    text.SetActive(false);
            }).OnComplete((() =>
            {
                imageRT.DOScaleX(0, 1f).OnComplete((() =>
                {
                    image.SetActive(false);
                }));
            }));
        }
        
        
    }
    public void OnUiShowAndHide(float delayTime)
    {
        OnUiShow(true);

        StartCoroutine(SD(delayTime));
        IEnumerator SD(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            OnUiShow(false);
        }
    }
}
/*
[CustomEditor(typeof(CurrentLevel))]
public class CurrentLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CurrentLevel currentLevel = (CurrentLevel)target;

        if (GUILayout.Button("��ܷ�e�a��"))
        {
            currentLevel.OnUiShow(true);
        }
        if (GUILayout.Button("���÷�e�a��"))
        {
            currentLevel.OnUiShow(false);
        }
        if (GUILayout.Button("�}�Ҩõ��ݫᵲ��"))
        {
            currentLevel.OnUiShowAndHide(3f);
        }
    }
}
*/