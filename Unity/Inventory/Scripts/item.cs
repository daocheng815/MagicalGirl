using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/New Item")]
// ScriptableObject �OUnity���O���s�W���󪺤�k
public class item : ScriptableObject
{
    public string itemName;
    public Sprite itemImaage;
    public int itemHeld;
    [TextArea]
    public string iteminfo;
    public bool available;

    public int Healnum;
}

