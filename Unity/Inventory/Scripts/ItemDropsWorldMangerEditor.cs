using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Events;
[CustomEditor(typeof(ItemDropsWorldManger))]
public class ItemDropsWorldMangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ItemDropsWorldManger idwm = (ItemDropsWorldManger)target;
        if (GUILayout.Button("觸發掉落物品1次"))
        {
            ItemDropsEvents.itemDropsWorld.Invoke(idwm.gameObject.transform.position,idwm.chance,idwm.itemID,idwm.test);
        }
        if (GUILayout.Button("觸發掉落物品10次"))
        {
            for (int i = 0; i < 10; i++)
            {
                ItemDropsEvents.itemDropsWorld.Invoke(idwm.gameObject.transform.position,idwm.chance,idwm.itemID,idwm.test);
            }
        }
        if (GUILayout.Button("觸發掉落物品100次"))
        {
            for (int i = 0; i < 100; i++)
            {
                ItemDropsEvents.itemDropsWorld.Invoke(idwm.gameObject.transform.position,idwm.chance,idwm.itemID,idwm.test);
            }
        }
    }
}
