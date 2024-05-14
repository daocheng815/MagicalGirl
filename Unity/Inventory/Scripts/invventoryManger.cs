using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using TMPro;
using UnityEngine;
using ItemTypeEnum;

public class invventoryManger : Singleton<invventoryManger>
{
    
    [Header("�I�]")]
    public Inventory[] Bag;
    public GameObject[] slotGrid;
    public GameObject[] emptySlot;
    //public slot slotPrefab;
    //showInfromation
    public TextMeshProUGUI[] itemInfromation;
    
    //public List<GameObject> slots_0 = new List<GameObject>();
    //public List<GameObject> slots_1 = new List<GameObject>();
    //�Q�Φr���x�sList�A�o�O�@�Ӥ���������G���x���A�n�`�N
    public Dictionary<string, List<GameObject>> slotsDictionary = new Dictionary<string, List<GameObject>>()
    {
        { "slots_0",  new List<GameObject>() },
        { "slots_1",  new List<GameObject>() }
    };
    
    private void OnEnable()
    {
        itemInfromation[0].text = "";
        PlayerController.lockplay = true;
    }
    private void OnDisable()
    {
        PlayerController.lockplay = false;
    }

    /// <summary>
    /// ��s���~�y�z
    /// </summary>
    public void UpdateItemInfo(string itemDescription)
    {
        //slotGrid.SetActive(false);
        itemInfromation[0].text = itemDescription;
    }
    /// <summary>
    /// �������������~
    /// </summary>
    public void RemoveAllItem(int bagNum)
    {
        for (int i = 0; i < Bag[bagNum].itemList.Count; i++)
        {
            if(Bag[bagNum].itemList[i] != null)
            {
                Bag[bagNum].itemList[i].itemHeld = 1;
                Bag[bagNum].itemList[i] = null;
            }
        }
    }
    /// <summary>
    /// �s�W���~
    /// </summary>
    public bool AddNewItem(int itemID,int bagNum,int addNum)
    {
        bool index = true;
        item item = ItemList.Instance.Item[itemID];
        if (ItemExistenceCheckerAllBag(itemID))
        {
            item.itemHeld += addNum;
            ItemEvents.ItemDropsTheSuccess(addNum,item);
        }
        else
        {
            int? nullBag = CheckerBagItem(bagNum);
            if (nullBag != null)
            {
                Debug.Log(nullBag);
                Bag[bagNum].itemList[(int)nullBag] = item;
                item.itemHeld += addNum - 1 ;
                ItemEvents.ItemDropsTheSuccess(addNum,item);
            }
            else
            {
                Debug.Log("�I�]���S���i�K�[���Ŧ�A�L�k�K�[���~");
                GameMessageEvents.AddMessage("�I�]���S���i�K�[���Ŧ�A�L�k������~",3f);
                index = false;
            }
        }
        return index;
    }
    /// <summary>
    /// �ϥ�KEY
    /// </summary>
    /// <param name="bagnum"></param>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public bool UsingKey(int bagnum,int itemID)
    {
        var item = ItemList.Instance.Item[itemID];
        if (item.itemType != ItemType.Key)
            return false;
        if (ItemExistenceCheckerBag(bagnum,itemID))
        {
            var i = CheckerItemIsABagToSlotID(bagnum,itemID);
            Deleteitems(i, bagnum);
            return true;
        }
        return false;
    }
    /// <summary>
    /// �������~�C
    /// </summary>
    public void Deleteitems(int slotID,int bagNum)
    {
        if (Bag[bagNum].itemList[slotID].itemHeld > 1)
            Bag[bagNum].itemList[slotID].itemHeld -= 1;
        else
        {
            if(bagNum != 1)
                itemInfromation[bagNum].text = "";
            Bag[bagNum].itemList[slotID] = null;
        }
        RefreshItem(bagNum);
    }

