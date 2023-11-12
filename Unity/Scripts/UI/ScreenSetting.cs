using System;
using System.Collections;
using System.Collections.Generic;
//DoTweening程式庫
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenSetting : MonoBehaviour
{
    //靜態
    public static int GameLoadNum;
    
    public GameObject loadingScreen;
    public Slider loadingSlidle;
    
    private GameObject TillImage;
    private GameObject logo;
    private GameObject menuButton;

    [SerializeField] private List<GameObject> uiList = new List<GameObject>();
    private void Awake()
    {
        TillImage = GameObject.Find("TillImage");
        logo = GameObject.Find("LogoMenu");
        menuButton = GameObject.Find("ButtonMenu");
        
        DontDestroyOnLoad(this.gameObject);
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
        GameLoadNum = loadNum;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync("SampleScene"));
        //SceneManager.LoadScene("SampleScene");
    }
    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        while (!loadOperation.isDone)
        {   
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            if (progress != loadingSlidle.value) 
            {
                loadingSlidle.value = progress;
            }
            Debug.Log("載入中");
            yield return null;
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
