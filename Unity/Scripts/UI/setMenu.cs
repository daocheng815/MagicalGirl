using UnityEngine;
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
        // ���ε{���h�X
        Application.Quit();

        // �`�N�G�bUnity�s�边������ɡAQuit()�i�ण�|�ߧY�ͮ�
        // �b�s�边�����ծɡA�A�i�H�ϥΨ�L�覡�B�z���}�C��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
