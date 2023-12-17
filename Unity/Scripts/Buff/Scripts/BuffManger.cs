using Events;
using JetBrains.Annotations;
using UnityEngine;

public class BuffManger : Singleton<BuffManger>
{
    public GameObject player;
    public BuffList allBuffList;
    public Buff[] buffs;
    private void OnEnable()
    {
        BuffEvents.AddBuff += AddBuff;
        BuffEvents.DelBuff += DelBuff;
    }

    private void OnDisable()
    {
        BuffEvents.AddBuff -= AddBuff;
        BuffEvents.DelBuff -= DelBuff;
    }

    protected override void Awake()
    {
        base.Awake();
        UpDataAllBuffList();
    }
    //�o�̦��@�����~�A�N�O�p�G��ƹL���e�j�A�]�\��l�Ʈɷ|���Ϳ��~
    /// <summary>
    /// ��s��lBuff�C��
    /// </summary>
    private void UpDataAllBuffList()
    {
        allBuffList.buffs.Clear();
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i].buffID = i;
            allBuffList.buffs.Add(buffs[i]);
        }
    }
    /// <summary>
    /// �P�w�ثe�OBuff�_�٥i�H�|�[�ϥ�
    /// </summary>
    /// <param name="obj0"></param>
    /// <param name="buffID"></param>
    /// <returns></returns>
    public bool ExamineBuffOverlay(GameObject obj0, int buffID)
    {
        bool index = false;
        buffEffect obj = obj0.GetComponent<buffEffect>();
        if (obj != null && !obj.isUpData)
        {
            //�]�mbuffID���̤p��
            buffID = Mathf.Clamp(buffID, 0, buffs.Length - 1);
            Buff buff = allBuffList.buffs[buffID];
            //�P�w�ثe�OBuff�_�٥i�H�|�[�ϥΡA�i�H�OTrue
            if (buff.buffOverlay)
            {
                index = obj.buffOverlayNum[buffID] != buff.maxBuffOverlayNum;
            }
            else
            {
                index = false;
            }
        }
        else
        {
            index = false;
        }
        return index;
    }
    /// <summary>
    /// �ˬd�Y���󨭤W�O�_���Y��BUFF�A�p�G���󨭤W�S��buffEffect�]�O�^��False
    /// </summary>
    /// <param name="obj0"></param>
    /// <param name="buffID"></param>
    public bool ExamineBuff(GameObject obj0, int buffID)
    {
        bool index = false;
        buffEffect obj = obj0.GetComponent<buffEffect>();
        if (obj != null && !obj.isUpData)
        {
            //�]�mbuffID���̤p��
            buffID = Mathf.Clamp(buffID, 0, buffs.Length - 1);
            Buff buff = allBuffList.buffs[buffID];
            //�ˬdBUFF�C���O�_�s�b�ĪG�F
            index = obj.buffList.Contains(buff);
            
            if(buff.buffOverlay)
                index = false;
        }
        else
        {
            index = false;
        }
        return index;
    }
    /// <summary>
    /// �s�WBuff�ĪG
    /// </summary>
    /// <param name="obj0"></param>
    /// <param name="buffID"></param>
    public void AddBuff(GameObject obj0,int buffID)
    {
        buffEffect obj = obj0.GetComponent<buffEffect>();
        if (obj != null && !obj.isUpData)
        {
            //�]�mbuffID���̤p��
            buffID = Mathf.Clamp(buffID, 0, buffs.Length - 1);
            Buff buff = allBuffList.buffs[buffID];
            //�ˬdBUFF�C���O�_�s�b�ĪG�F
            bool contains = obj.buffList.Contains(buff);
            //�p�G���s�b�A�B���i�|�[��
            if (!contains && !buff.buffOverlay)
            {
                Debug.Log("�s�WBuff");
                //�NBUFF�[�J�M��
                obj.buffList.Add(buff);
                //�}�lBUFF�p��
                obj.StartBuffTime(buff);
            }
            //�p�G�i�|�[�A���|����(���βz�O�_�s�b��C��F)
            if (!buff.buffOverlay) return;
            if (obj.buffOverlayNum[buffID] >= buff.maxBuffOverlayNum) return;
            
            //���s�ĪG�ɶ�
            obj.buffTime[buffID] = 0f;
            //��l�Ƽh��
            if (obj.buffOverlayNum[buffID] == 0)
                obj.buffOverlayNum[buffID] = 1;
            //�p�G�s�b�o�ӮĪG�A�N�W�[�h��
            if (contains)
            {
                obj.buffOverlayNum[buffID] += 1;
                //�����즳�ĪG
                DelBuff(obj0, buffID);
            }
            Debug.Log("�s�WBuff");
            obj.buffList.Add(buff);
            obj.StartBuffTime(buff);
        }
        else
        {
            Debug.Log("��H"+obj0+"�S��buff���A�L�k�K�[BUFF");
        }
    }

    /// <summary>
    /// ����Buff�ĪG
    /// </summary>
    /// <param name="obj0"></param>
    /// <param name="buffID"></param>
    public void DelBuff(GameObject obj0, int buffID)
    {
        buffEffect obj = obj0.GetComponent<buffEffect>();
        if (obj != null)
        {
            //�j�MbuffList�M�椺�O�_����buff
            for (int i = 0; i < obj.buffList.Count; i++)
            {
                Buff buff = obj.buffList[i];
                if (buff.buffID == buffID)
                {
                    //�R�����w���ަ�m�������A�᭱�����e����
                    obj.buffList.RemoveAt(i);
                }
            }
        }
        
    }
}
