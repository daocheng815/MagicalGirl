using System;
using Flower;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
// Invoke("RemoveButtonGroup", 0.4f);�o�@�q�b���ᥲ���n�ק�
public class AVGSystem : MonoBehaviour
{
    public bool isOn = true;
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
        
        //�I�s��ܨt��
        
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
                
                //fs.ReadTextFromResource("start1");//�ª�
                //fs.ReadTextFromResource("hide");
            }
        }
        else
        {
            fs.ReadTextFromResource("hide");
        }
        
        fs.SetVariable("p", "<color=#EA4B4E><size=30><b>�`</b></size></color>");
        fs.SetVariable("p_0", "???");
        fs.SetVariable("o_0", "�_�Ǫ��ͪ�");
        fs.SetVariable("o", "<color=#7FCFFF><size=28><b>�[����</b></size></color>");
        fs.SetVariable("s_0","<color=#BA67CF><size=30><b>???</b></size></color>");
        fs.SetVariable("s","<color=#BA67CF><size=30><b>������</b></size></color>");

        Commad();
    }
    private void buttonreset(bool index)
    {
        NPC2 = GameObject.Find(_NPC2);
        Debug.Log("����W��:" + NPC2);
        if(index)
            CharacterEvents.characterText.Invoke(NPC2, "�ʶR���\");
        else
            CharacterEvents.characterText.Invoke(NPC2, "�ʶR����");
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
        Debug.Log($"�@������ sairesuu = {sairesuu}");
    }
    private void Commad()
    {
        fs.RegisterCommand("sairesuuEnd", (List<string> _params) =>
        {
            Debug.Log($"�@������ sairesuu = {sairesuu}");
            sairesuu = true;
            AvgEvents.Sairesuuss.Invoke();
            Debug.Log($"�@������ sairesuu = {sairesuu}");
            
        });
        fs.RegisterCommand("start1", (List<string> _params) => {
            fs.RemoveDialog();
            fs.SetupDialog();
            fs.ReadTextFromResource("NewStart1");
        });
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
                        //DebugTask.Log("���a");
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
            //NPC1.SetActive(true);
            //NPC1 = GameObject.Find(_NPC1);
            //NPC1.SetActive(false);
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
        fs.RegisterCommand("OnUiShowAndHide", (List<string> _params) => {
            CurrentLevel.Instance.OnUiShowAndHide(3.5f);
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
                PlayerController.lockplay = false;
                Time.timeScale = 1f;
                //Debug.Log($"�Ѿl�ۼƶq : {fs.CurrentTextListIndex}");
                //CurrentLevel.Instance.OnUiShowAndHide(3.5f);
                fs.ReadTextFromResource("hide");
                //Debug.Log($"�Ѿl�ۼƶq : {fs.CurrentTextListIndex}");
                
                
                fs.NextDelay(0.03f);
                fs.RemoveButtonGroup();
                
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
            fs.SetupButton("�^�_�Ĥ�(�^�_�q:65)�ݭn2�ӯ}�H�F��", () =>
            {
                if (invventoryManger.Instance.ItemExistenceCheckerAllBagNumDel(2, 2))
                {
                    bool index = invventoryManger.Instance.AddNewItem(6,0,2);
                    buttonreset(index);
                }
                else
                {
                    GameMessageEvents.AddMessage("�}�H�F����A�����W�������F��H��", 3f);
                }
            });
            fs.SetupButton("�^�_�Ĥ�(�^�_�q:155)�ݭn4�ӯ}�H�F��", () => {
                if (invventoryManger.Instance.ItemExistenceCheckerAllBagNumDel(2, 4))
                {
                    bool index = invventoryManger.Instance.AddNewItem(9,0,2);
                    buttonreset(index);
                }
                else
                {
                    GameMessageEvents.AddMessage("�}�H�F����A�����W�������F��H��", 3f);
                }
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
            //�NIEnumerator��J�C����
            character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.5f, true));
            //character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.3f, false));
            //character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.4f, true));
            //character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.2f, false));
            //character_image.Instance.Queue.Enqueue(character_image.Instance.color_A(0,0.3f, true));
            //�M��A�����{
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