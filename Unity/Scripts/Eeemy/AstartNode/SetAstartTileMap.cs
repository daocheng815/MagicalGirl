using System;
using Events;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class SetAstartTileMap : Singleton<SetAstartTileMap>
{
    [SerializeField]private Tilemap tilemap;
    public AstartMap aStartMap;
    private int _cXmin;
    private int _cYmin;
    private int _maph;
    private int _mapw;
    private BoundsInt _bounds;
    private const float XYOffset = 0.5f;
    private const string StartWark = "Astart_0";
    private const string StartStop = "Astart_1";
    
    
#if UNITY_EDITOR
    private Vector3 _lastWorldPosition;
    [field: SerializeField] public string Pname { get; private set; }
    public GameObject testInfo;

    public bool isTestInfo = true;
#endif
    /// <summary>
    /// 在單利模式中要記得繼承Awake
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _bounds = tilemap.cellBounds;
        _cXmin = Math.Abs(_bounds.xMin);
        _cYmin = Math.Abs(_bounds.yMin);
        _mapw = _bounds.size.x;
        _maph = _bounds.size.y;
        InitMapTtpe();
    }
    /// <summary>
    ///  初始化
    /// </summary>
    public void InitMapTtpe()
    {
        //分配內存給A星管理器
        aStartMap.InitMapType = new E_Node_Type[_mapw, _maph];
        aStartMap.InitMapPosWtoC.Clear();
        aStartMap.InitMapPosCtoW.Clear();
        Debug.Log("初始化開始");
        for (int i = 0; i < _mapw; i++)
        {
            for (int j = 0; j < _maph; j++)
            {
                aStartMap.InitMapPosWtoC.Add(ChengerCtoW(new Vector2Int(i, j)),new Vector2Int(i,j));
                aStartMap.InitMapPosCtoW.Add(new Vector2Int(i,j),ChengerCtoW(new Vector2Int(i, j)));
                switch (SearchTileMapToName(i,j))
                {
                    case StartWark:
                        aStartMap.InitMapType[i,j] = E_Node_Type.Walk;
                        break;
                    case StartStop:
                        aStartMap.InitMapType[i,j] = E_Node_Type.Stop;
                        break;
                    case null:
                        aStartMap.InitMapType[i,j] = E_Node_Type.Stop;
                        break;
                }
            }
        }
        //初始化巡路
        AstartManger.Instance.InitMapInfo(_mapw, _maph);
#if UNITY_EDITOR
        InitTextInfo(_mapw+1,_maph);
#endif
    }
    
    /// <summary>
    /// 巡路函數
    /// </summary>
    /// <param name="start">開始的點</param>
    /// <param name="end">結束的點</param>
    /// <returns></returns>
    [CanBeNull]
    public List<AstartNode> FindPath(Vector3 start,Vector3 end)
    {
         var nodes = AstartManger.Instance.FindPath(ChengerWtoC(start),
            ChengerWtoC(end));
         return nodes;
    }
    /// <summary>
    /// 快速巡路函數
    /// </summary>
    /// <param name="start">開始的點</param>
    /// <param name="end">結束的點</param>
    /// <returns></returns>
    [CanBeNull]
    public List<AstartNode> FastFindPath(Vector3 start,Vector3 end)
    {
        Vector2Int? sn = WtoC(start);
        Vector2Int? en = WtoC(end);

        if (sn != null && en != null)
        {
            Vector2Int s = (Vector2Int)sn;
            Vector2Int e = (Vector2Int)en;

            var nodes = AstartManger.Instance.FindPath(s, e);
            return nodes;
        }
        // 返回空列表或適當的預設值，視情況而定
        return new List<AstartNode>();
    }
    /// <summary>
    /// 查找陣列中TileMap的名稱
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [CanBeNull]
    private string SearchTileMapToName(int x,int y)
    {
        string index = null;
        Vector3Int v3O = new Vector3Int(x - _cXmin, y - _cYmin, 0);
        TileBase tile = tilemap.GetTile( v3O );
        if (tile != null && tile is Tile)
        {
            Sprite sprite = (tile as Tile).sprite;
            index = sprite.name;
        }
        return index;
    }
    /// <summary>
    /// 轉換世界座標為巡路地圖座標
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2Int ChengerWtoC(Vector3 pos)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        return new Vector2Int(cellPosition.x+_cXmin,cellPosition.y+_cYmin);
    }

    public Vector2Int? WtoC(Vector3 pos)
    {
        var x = Mathf.FloorToInt(pos.x) + _cXmin;
        var y = Mathf.FloorToInt(pos.y) + _cYmin;
        if (x < 0 && x >= _mapw && y < 0 && y >= _maph)
            return null;
        return new Vector2Int(x , y);
        //轉換數值
        // Vector2 w = new Vector2(Mathf.FloorToInt(pos.x) + 0.5f, Mathf.FloorToInt(pos.y) + 0.5f);
        // Vector2 b = new Vector2(x,y);
        // if (aStartMap.InitMapPosWtoC.TryGetValue(w, out var v))
        // {
        //     DebugTask.Log(v+" "+b);
        //     return v;
        // }
        // return null;
    }
    /// <summary>
    /// 轉換巡路地圖座標為世界座標
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 ChengerCtoW(Vector2Int pos)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(pos.x - _cXmin, pos.y - _cYmin, 0));
    }

    public Vector3? ChengerCtoW(AstartNode node)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(node.x - _cXmin, node.y - _cYmin, 0));;
    }
    /// <summary>
    /// 轉換巡路座標為世界座標
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public Vector2? CtoW(AstartNode node)
    {
        var x = node.x - _cXmin + XYOffset;
        var y = node.y - _cYmin + XYOffset;
        if (x < 0 && x >= _mapw && y < 0 && y >= _maph)
            return null;
        return new Vector2(x , y);
        
        
        if (aStartMap.InitMapPosCtoW.TryGetValue(new Vector2Int(node.x, node.y), out var v))
        {
            return v;
        }
        return null;
    }
    /// <summary>
    /// 轉換整個巡路清單
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public List<Vector2> NodesCtoW(List<AstartNode> nodes)
    {
        List<Vector2> path = new List<Vector2>();
        foreach (var node in nodes)
        {
            var n = CtoW(node);
            if(n!=null)
                path.Add((Vector2)n);
        }
        return path;
    }
    public List<Vector2> ChengerNodesToW(List<AstartNode> nodes)
    {
        List<Vector2> path = new List<Vector2>();
        foreach (var node in nodes)
        {
            var n = ChengerCtoW(node);
            if(n != null)
                path.Add((Vector2)n);
        }
        return path;
    }
    public Vector3? RandomNotStopNode(Vector3 myNode,int zMax =1 ,int zMin = 0,bool nodeStop = false)
    {
        var n = WtoC(myNode);
        if (n != null)
        {
            var no = (Vector2Int)n;
            AstartNode node = AstartManger.Instance.RandomNotStopNode(no.x,no.y,zMax,zMin,nodeStop);
            return CtoW(node);
        }
        return null;
    }
    public bool IsNodesStop(Vector3 pos)
    {
        Vector2Int a = ChengerWtoC(pos);
        return AstartManger.Instance.IsNodesStop(a.x, a.y);
    }

