using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenSetting : MonoBehaviour
{
    public static int GameLoadNum;
    private void Start()
    {
        GameLoadNum = 0;
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void OnStartGame()
    {
        OnSL(0);
    }
    public void OnLoadGame(int LoadNum)
    {
        OnSL(LoadNum);
    }
    void OnSL(int LoadNum = 1)
    {
        GameLoadNum = LoadNum;
        SceneManager.LoadScene("SampleScene");
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
