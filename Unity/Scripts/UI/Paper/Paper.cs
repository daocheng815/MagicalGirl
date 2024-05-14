using UnityEngine;

[CreateAssetMenu(fileName = "New Paper",menuName = "Paper/New Paper")]
public class Paper : ScriptableObject
{
    [Header("標題")]
    [TextArea(1, 10)]
    public string tileText;
    [Header("信紙內容")]
    [TextArea(40, 10)]
    public string textInfo;
}
