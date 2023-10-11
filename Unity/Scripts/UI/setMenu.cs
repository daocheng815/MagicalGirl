using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class setMenu : MonoBehaviour
{
    public void ResetGame()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
