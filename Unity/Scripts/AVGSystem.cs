using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Invoke("RemoveButtonGroup", 0.4f);這一段在之後必須要修改
public class AVGSystem : MonoBehaviour
{
    FlowerSystem fs;

    // 任何物件都必須初始化這樣在重新開始遊戲時才不會產生尋找不到物件的情況
    public GameObject NPC1;
    private string _NPC1;
    public GameObject NPC2;
    private string _NPC2;
    //判定是否暫停系統
    bool IsFsStop = false;
    public item thisItem1;
    public item thisItem2;

    public bool isDialog;

    public FilleManager FilleManager;

    // Start is called before the first frame update
    void Start()
    {

        NPC1 = GameObject.Find("QB");
        _NPC1 = NPC1.name;
        NPC1.SetActive(false);
        NPC2 = GameObject.Find("NPC1_0");
        _NPC2 = NPC2.name;

        //呼叫對話系統
        fs = FlowerManager.Instance.CreateFlowerSystem("FlowerSample", false);

        fs.Stop();
        fs.Resume();
        fs.SetupDialog();
        if (ScreenSetting.GameLoadNum == 1)
        {
            FilleManager.Lord(1);
        }
        if (ScreenSetting.GameLoadNum == 0)
        {

            fs.ReadTextFromResource("start1");
        }

        fs.SetVariable("p", "常");
        fs.SetVariable("p_0", "???");
        fs.SetVariable("o_0", "奇怪的生物");
        fs.SetVariable("o", "OS");

        Commad();
    }

    private void buttonreset()
    {
        NPC2 = GameObject.Find(_NPC2);
        Debug.Log("物件名稱:" + NPC2);
        CharacterEvents.characterText.Invoke(NPC2, "購買成功");
        fs.Resume();
        fs.RemoveButtonGroup();
        fs.SetupButtonGroup();
    }

    public void RemoveButtonGroup()
    {
        fs.Next();

    }

