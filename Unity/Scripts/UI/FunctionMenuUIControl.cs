using System.Collections;
using TMPro;
using UnityEngine;

public class FunctionMenuUIControl : MonoBehaviour
{
    public GameObject player;
    private Damageable playDamageable;
    private TouchingDirections TouchingDirections;
    
    public GameObject Lord;
    public GameObject Save;
    public GameObject Menu;
    public GameObject Confirm_menu;
    public TextMeshProUGUI Confirm_menu_Title;
    public TextMeshProUGUI Confirm_menu_text;

    private string Confirm_menu_Title_save = "存檔";
    private string Confirm_menu_text_save = "存檔後將會覆蓋掉原有存檔\n請問真的要要進行存檔嗎?\n";
    private string Confirm_menu_Title_load = "讀檔";
    private string Confirm_menu_text_load = "讀檔後將會放棄掉現有進度\n請問真的要要進行讀檔嗎?\n";

    public FilleManager FilleManager;

    public GameObject[] NPC;

    public string mod;
    public int Num;
    public int NPCNum;

    private void Awake()
    {
        playDamageable = player.GetComponent<Damageable>();
        TouchingDirections = player.GetComponent<TouchingDirections>();
    }

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
        if(_switch == false)
            PlayerController.lockplay = _switch;
        else
        {
            // 以下控制到地面才開始鎖住玩家
            if (!TouchingDirections.IsGrounded)
            {
                playDamageable.LockVelocity = true;
                StartCoroutine(Lock_player_timer());
            }
            else
            {
                PlayerController.lockplay = true;
            }
            IEnumerator Lock_player_timer()
            {
                var timer = 0f;
                var timedelay = 5f;
                while (timer < timedelay)
                {
                    timer += Time.deltaTime;
                    
                    if (TouchingDirections.IsGrounded)
                    {
                        PlayerController.lockplay = true;
                        playDamageable.LockVelocity = false;
                        //Debug.Log("落地");
                        timer = timedelay;
                    }
                    yield return null;
                }
            }
        }
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
            CharacterEvents.characterText.Invoke(NPC[NPCNum], "存檔成功");
        }
        else if((mod == "load"))
        {
            FilleManager.Lord(Num);
        }
    }
}
