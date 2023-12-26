#if UNITY_EDITOR
using Events;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDropsWorldManger))]
public class ItemDropsWorldMangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ItemDropsWorldManger idwm = (ItemDropsWorldManger)target;
        if (GUILayout.Button("Ĳ�o�������~1��"))
        {
            ItemEvents.ItemDropsTheWorld.Invoke(idwm.gameObject.transform.position,new Vector2(idwm.chance,1),idwm.itemID,idwm.test);
        }
        if (GUILayout.Button("Ĳ�o�������~10��"))
        {
            for (int i = 0; i < 10; i++)
            {
                ItemEvents.ItemDropsTheWorld.Invoke(idwm.gameObject.transform.position,new Vector2(idwm.chance,1),idwm.itemID,idwm.test);
            }
        }
        if (GUILayout.Button("Ĳ�o�������~100��"))
        {
            for (int i = 0; i < 100; i++)
            {
                ItemEvents.ItemDropsTheWorld.Invoke(idwm.gameObject.transform.position,new Vector2(idwm.chance,1),idwm.itemID,idwm.test);
            }
        }
    }
}
[CustomPropertyDrawer(typeof(ItemIDNameAttribute))]
public class ItemIDNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int itemID = 0;
        if (ItemList.Instance != null)
        {
            if (ItemList.Instance.Item != null && ItemList.Instance.Item.Count > 0)
            {
                itemID = Mathf.Clamp(property.intValue,0, ItemList.Instance.Item.Count - 1) ;
                
                // ���o���~ID�A�B�]�w�b���~���̤j�̤p�Ȥ�
         
        
                // ?���۩w??��
                ItemIDNameAttribute itemNameAttribute = attribute as ItemIDNameAttribute;
                string itemNamePrefix = itemNameAttribute.prefix;

                // ?�m??
                GUIContent customLabel = new GUIContent(itemNamePrefix + ItemList.Instance.Item[itemID].itemNameNbt);

                // ?���q?��?��?��
                EditorGUI.PropertyField(position, property, customLabel);
            }
        }
       
    }
}

// �۩w??��?
public class ItemIDNameAttribute : PropertyAttribute
{
    public string prefix;

    public ItemIDNameAttribute(string prefix)
    {
        this.prefix = prefix;
    }
}
#endif