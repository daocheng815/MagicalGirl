using System;
using Flower;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
// Invoke("RemoveButtonGroup", 0.4f);這一段在之後必須要修改
public class AVGSystem : MonoBehaviour
{
    public bool isOn = true;
    FlowerSystem fs;

    public GameObject player;
    private Damageable playDamageable;
    private TouchingDirections TouchingDirections;
    
    // 任何物件都必須初始化這樣在重新開始遊戲時才不會產生尋找不到物件的情況
    public GameObject NPC1;
    private string _NPC1;
    public GameObject NPC2;
    private string _NPC2;
    //判定是否暫停系統
    bool IsFsStop = false;

    public bool isDialog;

    private FilleManager FilleManager;

    public bool sairesuu;

    // Start is called before the first frame update
    private void Awake()
    {
        playDamageable = player.GetComponent<Damageable>();
        TouchingDirections = player.GetComponent<TouchingDirections>();
        FilleManager = player.GetComponent<FilleManager>();
    }

    private void OnEnable()
    {
        AvgEvents.StartScript += StartScript;
        AvgEvents.Sairesuuss += Sairesuuss;
    }

    private void OnDisable()
    {
        fs.RemoveDialog();
        AvgEvents.StartScript -= StartScript;
        AvgEvents.Sairesuuss -= Sairesuuss;
        
    }
    private void Start()
    {
        NPC1 = GameObject.Find("QB");
        _NPC1 = NPC1.name;
        NPC1.SetActive(false);
        NPC2 = GameObject.Find("NPC1_0");
        _NPC2 = NPC2.name;
        
        //呼叫對話系統
        
        fs = FlowerManager.Instance.CreateFlowerSystem("FlowerSample", false);
        if(isOn)
            fs.SetupDialog();
        fs.Stop();
        fs.Resume();
        fs.SetupDialog("DefaultDialogPrefab1", true);
        fs.fs.SteUpUiImage();
        if (Persistence.GameLoadNum == 0)
        {
            if (isOn)
            {
                fs.RemoveDialog();
                fs.SetupDialog("DefaultDialogPrefab1", true);
                fs.ReadTextFromResource("Open_1");
                //fs.ReadTextFromResource("start1");
            }
            else
            {
                fs.RemoveDialog();
                fs.SetupDialog();
                fs.ReadTextFromResource("NewStart1");
                
                //fs.ReadTextFromResource("start1");//舊版
                //fs.ReadTextFromResource("hide");
            }
        }
        else
        {
            fs.ReadTextFromResource("hide");
        }
        
        fs.SetVariable("p", "<color=#EA4B4E><size=30><b>常</b></size></color>");
        fs.SetVariable("p_0", "???");
        fs.SetVariable("o_0", "奇怪的生物");
        fs.SetVariable("o", "<color=#7FCFFF><size=28><b>觀測者</b></size></color>");
        fs.SetVariable("s_0","<color=#BA67CF><size=30><b>???</b></size></color>");
        fs.SetVariable("s","<color=#BA67CF><size=30><b>賽蕾絲</b></size></color>");

        Commad();
    }
    private void buttonreset(bool index)
    {
        NPC2 = GameObject.Find(_NPC2);
        Debug.Log("物件名稱:" + NPC2);
        if(index)
            CharacterEvents.characterText.Invoke(NPC2, "購買成功");
        else
            CharacterEvents.characterText.Invoke(NPC2, "購買失敗");
        fs.Resume();
        fs.RemoveButtonGroup();
        fs.SetupButtonGroup();
    }

    //public void RemoveButtonGroup()
    //{
    //    fs.Next();

    //}

