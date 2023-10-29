using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenSetting : MonoBehaviour
{
    //�R�A
    public static int GameLoadNum;

    [SerializeField]private bool isLordMenuOn = false;
    [SerializeField]private bool isSettingsMenuOn = false;
    
    public AnimationCurve myCurve;
    public GameObject loadingScreen;
    public Slider loadingSlidle;
    
    private GameObject logo;
    private GameObject menuButton;
    private GameObject lordMenu;
    private GameObject settingsMenu;
    
    private Vector2 logoMenuShaft;
    private Vector2 menuButtonShaft;
    private Vector2 lordMenuShaft;
    private void Start()
    {
        GameLoadNum = 0;
        //���o���󪺭�l��m
        logoMenuShaft = logo.GetComponent<RectTransform>().anchoredPosition;
        menuButtonShaft = menuButton.GetComponent<RectTransform>().anchoredPosition;
        lordMenuShaft = lordMenu.GetComponent<RectTransform>().anchoredPosition;

        lordMenu.SetActive(false);
    }
    private void Awake()
    {
        logo = GameObject.Find("LogoMenu");
        menuButton = GameObject.Find("ButtonMenu");
        lordMenu = GameObject.Find("LordMenu");
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
            StartCoroutine(OnLoadUiMenuIe(logo,myCurve,-200,logoMenuShaft.y,0.4f,swap));
            StartCoroutine(OnLoadUiMenuIe(menuButton,myCurve,900,menuButtonShaft.y,0.5f,swap));
            StartCoroutine(OnLoadUiMenuIe(lordMenu,myCurve,930,lordMenuShaft.y,0.6f,swap));
            isLordMenuOn = swap;
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
            Debug.Log("���J��");
            yield return null;
        }
    }
    
    
    public void QuitGame()
    {
        // ���ε{���h�X
        Application.Quit();

        // �`�N�G�bUnity�s�边������ɡAQuit()�i�ण�|�ߧY�ͮ�
        // �b�s�边�����ծɡA�A�i�H�ϥΨ�L�覡�B�z���}�C��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
