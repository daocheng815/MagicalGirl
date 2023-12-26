using System;
using Events;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SetAstartTileMap : Singleton<SetAstartTileMap>
{
    [SerializeField]private Tilemap tilemap;
    
    private int _cXmin;
    private int _cYmin;
    private BoundsInt _bounds;
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
        InitMapTtpe();
    }
    /// <summary>
    ///  初始化
    /// </summary>
    public void InitMapTtpe()
    {
        Debug.Log("初始化開始");
        AMT.AmtUpdata = false;
        //分配內存給陣列
        AMT.InitMapType = new E_Node_Type[_bounds.size.x, _bounds.size.y];
        for (int i = 0; i < _bounds.size.x; i++)
        {
            for (int j = 0; j < _bounds.size.y; j++)
            {
                switch (SearchTileMapToName(i,j))
                {
                    case StartWark:
                        AMT.InitMapType[i, j] = E_Node_Type.Walk;
                        break;
                    case StartStop:
                        AMT.InitMapType[i, j] = E_Node_Type.Stop;
                        break;
                    case null:
                        AMT.InitMapType[i, j] = E_Node_Type.Stop;
                        break;
                }
            }
        }
        
        AMT.AmtUpdata = true;
        //初始化巡路
        AstartManger.Instance.InitMapInfo(_bounds.size.x, _bounds.size.y);
#if UNITY_EDITOR
        InitTextInfo(_bounds.size.x+1,_bounds.size.y);
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
    /// <summary>
    /// 轉換巡路地圖座標為世界座標
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 ChengerCtoW(Vector2Int pos)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(pos.x - _cXmin, pos.y - _cYmin, 0));;
    }

    public Vector3? ChengerCtoW(AstartNode node)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(node.x - _cXmin, node.y - _cYmin, 0));;
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
    public Vector3? RandomNotStopNode(Vector3 myNode,int zMax =1 ,int zMin = 0)
    {
        AstartNode node = AstartManger.Instance.RandomNotStopNode(ChengerWtoC(myNode).x,ChengerWtoC(myNode).y,zMax,zMin);
        return ChengerCtoW(node);
    }
    public bool IsNodesStop(Vector3 pos)
    {
        Vector2Int a = ChengerWtoC(pos);
        return AstartManger.Instance.IsNodesStop(a.x, a.y);
    }
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
        for (int i = 0; i < _bounds.size.x; i++)
        {
            for (int j = 0; j < _bounds.size.y; j++)
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
}