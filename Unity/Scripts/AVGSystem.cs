using System;
using Flower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Invoke("RemoveButtonGroup", 0.4f);�o�@�q�b���ᥲ���n�ק�
public class AVGSystem : MonoBehaviour
{
    FlowerSystem fs;

    public GameObject player;
    private Damageable playDamageable;
    private TouchingDirections TouchingDirections;
    
    // ���󪫥󳣥�����l�Ƴo�˦b���s�}�l�C���ɤ~���|���ʹM�䤣�쪫�󪺱��p
    public GameObject NPC1;
    private string _NPC1;
    public GameObject NPC2;
    private string _NPC2;
    //�P�w�O�_�Ȱ��t��
    bool IsFsStop = false;
    public item thisItem1;
    public item thisItem2;

    public bool isDialog;

    private FilleManager FilleManager;

    // Start is called before the first frame update
    private void Awake()
    {
        playDamageable = player.GetComponent<Damageable>();
        TouchingDirections = player.GetComponent<TouchingDirections>();
        FilleManager = player.GetComponent<FilleManager>();
    }

    void Start()
    {
        NPC1 = GameObject.Find("QB");
        _NPC1 = NPC1.name;
        NPC1.SetActive(false);
        NPC2 = GameObject.Find("NPC1_0");
        _NPC2 = NPC2.name;

        //�I�s��ܨt��
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

        fs.SetVariable("p", "�`");
        fs.SetVariable("p_0", "???");
        fs.SetVariable("o_0", "�_�Ǫ��ͪ�");
        fs.SetVariable("o", "OS");

        Commad();
    }
    private void buttonreset()
    {
        NPC2 = GameObject.Find(_NPC2);
        Debug.Log("����W��:" + NPC2);
        CharacterEvents.characterText.Invoke(NPC2, "�ʶR���\");
        fs.Resume();
        fs.RemoveButtonGroup();
        fs.SetupButtonGroup();
    }

    //public void RemoveButtonGroup()
    //{
    //    fs.Next();

    //}

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
        //�}�һP�������a��w
        fs.RegisterCommand("lock_player", (List<string> _params) => {
            isDialog = true;
            Time.timeScale = 0.5f;
            //PlayerController.lockplay = true;
            // �H�U�����a���~�}�l����a
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
                        //Debug.Log("���a");
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

        //�}�һP����NPC����
        fs.RegisterCommand("showNPC1", (List<string> _params) => {
            //�j�M����ɥ�����������i���A�~�i�H�j�M���쪫��A�o�ˤ~�୫�s��s�e��
            NPC1.SetActive(true);
            NPC1 = GameObject.Find(_NPC1);
            NPC1.SetActive(false);
            if (NPC1 != null)
            {
                NPC1.SetActive(true);
            }
            else
            {
                Debug.LogWarning("���~");
            }
        });
        fs.RegisterCommand("hideNPC1", (List<string> _params) => {
            NPC1 = GameObject.Find(_NPC1);
            NPC1.SetActive(false);
        });

        // ���}��ܮ�
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
                //�]���L�k�եΦ���ơA�ҥH�Ȯ�����
                //Invoke("RemoveButtonGroup", 0.4f);
                //fs.RemoveDialog();

            }, false);
        });
        fs.RegisterCommand("button_st_hide", (List<string> _params) =>
        {
            fs.RemoveButtonGroup();
        });
        // ���
        fs.RegisterCommand("choose1", (List<string> _params) => {
            fs.RemoveButtonGroup();
            fs.SetupButtonGroup();
            fs.Stop();
            fs.SetupButton("", () =>
            {
            }, true, "DefaultButtonPrefab 1");
            fs.SetupButton("�ڭn�����]�k�֤k", () => {
                fs.ReadTextFromResource("start1_2");
                fs.Resume();
                fs.RemoveButtonGroup();
                fs.SetupButtonGroup();

            });
            fs.SetupButton("�ڤ��Q�����]�k�֤k", () => {
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
            fs.SetupButton("�Ĥ�(�p)", () => {
                invventoryManger.AddNewItem(thisItem1);
                buttonreset();
            });
            fs.SetupButton("�Ĥ�(��)", () => {
                invventoryManger.AddNewItem(thisItem2);
                buttonreset();
            });
            fs.SetupButton("�����ʶR", () => {
                fs.Resume();
                fs.RemoveButtonGroup();
                fs.SetupButtonGroup();
            }, true);
        });
        // �ܤֱ��p�|�Ψ�
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
            //�NIEnumerator��J�C����
            character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0.5f, true));
            character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0.3f, false));
            character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0.4f, true));
            character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0.2f, false));
            character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0.3f, true));
            //�M��A�����{
            character_image.Instance.StartCoroutine(character_image.Instance.QueueExecute());
        });
        fs.RegisterCommand("FadeOut", (List<string> _params) => {
            character_image.Instance.Fade(0.5f, false);

        });
        fs.RegisterCommand("Zoom", (List<string> _params) => {
            //character_image.Instance.Zoom(0.5f, 1.2f, 1.2f);
            character_image.Instance.zoom_num(11);
        });

    }
}