    private void Update()
    {
        if (!IsFsStop)
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
            {
                //Debug.Log($"{fs.CurrentTextListIndex}");
                fs.Next();
            }
            if (Input.GetKeyDown(KeyCode.RightControl)||Input.GetKeyDown(KeyCode.LeftControl))
            {
                fs.textSpeed = GameSettings.Instance.TextSpeed;
            }
            if (Input.GetKeyUp(KeyCode.RightControl)||Input.GetKeyUp(KeyCode.LeftControl))
            {
                fs.textSpeed = 0.01f;
            }
            if (Input.GetKey(KeyCode.RightControl)||Input.GetKey(KeyCode.LeftControl))
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
    public void Dramtic_emotion_4()
    {
        fs.RemoveDialog();
        fs.SetupDialog();
        fs.ReadTextFromResource("npc_4");
    }
    public void OnDramticEmotion(string emotion)
    {
        fs.RemoveDialog();
        fs.SetupDialog();
        fs.ReadTextFromResource(emotion);
    }
    public void StartScript(string emotion,int idnex = 0)
    {
        fs.RemoveDialog();
        fs.SetupDialog();
        fs.ReadTextFromResource(emotion,idnex);
    }

    private void Sairesuuss()
    {
        sairesuu = true;
        Debug.Log($"劇情結束 sairesuu = {sairesuu}");
    }
    private void Commad()
    {
        fs.RegisterCommand("sairesuuEnd", (List<string> _params) =>
        {
            Debug.Log($"劇情結束 sairesuu = {sairesuu}");
            sairesuu = true;
            AvgEvents.Sairesuuss.Invoke();
            Debug.Log($"劇情結束 sairesuu = {sairesuu}");
            
        });
        fs.RegisterCommand("start1", (List<string> _params) => {
            fs.RemoveDialog();
            fs.SetupDialog();
            fs.ReadTextFromResource("NewStart1");
        });
        //開啟與關閉玩家鎖定
        fs.RegisterCommand("lock_player", (List<string> _params) => {
            isDialog = true;
            Time.timeScale = 0.5f;
            //PlayerController.lockplay = true;
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
                        //DebugTask.Log("落地");
                        timer = timedelay;
                    }
                    yield return null;
                }
            }
        });
        fs.RegisterCommand("unlock_player", (List<string> _params) => {
            Time.timeScale = 1f;
            PlayerController.lockplay = false;
            isDialog = false;
        });

        //開啟與關閉NPC物件
        fs.RegisterCommand("showNPC1", (List<string> _params) => {
            //搜尋物件時必須先讓物件可見，才可以搜尋的到物件，這樣才能重新更新畫面
            //NPC1.SetActive(true);
            //NPC1 = GameObject.Find(_NPC1);
            //NPC1.SetActive(false);
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
        fs.RegisterCommand("OnUiShowAndHide", (List<string> _params) => {
            CurrentLevel.Instance.OnUiShowAndHide(3.5f);
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
                PlayerController.lockplay = false;
                Time.timeScale = 1f;
                //Debug.Log($"剩餘自數量 : {fs.CurrentTextListIndex}");
                //CurrentLevel.Instance.OnUiShowAndHide(3.5f);
                fs.ReadTextFromResource("hide");
                //Debug.Log($"剩餘自數量 : {fs.CurrentTextListIndex}");
                
                
                fs.NextDelay(0.03f);
                fs.RemoveButtonGroup();
                
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
            fs.SetupButton("回復藥水(回復量:65)需要2個破碎靈魂", () =>
            {
                if (invventoryManger.Instance.ItemExistenceCheckerAllBagNumDel(2, 2))
                {
                    bool index = invventoryManger.Instance.AddNewItem(6,0,2);
                    buttonreset(index);
                }
                else
                {
                    GameMessageEvents.AddMessage("破碎靈魂不夠，請戴上足夠的靈魂碎片", 3f);
                }
            });
            fs.SetupButton("回復藥水(回復量:155)需要4個破碎靈魂", () => {
                if (invventoryManger.Instance.ItemExistenceCheckerAllBagNumDel(2, 4))
                {
                    bool index = invventoryManger.Instance.AddNewItem(9,0,2);
                    buttonreset(index);
                }
                else
                {
                    GameMessageEvents.AddMessage("破碎靈魂不夠，請戴上足夠的靈魂碎片", 3f);
                }
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
        fs.RegisterCommand("image_B_show", (List<string> _params) => {
            character_image.Instance.ImageActive(true,1);
            character_image.Instance.AddImage("black",1,1, 1, 1, 0, 0);
            
        });
        fs.RegisterCommand("image_B_hide", (List<string> _params) => {
            character_image.Instance.ImageActive(false,1);
        });
        fs.RegisterCommand("image_B_FadeOut", (List<string> _params) => {
            character_image.Instance.Fade(0,0.5f, false);
        });
        fs.RegisterCommand("image_B_FadeIn", (List<string> _params) => {
            character_image.Instance.Fade(0,0.5f, true);
        });
        fs.RegisterCommand("image_1_show", (List<string> _params) => {
            character_image.Instance.ImageActive(true,0);
            character_image.Instance.AddImage("player_idle",0,0, 1, 1, 190, -220);
        });
        fs.RegisterCommand("image_1_hide", (List<string> _params) => {
            character_image.Instance.ImageActive(false,0);
        });
        fs.RegisterCommand("image_1_offset", (List<string> _params) => {
            character_image.Instance.Offset(0,0.5f, 100f, 0f);
        });
        fs.RegisterCommand("FadeIn", (List<string> _params) => {
            //character_image.Instance.fade(0.5f, true);
            //character_image.Instance.StartCoroutine(character_image.Instance.color_A(0.5f, true));
            //將IEnumerator放入列隊中
            character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.5f, true));
            //character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.3f, false));
            //character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.4f, true));
            //character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.2f, false));
            //character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.3f, true));
            //然後再執行協程
            character_image.Instance.StartCoroutine(character_image.Instance.QueueExecute());
        });
        fs.RegisterCommand("FadeOut", (List<string> _params) => {
            character_image.Instance.Fade(0,0.5f, false);
        });
        fs.RegisterCommand("Zoom", (List<string> _params) => {
            //character_image.Instance.Zoom(0.5f, 1.2f, 1.2f);
            character_image.Instance.zoom_num(11);
        });

    }

    private bool isNext;
    private IEnumerator IsNext(float delayTime)
    {
        isNext = true;
        yield return new WaitForSeconds(delayTime);
        fs.Next(); 
        fs.textSpeed = 0.01f;
        isNext = false;
    }
}