
using DG.Tweening;
using Events;
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
        //重設陣列數量
        prefad = new GameObject[Images.Length];
        coolingImage = new Image[Images.Length];
        for (int i = 0; i < Images.Length; i++) 
        {
            //實例化預制件
            prefad[i] = Instantiate(coolingPrefad, Vector3.zero, Quaternion.identity);
            //設定父級
            prefad[i].transform.SetParent(gameObject.transform);
            //替換技能圖示
            prefad[i].transform.Find("skill").GetComponent<Image>().sprite = Images[i];
            //取得冷卻時間圖片
            coolingImage[i] = prefad[i].transform.Find("cooling").GetComponent<Image>();
            //設定正確的縮放大小
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
