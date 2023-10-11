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
        GameLoadNum = 0;
        SceneManager.LoadScene("SampleScene");
    }
    public void OnLoadGame(int LoadNum)
    {
        GameLoadNum = 1;
        SceneManager.LoadScene("SampleScene");
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
