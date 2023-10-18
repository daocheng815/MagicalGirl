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
    public Material [] screemMaterial;
    public float screemMaterial_BW = 1f;


    private void Start()
    {
        //���s�I�]�w�K�����n�����D
        mybag.SetActive(true);
        mybag.SetActive(false);
        Screen.SetActive(false);

        foreach (var screemMaterial in screemMaterial)
        {
            screemMaterial.SetFloat("_TimE", screemMaterial_BW);
        }
    }

    

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

    public void OnOpenMyBag(InputAction.CallbackContext context)
    {
        if (context.canceled && playerController.IsAlive && !AVGSystem.isDialog)
        {
            //�NUI�J�I���ܷ�e����W
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
            if (isOpenEscMenu){
                PlayerController.lockplay = true;
                }
            else{
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
        AVGSystem.RemoveDialog_C();
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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
