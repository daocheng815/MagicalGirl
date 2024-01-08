
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AstartMap" , menuName = "AstartMap/newMap")]
public class AstartMap : ScriptableObject
{
    /// <summary>
    /// A星座標轉換世界座標
    /// </summary>
    public Dictionary<Vector2Int, Vector2> InitMapPosCtoW = new Dictionary<Vector2Int, Vector2>();
    /// <summary>
    /// 世界座標轉換A星座標
    /// </summary>
    public Dictionary<Vector2, Vector2Int> InitMapPosWtoC = new Dictionary<Vector2, Vector2Int>();
    /// <summary>
    /// A星地圖Type
    /// </summary>
    public E_Node_Type[,] InitMapType;
}