using UnityEngine;

/// <summary>
/// 格子類型
/// </summary>
public enum E_Node_Type
{
    //可以走的地方
    Walk,
    //不能走的地方
    Stop,
}
/// <summary>
/// A星格子類
/// </summary>
public class AstartNode
{
    //格子對象的座標
    public int x;
    public int y;
    //巡路消耗
    public float f;
    //離起點距離
    public float g;
    //離終點距離
    public float h;
    //父對象
    public AstartNode father;
    
    //格子的類型
    public E_Node_Type type;
    /// <summary>
    /// 構造函數
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    public AstartNode(int x, int y, E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}