    /// <summary>
    /// �ˬd�Y�ӭI�]�O�_�s�b�Y�Ӫ��~
    /// </summary>
    /// <param name="b"></param>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public bool ItemExistenceCheckerBag(int b ,int itemID)
    {
        bool index = false;
        item item = ItemList.Instance.Item[itemID];
        index = Bag[b].itemList.Contains(item);
        return index;
    }
    /// <summary>
    /// �ˬd�Y�Ӫ��~�b�I�]����SLOT
    /// </summary>
    /// <param name="b"></param>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public int CheckerItemIsABagToSlotID(int b ,int itemID)
    {
        int index = 0;
        for (int i = 0; i < Bag[b].itemList.Count; i++)
        {
            if (Bag[b].itemList[i].itemID == itemID)
            {
                index = i;
                break; 
            }
        }
        return index;
    }
    /// <summary>
    /// �ˬd�Ҧ��I�]�O�_�s�b�Y�Ӫ��~
    /// </summary>
    /// <param name="itemID"></para m>
    /// <returns></returns>
    public bool ItemExistenceCheckerAllBag(int itemID)
    {
        bool index = false;
        item item = ItemList.Instance.Item[itemID];
        foreach (var bag in Bag)
        {
            index = bag.itemList.Contains(item);
            if(index)
                break;
        }
        return index;
    }
    /// <summary>
    /// �ˬd�Ҧ��I�]�O�_�s�b�Y�Ӫ��~�A�B�O�_�����������~�A�p�����N����
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public bool ItemExistenceCheckerAllBagNumDel(int itemID,int num)
    {
        bool index = false;
        if (ItemExistenceCheckerAllBag(itemID))
        {
            if (ItemList.Instance.Item[itemID].itemHeld >= num)
            {
                ItemList.Instance.Item[itemID].itemHeld -= num;
                index = true;
            }
        }
        return index;
    }
    /// <summary>
    /// �ˬd�Y�ӭI�]���x�s��O�_�s�b���~
    /// </summary>
    /// <param name="bagNum">�I�]</param>
    /// <param name="slotID">�x�s��ID</param>
    /// <returns></returns>
    public bool ItemExistenceChecker(int bagNum, int slotID)
    {
            return Bag[bagNum].itemList[slotID] != null;
    }
    /// <summary>
    /// �ˬd�Y�ӭI�]�O�_���Ŧ�A�æ^�ǳ̪񪺪Ŧ�A�Y�O�S���^��null
    /// </summary>
    /// <param name="bagNum"></param>
    /// <returns></returns>
    public int? CheckerBagItem(int bagNum)
    {
        int? index = null;
        //����bagNum�O�_���T
        if (bagNum > 0 || bagNum < Bag.Length)
        {
            index = Bag[bagNum].itemList.FindIndex(item => item == null);
        }
        return index;
    }
    /// <summary>
    /// �B�zBUFF���~ �A������
    /// </summary>
    /// <param name="playerObject">BUFF��H</param>
    /// <param name="slotID">���~�Ҧb���Ѧ� ID�C</param>
    /// <param name="bagNum">�ثe�I�]</param>
    /// <returns></returns>
    public bool Buff(GameObject playerObject, int slotID, int bagNum)
    {
        bool index = false;
        if (!ItemExistenceChecker(bagNum,slotID))
            return false;
        if (Bag[bagNum].itemList[slotID].itemType == ItemType.Buff)
        {
            if (Bag[bagNum].itemList[slotID].available)
            {
                //���󨭤W�O�_���Y��BUFF
                if (BuffManger.Instance.ExamineBuff(playerObject, Bag[bagNum].itemList[slotID].buffID))
                {
                    index = false;
                }
                else
                {
                    //�P�w���~�ثe�O�_�٥i�H�|�[�ϥ�
                    if (BuffManger.Instance.ExamineBuffOverlay(playerObject, Bag[bagNum].itemList[slotID].buffID))
                    {
                        BuffEvents.AddBuff(playerObject, Bag[bagNum].itemList[slotID].buffID);
                        Deleteitems(slotID,bagNum);
                        RefreshItem(bagNum);
                        index = true;
                    }
                    else
                    {
                        index = false;
                        GameMessageEvents.AddMessage("�ĪG�w�|��",3f);
                    }
                    
                }
            }
        }
        return index;
    }
    /// <summary>
    /// �o�Ӥ�k�Ω�B�z�b���Ĥ����ϥΡC
    /// </summary>
    /// <param name="playerObject">�Ĥ��^�_��H�C</param>
    /// <param name="slotID">���~�Ҧb���Ѧ� ID�C</param>
    /// <param name="bagNum">�ثe�I�]</param>
    public bool Purify(GameObject playerObject,int slotID,int bagNum)
    {
        bool index = false;
        if (!ItemExistenceChecker(bagNum,slotID))
            return false;
        if (Bag[bagNum].itemList[slotID].itemType == ItemType.Purify)
        {
            if (Bag[bagNum].itemList[slotID].available)
            {
                if (PlayerVar.Instance.shuDyeingVar.TestFilthyVar(0))
                {
                    Debug.Log("��e�S�����V��");
                    GameMessageEvents.AddMessage("���V�Ȭ��s�A�L���ϥβb�ơC",3f);
                }
                else
                {
                    PlayerVar.Instance.shuDyeingVar.FilthySub(Bag[bagNum].itemList[slotID].purifyNum);
                    Deleteitems(slotID,bagNum);
                    RefreshItem(bagNum);
                    index = true;
                }
            }
        }
        return index;
    }
    