    void Update()
    {
        if (!IsFsStop)
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
            {
                fs.Next();
            }
        }

    }
    public void RemoveDialog_C()
    {
        fs.RemoveDialog();
    }
    public void Dramtic_emotion_1()
    {
        fs.RemoveDialog();
        fs.SetupDialog();
        fs.ReadTextFromResource("npc_1");
    }

    public void Dramtic_emotion_2()
    {
        fs.RemoveDialog();
        fs.SetupDialog();
        fs.ReadTextFromResource("npc_2");
    }
    public void Dramtic_emotion_3()
    {
        fs.RemoveDialog();
        fs.SetupDialog();
        fs.ReadTextFromResource("npc_3");
    }
    public void Commad()
    {
        //開啟與關閉玩家鎖定
        fs.RegisterCommand("lock_player", (List<string> _params) => {
            Time.timeScale = 0.5f;
            PlayerController.lockplay = true;
            isDialog = true;
        });
        fs.RegisterCommand("unlock_player", (List<string> _params) => {
            Time.timeScale = 1f;
            PlayerController.lockplay = false;
            isDialog = false;
        });

        //開啟與關閉NPC物件
        fs.RegisterCommand("showNPC1", (List<string> _params) => {
            //搜尋物件時必須先讓物件可見，才可以搜尋的到物件，這樣才能重新更新畫面
            NPC1.SetActive(true);
            NPC1 = GameObject.Find(_NPC1);
            NPC1.SetActive(false);
            if (NPC1 != null)
            {
                NPC1.SetActive(true);
            }
            else
            {
                Debug.LogWarning("錯誤");
            }
        });
        fs.RegisterCommand("hideNPC1", (List<string> _params) => {
            NPC1 = GameObject.Find(_NPC1);
            NPC1.SetActive(false);
        });
        // 離開對話框
        fs.RegisterCommand("button_st_show", (List<string> _params) =>
        {
            fs.RemoveButtonGroup();
            fs.SetupButtonGroup("ButtonGroup_st");
            fs.SetupButton("", () =>
            {
            }, true, "DefaultButtonPrefab 1");
            fs.SetupButton("SKIP", () =>
            {

                Time.timeScale = 1f;
                PlayerController.lockplay = false;


                fs.ReadTextFromResource("hide");
                fs.RemoveButtonGroup();
                //因為無法調用此函數，所以暫時關閉
                //Invoke("RemoveButtonGroup", 0.4f);
                //fs.RemoveDialog();

            }, false);
        });
        fs.RegisterCommand("button_st_hide", (List<string> _params) =>
        {
            fs.RemoveButtonGroup();
        });
        // 選擇
        fs.RegisterCommand("choose1", (List<string> _params) => {
            fs.RemoveButtonGroup();
            fs.SetupButtonGroup();
            fs.Stop();
            fs.SetupButton("", () =>
            {
            }, true, "DefaultButtonPrefab 1");
            fs.SetupButton("我要成為魔法少女", () => {
                fs.ReadTextFromResource("start1_2");
                fs.Resume();
                fs.RemoveButtonGroup();
                fs.SetupButtonGroup();

            });
            fs.SetupButton("我不想成為魔法少女", () => {
                fs.ReadTextFromResource("start1_1");
                fs.Resume();
                fs.RemoveButtonGroup();
                fs.SetupButtonGroup();
            });
        });

        fs.RegisterCommand("shop", (List<string> _params) => {
            fs.RemoveButtonGroup();
            fs.SetupButtonGroup();
            fs.Stop();
            fs.SetupButton("藥水(小)", () => {
                invventoryManger.AddNewItem(thisItem1);
                buttonreset();
            });
            fs.SetupButton("藥水(中)", () => {
                invventoryManger.AddNewItem(thisItem2);
                buttonreset();
            });
            fs.SetupButton("取消購買", () => {
                fs.Resume();
                fs.RemoveButtonGroup();
                fs.SetupButtonGroup();
            }, true);
        });
        // 很少情況會用到
        fs.RegisterCommand("stop_on", (List<string> _params) => {
            fs.Stop();
            IsFsStop = true;
        });
        fs.RegisterCommand("image_1_show", (List<string> _params) => {
            character_image.Instance.ImageActive(true);
            character_image.Instance.AddImage("player_idle", 20, 20, 190, -220);
        });
        fs.RegisterCommand("image_1_hide", (List<string> _params) => {
            character_image.Instance.ImageActive(false);
        });
        fs.RegisterCommand("image_1_offset", (List<string> _params) => {
            character_image.Instance.Offset(0.5f, 100f, 0f);
        });
        fs.RegisterCommand("FadeIn", (List<string> _params) => {
            //character_image.Instance.fade(0.5f, true);
            //character_image.Instance.StartCoroutine(character_image.Instance.color_A(0.5f, true));
            //將IEnumerator放入列隊中
            character_image.Instance.queue.Enqueue(character_image.Instance.color_A(0.5f, true));
            character_image.Instance.queue.Enqueue(character_image.Instance.color_A(0.3f, false));
            character_image.Instance.queue.Enqueue(character_image.Instance.color_A(0.4f, true));
            character_image.Instance.queue.Enqueue(character_image.Instance.color_A(0.2f, false));
            character_image.Instance.queue.Enqueue(character_image.Instance.color_A(0.3f, true));
            //然後再執行協程
            character_image.Instance.StartCoroutine(character_image.Instance.QueueExecute());
        });
        fs.RegisterCommand("FadeOut", (List<string> _params) => {
            character_image.Instance.fade(0.5f, false);

        });
        fs.RegisterCommand("Zoom", (List<string> _params) => {
            //character_image.Instance.Zoom(0.5f, 1.2f, 1.2f);
            character_image.Instance.zoom_num(11);
        });

    }
}