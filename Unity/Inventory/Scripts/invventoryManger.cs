

using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

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
    
    public Dictionary<string, List<GameObject>> slotsDictionary = new Dictionary<string, List<GameObject>>()
    {
        { "slots_0",  new List<GameObject>() },
        { "slots_1",  new List<GameObject>() }
    };
    
    
    private void OnEnable()
    {
        RefreshItem(0);
        RefreshItem(1);
        itemInfromation[0].text = "";
        PlayerController.lockplay = true;
    }
    private void OnDisable()
    {
        PlayerController.lockplay = false;
    }
    private void Start()
    {
        
        //
        RefreshItem(0);
        RefreshItem(1);
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
    public void AddNewItem(item thisItem,int bagNum)
    {
        // �P�_�o�ӦC��(�I�])���O�_�w�g�S���o�Ӫ��~
        if (!Bag[bagNum].itemList.Contains(thisItem))
        {
            //�P�w�b�I�]��18�椤�O�_���Ŧ�
            for (int i = 0; i < Bag[bagNum].itemList.Count; i++)
            {
                if (Bag[bagNum].itemList[i] == null)
                {
                    Bag[bagNum].itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;
        }
        RefreshItem(0);
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
        RefreshItem(0);
    }
    /// <summary>
    /// �o�Ӥ�k�Ω�B�z�b���Ĥ����ϥΡC
    /// </summary>
    /// <param name="playerObject">�Ĥ��^�_��H�C</param>
    /// <param name="slotID">���~�Ҧb���Ѧ� ID�C</param>
    /// <param name="bag">�ثe�I�]</param>
    public void Purify(GameObject playerObject,int slotID,int bagNum)
    {
        if (Bag[bagNum].itemList[slotID].available)
        {
            if (!PlayerVar.Instance.shuDyeingVar.TestFilthyVar(0))
            {
                PlayerVar.Instance.shuDyeingVar.FilthySub(Bag[bagNum].itemList[slotID].purifyNum);
                Deleteitems(slotID,bagNum);
                RefreshItem(bagNum);
            }
        }
    }
    /// <summary>
    /// <summary>
    /// �o�Ӥ�k�Ω�B�z�Ĥ����ϥΡC
    /// </summary>
    /// <param name="playerObject">�Ĥ��^�_��H�C</param>
    /// <param name="slotID">���~�Ҧb���Ѧ� ID�C</param>
    /// <param name="bag">�ثe�I�]</param>
    public void Potion(GameObject playerObject,int slotID,int bagNum)
    {
        if (Bag[bagNum].itemList[slotID].available)
        {
            bool isHeal = playerObject.GetComponent<Damageable>().Heal(Bag[bagNum].itemList[slotID].Healnum);
            if (isHeal)
            {
                Deleteitems(slotID,bagNum);
                RefreshItem(bagNum);
            }
        }
        Debug.Log(playerObject.GetComponent<Damageable>().health);
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
            slotsDictionary["slots_"+bagNum].Clear();
        }
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