    /// <summary>
    /// <summary>
    /// �o�Ӥ�k�Ω�B�z�Ĥ����ϥΡC
    /// </summary>
    /// <param name="playerObject">�Ĥ��^�_��H�C</param>
    /// <param name="slotID">���~�Ҧb���Ѧ� ID�C</param>
    /// <param name="bag">�ثe�I�]</param>
    public bool Potion(GameObject playerObject,int slotID,int bagNum)
    {
        bool index = false;
        if (!ItemExistenceChecker(bagNum,slotID))
            return false;
        if (Bag[bagNum].itemList[slotID].itemType == ItemType.Potion)
        {
            if (Bag[bagNum].itemList[slotID].available )
            {
                bool isHeal = playerObject.GetComponent<Damageable>().Heal(Bag[bagNum].itemList[slotID].Healnum);
                if (!isHeal)
                {
                    Debug.Log("��e��q�w��");
                    GameMessageEvents.AddMessage("��q�w��",3f);
                }
                else
                {
                    index = true;
                    Debug.Log(Bag[bagNum].itemList[slotID].coolingTime);
                    //�p�G�O�̫�@�Ӫ��~�]��^�_�A���F�N�o�ɶ���UI���`�B�@
                    if (Bag[bagNum].itemList[slotID].itemHeld == 1)
                        index = false;
                    Deleteitems(slotID,bagNum);
                    RefreshItem(bagNum);
                }
            }
            Debug.Log(playerObject.GetComponent<Damageable>().health);
        }

        return index;
    }
    /// <summary>
    /// ����Y�I�]�x�s��ƶq
    /// </summary>
    /// <param name="bagNum"></param>
    /// <returns></returns>
    public int BagCount(int bagNum)
    {
        bagNum = Mathf.Clamp(bagNum, 0, Bag.Length);
        return Bag[bagNum].itemList.Count;
    }
    /// <summary>
    /// �R���I�]�A�åB���s�ͦ�
    /// </summary>
    public void RefreshItem(int bagNum)
    {
        //�P���I�]����í��s�ͦ�
        for (int i=0 ; i < slotGrid[bagNum].transform.childCount ; i++)
        {
            if (slotGrid[bagNum].transform.childCount == 0)
                break;
            Destroy(slotGrid[bagNum].transform.GetChild(i).gameObject);
        }
        slotsDictionary["slots_"+bagNum].Clear();
        for (int i = 0; i < Bag[bagNum].itemList.Count; i++)
        {
            
            //CreateNewItem(instance.myBag.itemList[i]);
            slotsDictionary["slots_"+bagNum].Add(Instantiate(emptySlot[bagNum]));
            slotsDictionary["slots_"+bagNum][i].transform.SetParent(slotGrid[bagNum].transform);
            slotsDictionary["slots_"+bagNum][i].GetComponent<slot>().slotID = i;
            //slotsDictionary["slots_" + bagNum][i].GetComponent<slot>().slotItem = Bag[bagNum].itemList[i];
            slotsDictionary["slots_"+bagNum][i].GetComponent<slot>().SetupSlot(Bag[bagNum].itemList[i]);
            //�N�I�]�C��������ǻ��^��
            slotsDictionary["slots_"+bagNum][i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //��ڦb�ͦ��ֱ��I�]��
            if (Bag[bagNum].name == "shortcut")
            {
                slotsDictionary["slots_" + bagNum][i].GetComponent<slot>().tmp.text = (i+1).ToString();
            }
        }
        Debug.Log("��s�I�]����");
        if (bagNum == 1)
        {
            ItemUpDate.BagUpData.Invoke();
        }
    }
    /// <summary>
    /// �R���I�]List�A�åB���s�ͦ�List
    /// </summary>
    public void resetbag(int bagNum) 
    {
        for(int i = 0;i < Bag[bagNum].itemList.Count;i++)
        {
            if(Bag[bagNum].itemList[i] != null)
            {
                Bag[bagNum].itemList[i].itemHeld = 1;
            }
        }

        var orgItemListNum = Bag[bagNum].itemList.Count;
        Bag[bagNum].itemList.Clear();
        for (int i = 0; i <orgItemListNum; i++)
        {
            Bag[bagNum].itemList.Add(null);
        }
    }

    [ContextMenu("�P���I�]")]
    public void deltbag(int bagNum)
    {
        resetbag(bagNum);
        RefreshItem(bagNum);
    }
    
    /// <summary>
    /// �j�M�I�]�����~
    /// </summary>
    /// <param name="bag">�I�]</param>
    /// <param name="slot">��l</param>
    /// <returns></returns>
    public static item ReturnBagSlotItem(int bag,int slot)
    {
        if (Instance.Bag[bag].itemList[slot] == null)
        {
            return null;
        }
        return Instance.Bag[bag].itemList[slot];
    }
}



//[CustomEditor(typeof(invventoryManger))]
//public class invventorymanger_editor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        invventoryManger script = (invventoryManger)target;

//        GUILayout.Space(20);
//        if(GUILayout.Button("�P���I�]"))
//        {
//            invventoryManger.resetbag();
//            invventoryManger.RefreshItem();
//        }
//    }
//}