using Events;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ItemDropsWorldManger))]
public class ItemDropsWorldMangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ItemDropsWorldManger idwm = (ItemDropsWorldManger)target;
        if (GUILayout.Button("觸發掉落物品1次"))
        {
            ItemEvents.ItemDropsWorld.Invoke(idwm.gameObject.transform.position,new Vector2(idwm.chance,1),idwm.itemID,idwm.test);
        }
        if (GUILayout.Button("觸發掉落物品10次"))
        {
            for (int i = 0; i < 10; i++)
            {
                ItemEvents.ItemDropsWorld.Invoke(idwm.gameObject.transform.position,new Vector2(idwm.chance,1),idwm.itemID,idwm.test);
            }
        }
        if (GUILayout.Button("觸發掉落物品100次"))
        {
            for (int i = 0; i < 100; i++)
            {
                ItemEvents.ItemDropsWorld.Invoke(idwm.gameObject.transform.position,new Vector2(idwm.chance,1),idwm.itemID,idwm.test);
            }
        }
    }
}
[CustomPropertyDrawer(typeof(ItemIDNameAttribute))]
public class ItemIDNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 取得物品ID，且設定在物品的最大最小值內
        int itemID = Mathf.Clamp(property.intValue,0, ItemList.Instance.Item.Count - 1) ;
        
        // ?取自定??性
        ItemIDNameAttribute itemNameAttribute = attribute as ItemIDNameAttribute;
        string itemNamePrefix = itemNameAttribute.prefix;

        // ?置??
        GUIContent customLabel = new GUIContent(itemNamePrefix + ItemList.Instance.Item[itemID].itemNameNbt);

        // ?用默?的?性?制
        EditorGUI.PropertyField(position, property, customLabel);
    }
}

// 自定??性?
public class ItemIDNameAttribute : PropertyAttribute
{
    public string prefix;

    public ItemIDNameAttribute(string prefix)
    {
        this.prefix = prefix;
    }
}
#endif