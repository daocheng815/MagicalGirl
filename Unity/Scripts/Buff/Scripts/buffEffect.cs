using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class buffEffect : MonoBehaviour
{
    
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
            StartCoroutine(Timer(buff.buffTime));
            //�p�G��e����O���a���ܴNĲ�oUI���ʧ@
            if (gameObject.CompareTag("Player"))
            {
                BuffEvents.PlayerCreateBuffPrefab(buff,gameObject);
            }
        }//�P�w�p�GBUFF�i�H�|�[
        else if(buff.buffOverlay)
        {
            if (gameObject.CompareTag("Player"))
            {
                BuffEvents.PlayerCreateBuffPrefab(buff,gameObject);
            }
        }
        IEnumerator Timer(float time)
        {
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
        }
    }
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

    private void Buff001()
    {
        
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