#region TextInfo
#if UNITY_EDITOR
    public void ChangeColorTextInfo(int x, int y)
    {
        var g = UpdataTextInfo(x, y, 0, 0, 0);
        g.GetComponent<TestInfo>().ChangeColor("#D83822");
    }
    public GameObject UpdataTextInfo(int x, int y , float g , float h , float f)
    {
        if (TextInfos[x, y] != null)
        {
            Destroy(TextInfos[x, y]);
            TextInfos[x, y] = null;
        }
        var n = ChengerCtoW(AstartManger.Instance.nodes[x, y]);
        if (n != null)
        {
            Vector3 pos = (Vector3)n;
            GameObject gobj = Instantiate(testInfo, new Vector3(pos.x-0.5f,pos.y-0.5f,pos.z), Quaternion.identity);
            TextInfos[x, y] = gobj;
            gobj.transform.SetParent(transform);
            gobj.GetComponent<TestInfo>().gText.text = g.ToString();
            gobj.GetComponent<TestInfo>().hText.text = h.ToString();
            gobj.GetComponent<TestInfo>().fText.text = f.ToString();
            return TextInfos[x, y];
        }
        return null;
    }
    // 測試使用
    private string ObtainTileMap(int x,int y)
    {
        string index = null;
        Vector3Int v3O = new Vector3Int(x, y , 0);
        TileBase tile = tilemap.GetTile( v3O );
        if (tile != null && tile is Tile)
        {
            Sprite sprite = (tile as Tile).sprite;
            
            index = sprite.name;
        }
        return index;
    }
    void Update()
    {
        // 只有在物体的世界空间坐标发生变化时才进行 Tilemap 坐标的转换
        Vector3 worldPosition = transform.position;
        if (worldPosition != _lastWorldPosition)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
            new Vector3(cellPosition.x+_cXmin,cellPosition.y+_cYmin,0);
            _lastWorldPosition = worldPosition;
            Pname = ObtainTileMap(cellPosition.x,cellPosition.y);
        }
    }
    //測試網格
    public GameObject[,] TextInfos;
    //初始化測試網格
    void InitTextInfo(int x, int y)
    {
        TextInfos = new GameObject[x+1, y];
    }
    //刪除測試
    public void DelTextInfo(float d = 0.1f)
    {
        for (int i = 0; i < _mapw; i++)
        {
            for (int j = 0; j < _maph; j++)
            {
                if (TextInfos[i, j] != null)
                {
                    Destroy(TextInfos[i, j]);
                    TextInfos[i, j] = null;
                }
            }
        }
    }
#endif
#endregion

}