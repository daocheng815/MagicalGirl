using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Events;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class buffEffect : MonoBehaviour
{
    //����}���A�B�ڧƱ�Buff�ĪG���B�z�Ȧ��b�o�Ӹ}����
    
    [SerializeField]private Damageable damageable;
    [SerializeField]private PMagic pMagic;
    [SerializeField]private ShuDyeingVar shuDyeingVar;
    [SerializeField]private attack[] attacks;
    [SerializeField]private bool isDamageable;
    [SerializeField]private bool isShuDyeingVar;
    [SerializeField]private bool isAttacks;
    [SerializeField]private bool isPMagic;
    
    //�O����
    private Coroutine _buffCoroutine;
    
    // buff����
    private int _buffNum;
    /// <summary>
    /// Buff�C��
    /// </summary>
    public List<Buff> buffList = new List<Buff>();
    /// <summary>
    /// �O�_�b�ĪG�ɶ�
    /// </summary>
    public bool[] isBuffTime;
    /// <summary>
    /// ��e�ĪG�ɶ�
    /// </summary>
    public float[] buffTime;
    /// <summary>
    /// �ĪG�|�[����
    /// </summary>
    public int[] buffOverlayNum;
    /// <summary>
    /// �O�_�b�N�o�ɶ�
    /// </summary>
    public bool[] isBuffCooling;
    /// <summary>
    /// ��e�N�o�ɶ�
    /// </summary>
    public float[] buffCoolingTime;
    /// <summary>
    /// ��s���
    /// </summary>
    public bool isUpData = true;
    [Range(0,10)]
    public int buffID;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        if (damageable != null)
            isDamageable = true;
        pMagic = GetComponent<PMagic>();
        if (pMagic != null)
            isPMagic = true;
        shuDyeingVar = GetComponent<ShuDyeingVar>();
        if (shuDyeingVar != null)
            isShuDyeingVar = true;
        attacks = gameObject.GetComponentsInChildren<attack>();
        if (attacks != null)
            isAttacks = true;
    }

    private void Start()
    {
        BuffUpData();
    }
    
    /// <summary>
    /// �}�l�p�ɮĪG�ɶ�
    /// </summary>
    /// <param name="buff">buff</param>
    public void StartBuffTime(Buff buff)
    {
        int buffID = buff.buffID;
        if (!isBuffTime[buffID])
        {
            isBuffTime[buffID] = true;
            //Ĳ�oBuff�p�ɾ�
            _buffCoroutine = StartCoroutine(Timer(buff.buffTime));
            //�p�G��e����O���a���ܴNĲ�oUI���ʧ@
            TriggerBuff(buff);
            UiAndMessage("���["+buff.buffNameNbt+buff.buffType+"�ĪG", buff);
        }//�P�w�p�GBUFF�i�H�|�[
        else if(buff.buffOverlay)
        {
            
             TriggerBuff(buff);
            UiAndMessage("���[" + buff.buffNameNbt + buff.buffType + "�ĪG�A" + "�ثe�h��" + buffOverlayNum[buffID] + "�h", buff);
        }
        IEnumerator Timer(float time)
        {
            Debug.Log("T�}�l");
            buffTime[buffID] = 0f;
            while (buffTime[buffID] < time)
            {
                buffTime[buffID] += Time.deltaTime;
                yield return null;
            }
            isBuffTime[buffID] = false;
            buffTime[buffID] = 0f;
            buffOverlayNum[buffID] = 0;
            BuffEvents.DelBuff.Invoke(gameObject, buffID);
            Debug.Log("T����");
        }
    }
    //Ĳ�oBUFF���aUI�P�T��
    private void UiAndMessage(string str,Buff buff)
    {
        if (gameObject.CompareTag("Player"))
        {
            BuffEvents.PlayerCreateBuffPrefab(buff,gameObject);
            GameMessageEvents.AddMessage(str, 3f);
        }
    }
    /// <summary>
    /// �C��^��
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="spacing"></param>
    /// <param name="healNum"></param>
    /// <returns></returns>
    private IEnumerator Heal(Buff buff,float spacing,int healNum)
    {
        var timer = 0f;
        var spacingTimer = 0f;
        var isOverlayNum = buffOverlayNum[buff.buffID];
        while (timer < buff.buffTime && isBuffTime[buff.buffID] && isOverlayNum == buffOverlayNum[buff.buffID])
        {
            timer += Time.deltaTime;
            spacingTimer += Time.deltaTime;
            yield return null;
            if (spacingTimer >= spacing)
            {
                damageable.Heal(healNum);
                spacingTimer = 0f;
            }
        }
    }
    /// <summary>
    /// �C��^�_���V��
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="spacing"></param>
    /// <param name="dyeingNum"></param>
    /// <returns></returns>
    private IEnumerator DyeingFilthySub(Buff buff,float spacing,int dyeingNum)
    {
        var timer = 0f;
        var spacingTimer = 0f;
        var isOverlayNum = buffOverlayNum[buff.buffID];
        while (timer < buff.buffTime && isBuffTime[buff.buffID] && isOverlayNum == buffOverlayNum[buff.buffID])
        {
            timer += Time.deltaTime;
            spacingTimer += Time.deltaTime;
            yield return null;
            if (spacingTimer >= spacing)
            {
                shuDyeingVar.FilthySub(dyeingNum);
                spacingTimer = 0f;
            }
        }
    }
    
    //Ĳ�o�ĪG��ܾ�
    private void TriggerBuff(Buff buff)
    {
        switch (buff.buffID)
        {
            case 0:
            {
                break;
            }
            case 1:
            {
                break;
            }
            case 2:
            {
                switch (buffOverlayNum[buff.buffID])
                {
                    case 1:
                    {
                        StartCoroutine(Heal(buff, 1f,1));
                        break;
                    }
                    case 2:
                    {
                        StartCoroutine(Heal(buff, 1f,1));
                        StartCoroutine(DyeingFilthySub(buff, 1f,1));
                        break;
                    }
                    case 3:
                    {
                        StartCoroutine(Heal(buff, 1f,2));
                        StartCoroutine(DyeingFilthySub(buff, 1f,1));
                        break;
                    }
                    case 4:
                    {
                        StartCoroutine(Heal(buff, 1f,2));
                        StartCoroutine(DyeingFilthySub(buff, 1f,2));
                        break;
                    }
                    case 5:
                    {
                        StartCoroutine(Heal(buff, 1f,3));
                        StartCoroutine(DyeingFilthySub(buff, 1f,2));
                        break;
                    }
                }
                break;
            }
            case 3:
            {
                break;
            }
        }
        
    }
    /// <summary>
    /// ��s�ۨ����
    /// </summary>
    private void BuffUpData()
    {
        isUpData = true;
        _buffNum = BuffManger.Instance.allBuffList.buffs.Count;
        if (_buffNum != 0)
        {
            isBuffTime = new bool[_buffNum];
            buffTime = new float[_buffNum];
            isBuffCooling = new bool[_buffNum];
            buffCoolingTime = new float[_buffNum];
            buffOverlayNum = new int[_buffNum];
        }
        isUpData = false;
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(buffEffect))]
public class buffEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        buffEffect be = (buffEffect)target;
        if (GUILayout.Button("�s�WBuff"))
        {
            BuffEvents.AddBuff.Invoke(be.gameObject,be.buffID);
        }
    }
}    
#endif
