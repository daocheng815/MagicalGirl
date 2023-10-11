
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class invventoryManger : MonoBehaviour
{
    static invventoryManger instance;

    public Inventory myBag;
    public GameObject slotGrid;
    public GameObject emptySlot;
    //public slot slotPrefab;
    public TextMeshProUGUI itemInfromation;
    public List<GameObject> slots = new List<GameObject>();
    

    void Awake()
    {
        if (instance!=null)
            Destroy(this);
        instance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
        instance.itemInfromation.text = "";
        PlayerController.lockplay = true;
        
    }
    private void OnDisable()
    {
        PlayerController.lockplay = false;
    }
    private void Start()
    {
        RefreshItem();
    }
    /// <summary>
    /// ��s���~�y�z
    /// </summary>
    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInfromation.text = itemDescription;
    }
    /// <summary>
    /// �������������~
    /// </summary>
    public static void RemoveAllItem()
    {
        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            if(instance.myBag.itemList[i] != null)
            {
                instance.myBag.itemList[i].itemHeld = 1;
                instance.myBag.itemList[i] = null;
            }
        }
    }
    /// <summary>
    /// �s�W���~
    /// </summary>
    public static void AddNewItem(item thisItem)
    {
        // �P�_�o�ӦC��(�I�])���O�_�w�g�S���o�Ӫ��~
        if (!instance.myBag.itemList.Contains(thisItem))
        {
            //�P�w�b�I�]��18�椤�O�_���Ŧ�
            for (int i = 0; i < instance.myBag.itemList.Count; i++)
            {
                if (instance.myBag.itemList[i] == null)
                {
                    instance.myBag.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;
        }
        RefreshItem();
    }
    /// <summary>
    /// �������~�C
    /// </summary>
    public static void Deleteitems(int slotID)
    {
        if (instance.myBag.itemList[slotID].itemHeld > 1)
            instance.myBag.itemList[slotID].itemHeld -= 1;
        else
        {
            instance.itemInfromation.text = "";
            instance.myBag.itemList[slotID] = null;
        }
        RefreshItem();
    }

    /// <summary>
    /// �o�Ӥ�k�Ω�B�z�Ĥ����ϥΡC
    /// </summary>
    /// <param name="playerObject">�Ĥ��^�_��H�C</param>
    /// <param name="slotID">���~�Ҧb���Ѧ� ID�C</param>
    public static void potion(GameObject playerObject,int slotID)
    {
        if (instance.myBag.itemList[slotID].available)
        {
            bool isHeal = playerObject.GetComponent<Damageable>().Heal(instance.myBag.itemList[slotID].Healnum);
            if (isHeal)
            {
                Deleteitems(slotID);
            }
        }
        Debug.Log(playerObject.GetComponent<Damageable>().health);
    }
    /// <summary>
    /// �R���I�]�A�åB���s�ͦ�
    /// </summary>
    public static void RefreshItem()
    {
        //�P���I�]����í��s�ͦ�
        for (int i=0; i< instance.slotGrid.transform.childCount;i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }
        for (int i = 0; i<instance.myBag.itemList.Count;i++)
        {
            //CreateNewItem(instance.myBag.itemList[i]);
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<slot>().slotID = i;
            instance.slots[i].GetComponent<slot>().SetupSlot(instance.myBag.itemList[i]);
            //�N�I�]�C��������ǻ��^��
        }
    }
    /// <summary>
    /// �R���I�]List�A�åB���s�ͦ�List
    /// </summary>
    public static void resetbag() 
    {
        for(int i = 0;i < instance.myBag.itemList.Count;i++)
        {
            if(instance.myBag.itemList[i] != null)
            {
                instance.myBag.itemList[i].itemHeld = 1;
            }
        }
        instance.myBag.itemList.Clear();
        for (int i = 0; i <18; i++)
        {
            instance.myBag.itemList.Add(null);
        }
    }

    [ContextMenu("�P���I�]")]
    public static void deltbag()
    {
        invventoryManger.resetbag();
        invventoryManger.RefreshItem();
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