using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Events;
using UnityEngine.UI;
using System.IO;
using SaveLord;
using TMPro;
using currentLevel;

public class ScreenSetting : MonoBehaviour
{
    public GameObject loadingScreen;

    [SerializeField]private TextMeshProUGUI[] loadText; 
    
    private GameObject TillImage;
    private GameObject logo;
    private GameObject menuButton;

    [SerializeField] private List<GameObject> uiList = new List<GameObject>();
    private void Awake()
    {
        TillImage = GameObject.Find("TillImage");
        logo = GameObject.Find("LogoMenu");
        menuButton = GameObject.Find("ButtonMenu");
        
        //DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        //GameLoadNum = 0;
        foreach (GameObject i in uiList)
        {
            i.SetActive(true);
        }
    }
    
    private void OnUI(bool swap ,GameObject go)
    {

        TillImage.transform.DOLocalMoveX(swap? 1200 : 530,1).SetEase(Ease.OutQuart);
        DOTween.ToAlpha(() => TillImage.GetComponent<Image>().color,  color => TillImage.GetComponent<Image>().color = color, swap ? 0f:1f, 1).SetEase(Ease.OutQuart);
        logo.transform.DOLocalMove(swap ? new Vector2(-790,492) : new Vector2(0,300),1).SetEase(Ease.OutQuart);
        logo.transform.DOScale(swap ? new Vector3(0.3f,0.3f,0.3f) : new Vector3(1f,1f,1f),1).SetEase(Ease.OutQuart);
        menuButton.transform.DOLocalMoveY(swap ? 400 : -464 ,1).SetEase(Ease.OutQuart);
        go.transform.DOLocalMoveY(swap ? -16 : -913 ,1).SetEase(Ease.OutQuart);
        if (swap)
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                if (uiList[i] != go)
                {
                    uiList[i].transform.DOLocalMoveY( -913 ,1).SetEase(Ease.OutQuart);
                }
            }
        }
    }
    
    public void OnLoadUiMenu(bool swap)
    {
        //更新UI描述
        for (int i = 1; i < 4; i++)
        {
            
            PlayerData pd = ArchiveSystemManager.Instance.Lord(i);
            if (pd != null) loadText[i - 1].text = LevelName.LevelNames[pd.myLevelState] + " " + pd.saveIsTime;
            else loadText[i - 1].text = "無";
        }
        OnUI(swap,uiList[0]);
    }
    public void OnAboutUiMenu(bool swap)
    {
        OnUI(swap,uiList[1]);
    }
    public void OnSettingsUiMenu(bool swap)
    {
        OnUI(swap,uiList[2]);
    }

    public void OnSL(int loadNum = 0)
    {   
        // 检查文件是否存在
        if (File.Exists(Path.Combine(Application.dataPath, "playerDataSave_" + loadNum + ".json"))||loadNum==0)
        {
            Persistence.GameLoadNum = loadNum;
            loadingScreen.SetActive(true);
        
            LodingManger.Instance.LodingScene("SampleScene");
        }
        else
        {
            Debug.Log("該存檔不存在");
        }
    }
    
    public void QuitGame()
    {
        // 應用程式退出
        Application.Quit();

        // 注意：在Unity編輯器中執行時，Quit()可能不會立即生效
        // 在編輯器中測試時，你可以使用其他方式處理離開遊戲
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
