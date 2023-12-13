using System.Collections.Generic;
using DG.Tweening;
using Events;
using ItemTypeEnum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagFuncMenuMange : Singleton<BagFuncMenuMange>
{
    public GameObject funcMenu;
    public Vector2 funcMenuOffset; 
    private RectTransform _rt;
    private GameObject _playerObject;

    [SerializeField]private GameObject background;
    [SerializeField]private GameObject actionButton;
    [SerializeField]private GameObject cancelButton;

    [SerializeField]private TextMeshProUGUI actionText;
    [SerializeField]private TextMeshProUGUI cancelText;

    private List<Image> _images = new List<Image>();
    
    [SerializeField]private bool _isOnDrag;
    
    //�������
    private int _mySlotID;
    private item _mySlotItem;
    private Inventory _myBag;
    protected override void Awake()
    {
        //�~�ӭ즳���ݩ�
        base.Awake();
        //�s�WFunc
        _playerObject = GameObject.Find("player");
        _rt = GetComponent<RectTransform>();
        funcMenu.SetActive(false);
        _isOnDrag = false;
        _images.Add(background.GetComponent<Image>());
        _images.Add(actionButton.GetComponent<Image>());
        _images.Add(cancelButton.GetComponent<Image>());
        //��l���C��
        foreach (var image in _images)
        {
            image.color = new Color(1, 1, 1, 0);
        }
        actionText.color = new Color(0, 0, 0, 0);
        cancelText.color = new Color(0, 0, 0, 0);
    }

    private void OnEnable()
    {
        BagFuncMenu.ItemOnClicked += ItemOnClicked;
        BagFuncMenu.ItemOnAction += FuncMenuAction;
        BagFuncMenu.IsItemOnDrag += IsOnDrag;
    }

    private void OnDisable()
    {
        BagFuncMenu.ItemOnClicked -= ItemOnClicked;
        BagFuncMenu.ItemOnAction -= FuncMenuAction;
        BagFuncMenu.IsItemOnDrag -= IsOnDrag;
    }

    private void IsOnDrag(bool index)
    {
        _isOnDrag = index;
        if (index)
            FuncMenuButton(false);
    }

    private void ItemOnClicked(int slotID, item slotItem,Inventory bag, Vector2 slotTransform)
    {
        _mySlotID = slotID;
        _mySlotItem = slotItem;
        _myBag = bag;
        
        Debug.Log("�I�����~ slotID:" + slotID + "�A���~�W��:"+slotItem.itemNameNbt + " �x�s���m:" + slotTransform);
        
        
        //���w�u��bmybag����A�t�@�ӭI�]�|�]��Grid����Ƨǰ��D���ͦ�m���~�A�����B�z
        if (bag.name == "mybag")
        {
            _rt.localPosition = slotTransform +�@funcMenuOffset;
            FuncMenuButton(true);
        }
    }
    
    // ���s���U�~Ĳ�o
    public void FuncMenuActionOn()
    {
        BagFuncMenu.ItemOnAction.Invoke();
    }
    public void FuncMenuAction()
    {
        Debug.Log("Ĳ�o���s");
        bool checkerBagName =  _myBag.name == "mybag";
        switch (_mySlotItem.itemType)
        {
            case ItemType.Potion :
                if (checkerBagName)
                    invventoryManger.Instance.Potion(_playerObject, _mySlotID,0);
                break;
            case ItemType.Purify :
                if (checkerBagName)
                    invventoryManger.Instance.Purify(_playerObject, _mySlotID,0);
                break;
            case ItemType.Buff :
                if (checkerBagName)
                    invventoryManger.Instance.Buff(_playerObject, _mySlotID,0);
                break;
        }
    }
    public void FuncMenuButton(bool index)
    {
        if (index)
        {
            
            funcMenu.SetActive(true);
            foreach (var image in _images)
            {
                image.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
            }
            DOTween.To(() => cancelText.color, x => cancelText.color = x, new Color(0, 0, 0, 1), 0.5f).SetEase(Ease.OutQuad);
            DOTween.To(() => actionText.color, x => actionText.color = x, new Color(0, 0, 0, 1), 0.5f).SetEase(Ease.OutQuad);
        }
        else
        {
            DOTween.To(() => cancelText.color, x => cancelText.color = x, new Color(0, 0, 0, 0), 0.2f).SetEase(Ease.OutQuad);
            DOTween.To(() => actionText.color, x => actionText.color = x, new Color(0, 0, 0, 0), 0.2f).SetEase(Ease.OutQuad);
            foreach (var image in _images)
            {
                image.DOFade(0f, 0.2f).SetEase(Ease.InQuad).OnComplete((() =>
                {
                    funcMenu.SetActive(false);
                }));
            }
        }
    }
}
