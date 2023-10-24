using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FunctionMenuUIControl : MonoBehaviour
{
    public GameObject Lord;
    public GameObject Save;
    public GameObject Menu;
    public GameObject Confirm_menu;
    public TextMeshProUGUI Confirm_menu_Title;
    public TextMeshProUGUI Confirm_menu_text;

    private string Confirm_menu_Title_save = "�s��";
    private string Confirm_menu_text_save = "�s�ɫ�N�|�л\���즳�s��\n�аݯu���n�n�i��s�ɶ�?\n";
    private string Confirm_menu_Title_load = "Ū��";
    private string Confirm_menu_text_load = "Ū�ɫ�N�|��󱼲{���i��\n�аݯu���n�n�i��Ū�ɶ�?\n";

    public FilleManager FilleManager;

    public GameObject[] NPC;

    public string mod;
    public int Num;
    public int NPCNum;
    public void Update()
    {
        if (mod == "save")
        {
            Confirm_menu_Title.text = Confirm_menu_Title_save;
            Confirm_menu_text.text = Confirm_menu_text_save;
        }
        else if ((mod == "load"))
        {
            Confirm_menu_Title.text = Confirm_menu_Title_load;
            Confirm_menu_text.text = Confirm_menu_text_load;
        }
    }
    public void PCLP(bool _switch)
    {
        PlayerController.lockplay = _switch;
    }
    public void Load_switch(bool _switch)
    {
        Lord.SetActive(_switch);
    }
    public void Save_switch(bool _switch)
    {
        Save.SetActive(_switch);
    }
    public void Menu_switch(bool _switch)
    {
        Menu.SetActive(_switch);
    }
    public void Confirm_menu_switch(bool _switch )
    {
        Confirm_menu.SetActive(_switch);
    }
    public void All_switch(bool _switch)
    {
        Lord.SetActive(_switch);
        Save.SetActive(_switch);
        Menu.SetActive(_switch);
        Confirm_menu.SetActive(_switch);
    }
    public void current_state_ismod(string ismod)
    {
        mod = ismod;
    }
    public void current_state_isNum( int isNum)
    {
        Num = isNum;
        Debug.Log(mod + Num);
    }
    public void NpcNum(int num)
    {
        NPCNum = num;
    }
    public void SL()
    {
        if(mod == "save")
        {
            FilleManager.Save(Num);
            CharacterEvents.characterText.Invoke(NPC[NPCNum], "�s�ɦ��\");
        }
        else if((mod == "load"))
        {
            FilleManager.Lord(Num);
        }
    }
}
