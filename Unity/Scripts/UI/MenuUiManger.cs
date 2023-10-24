using Flower;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuUiManger : MonoBehaviour
{
    public PlayerController playerController;
    public Damageable damageable;

    public GameObject mybag;
    public GameObject mybagSelected;

    public GameObject escMenu;
    public GameObject escMenuSelected;

    public AVGSystem AVGSystem;

    public bool isOpenMybag;
    public bool isOpenEscMenu;

    public GameObject Screen;
    public GameObject ScreenSelected;
    
    //public Material [] screemMaterial;
    //public float screemMaterial_BW = 1f;


    private void Start()
    {
        //重製背包已免不必要的問題
        mybag.SetActive(true);
        mybag.SetActive(false);
        Screen.SetActive(false);
        /*
        foreach (var screemMaterial in screemMaterial)
        {
            screemMaterial.SetFloat("_TimE", screemMaterial_BW);
        }*/
    }

    private void Update()
    {
        if (!damageable.IsAlive)
        {
            //將UI焦點移至當前物件上
            EventSystem.current.SetSelectedGameObject(ScreenSelected);
            GlobalVolumeManger.Instance.NewSaturation(-100f, 1f);
            Screen.SetActive(true);
        }
    }

    /*
    private void Update()
    {
        if (!damageable.IsAlive)
        {
            EventSystem.current.SetSelectedGameObject(ScreenSelected);
            Screen.SetActive(true);
            InvokeRepeating("fade", 0, 5);
            if (screemMaterial_BW <= 0)
            {
                CancelInvoke("fade");
            }
        }
    }


    private void fade()
    {
        screemMaterial_BW -= Time.deltaTime;
       
        foreach (var screemMaterial in screemMaterial)
        {
            screemMaterial.SetFloat("_TimE", screemMaterial_BW);
        }
    }
    */
    public void OnOpenMyBag(InputAction.CallbackContext context)
    {
        if (context.canceled && playerController.IsAlive && !AVGSystem.isDialog)
        {
            //將UI焦點移至當前物件上
            EventSystem.current.SetSelectedGameObject(mybagSelected);
            invventoryManger.RefreshItem();
            isOpenMybag = !isOpenMybag;
            mybag.SetActive(isOpenMybag);
            escMenu.SetActive(false);
            isOpenEscMenu = false;
        }
    }
    public void OnOpenEscMune(InputAction.CallbackContext context)
    {
        if (context.canceled && playerController.IsAlive && !AVGSystem.isDialog)
        {
            EventSystem.current.SetSelectedGameObject(escMenuSelected);
            isOpenEscMenu = !isOpenEscMenu;
            if (isOpenEscMenu)
            {
                PlayerController.lockplay = true;
            }
            else
            {
                PlayerController.lockplay = false;
            }
            escMenu.SetActive(isOpenEscMenu);
            mybag.SetActive(false);
            isOpenMybag= false;
        }
    }
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

    }

    public void ResetGame()
    {
        
        //GameObject ui = GameObject.Find("UIMager");
        //ui.GetComponent<UIManger>().SubscribeToEvents();
        //因為事前已經刪除對話框 所以要加回來
        AVGSystem.RemoveDialog_C();
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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
