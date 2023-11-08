using System;
using System.Collections;
using System.Collections.Generic;
//DoTweening程式庫
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenSetting : MonoBehaviour
{
    //靜態
    public static int GameLoadNum;
    
    //ui控制
    [SerializeField]private bool isLordMenuOn = false;
    [SerializeField]private bool isAboutMenuOn = false;
    [SerializeField]private bool isSettingsMenuOn = false;
    
    public AnimationCurve myCurve;
    public GameObject loadingScreen;
    public Slider loadingSlidle;
    
    private GameObject logo;
    private GameObject menuButton;
    private GameObject lordMenu;
    private GameObject AboutMenu;
    private GameObject SettingsMenu;
    
    private Vector2 logoMenuShaft;
    private Vector2 menuButtonShaft;
    private Vector2 lordMenuShaft;
    private Vector2 AboutMenuShaft;
    private Vector2 SettingsMenuShaft;
    private void Start()
    {
        GameLoadNum = 0;
        lordMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        AboutMenu.SetActive(false);
    }
    private void Awake()
    {
        logo = GameObject.Find("LogoMenu");
        menuButton = GameObject.Find("ButtonMenu");
        
        lordMenu = GameObject.Find("LordMenu");
        AboutMenu = GameObject.Find("AboutMenu");
        SettingsMenu = GameObject.Find("SettingsMenu");
        
        //取得遊戲物件內的RectTransform原始位置
        logoMenuShaft = logo.GetComponent<RectTransform>().anchoredPosition;
        menuButtonShaft = menuButton.GetComponent<RectTransform>().anchoredPosition;
        
        lordMenuShaft = lordMenu.GetComponent<RectTransform>().anchoredPosition;
        AboutMenuShaft = AboutMenu.GetComponent<RectTransform>().anchoredPosition;
        SettingsMenuShaft = SettingsMenu.GetComponent<RectTransform>().anchoredPosition;
        
        DontDestroyOnLoad(this.gameObject);
    }
    public void OnStartGame()
    {
        OnSL(0);
    }
    public void OnLoadGame(int loadNum)
    {
        OnSL(loadNum);
    }
    
    public void OnLoadUiMenu(bool swap)
    {
        lordMenu.SetActive(swap);
        if (swap ? !isLordMenuOn : isLordMenuOn)
        {
            GameObject.Find("Test").transform.DOLocalMoveY(swap ? -300: GameObject.Find("Test").GetComponent<RectTransform>().localScale.y,1).SetEase(Ease.OutQuart);
            logo.transform.DOLocalMoveY(swap ? 200 : logo.GetComponent<RectTransform>().localScale.y ,1);
            //StartCoroutine(OnLoadUiMenuIe(logo,myCurve,-200,logoMenuShaft.y,0.4f,swap));
            menuButton.transform.DOLocalMoveY(swap ? 300 : menuButton.GetComponent<RectTransform>().localScale.y ,1).SetEase(Ease.OutQuart);
            //StartCoroutine(OnLoadUiMenuIe(menuButton,myCurve,900,menuButtonShaft.y,0.5f,swap));
            //StartCoroutine(OnLoadUiMenuIe(lordMenu,myCurve,930,lordMenuShaft.y,0.6f,swap));
            isLordMenuOn = swap;
        }
    }
    public void OnAboutUiMenu(bool swap)
    {
        AboutMenu.SetActive(swap);
        if (swap ? !isAboutMenuOn : isAboutMenuOn)
        {
            StartCoroutine(OnLoadUiMenuIe(logo,myCurve,-200,logoMenuShaft.y,0.4f,swap));
            StartCoroutine(OnLoadUiMenuIe(menuButton,myCurve,900,menuButtonShaft.y,0.5f,swap));
            StartCoroutine(OnLoadUiMenuIe(AboutMenu,myCurve,930,AboutMenuShaft.y,0.6f,swap));
            isAboutMenuOn = swap;
        }
    }
    public void OnSettingsUiMenu(bool swap)
    {
        SettingsMenu.SetActive(swap);
        if (swap ? !isSettingsMenuOn : isSettingsMenuOn)
        {
            StartCoroutine(OnLoadUiMenuIe(logo,myCurve,-200,logoMenuShaft.y,0.4f,swap));
            StartCoroutine(OnLoadUiMenuIe(menuButton,myCurve,900,menuButtonShaft.y,0.5f,swap));
            StartCoroutine(OnLoadUiMenuIe(SettingsMenu,myCurve,930,SettingsMenuShaft.y,0.6f,swap));
            isSettingsMenuOn = swap;
        }
    }
    
    /// <summary>
    /// UI Offset Animator
    /// </summary>
    /// <param name="go">UI GameObject</param>
    /// <param name="addOfssetNumShaft">add num</param>
    /// <param name="orgOffsetShaft">org num</param>
    /// <param name="timerDelay">timerDela</param>
    /// <param name="fadeMod">true = in , false = out</param>
    /// <returns></returns>
    public IEnumerator OnLoadUiMenuIe(GameObject go,AnimationCurve curve, int addOfssetNumShaft , float orgOffsetShaft,float timerDelay,bool fadeMod = true)
    {
        float timer = 0f;
        RectTransform rt = go.GetComponent<RectTransform>();
        while (timer < timerDelay)
        {
            timer += Time.deltaTime;
            var newMenuButtonRT = Mathf.Lerp
            (
                fadeMod ? orgOffsetShaft : orgOffsetShaft + addOfssetNumShaft, 
                fadeMod ? orgOffsetShaft + addOfssetNumShaft : orgOffsetShaft, 
                curve.Evaluate(timer / timerDelay)
            );
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, newMenuButtonRT);
            yield return null;
        }
    }
    void OnSL(int loadNum = 1)
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
