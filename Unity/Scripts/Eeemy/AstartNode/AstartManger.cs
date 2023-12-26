using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;
using Random = System.Random;

/// <summary>
/// A星巡路管理器 單利模式
/// </summary>
public class AstartManger : CSingleton<AstartManger>
{
    //地圖的寬高
    private int mapW,mapH;
    //地圖相關所有格子對象容器，也就是地圖陣列
    public AstartNode[,] nodes;
    //開啟列表
    private List<AstartNode> openList = new List<AstartNode>();
    //關閉列表
    private List<AstartNode> closeList = new List<AstartNode>();
    // g的斜邊長度
    private const float SD = 1.4f;
    
    /// <summary>
    /// 初始化地圖訊息
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <param name="xmin"></param>
    /// <param name="ymin"></param>
    public bool InitMapInfo(int w, int h)
    {
        var index = AMT.AmtUpdata;
        if (!index) return false;
        //記錄寬高
        this.mapW = w;
        this.mapH = h;
        //分配內存空間給地圖陣列
        nodes = new AstartNode[w, h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                AstartNode node = new AstartNode(i, j, AMT.InitMapType[i, j]);
                nodes[i, j] = node;
            }
        }
        //根據寬高創建格子
        Debug.Log("完成");
        return true;
    }
    
    /// <summary>
    /// 巡路方式 提供給外部使用
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    [CanBeNull]
    public List<AstartNode> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        
        Debug.Log("開始判定");
        //判斷座標是否正確
        if (startPos.x < 0 || startPos.x >= mapW ||
            startPos.y < 0 || startPos.y >= mapH ||
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH)
        {
            Debug.Log("開始或是結束點在地圖範圍外。");
            return null;
        }
        // 要不要阻擋
        //獲取起點終點的格子
        AstartNode start = nodes[startPos.x, startPos.y];
        AstartNode end = nodes[endPos.x, endPos.y];
        if (start.type == E_Node_Type.Stop ||
            end.type == E_Node_Type.Stop)
        {
            Debug.Log("開始或是結束點是阻擋");
            return null;
        }
        //清空關閉和開始列表
        closeList.Clear();
        openList.Clear();
        
        //把開始點放入關閉列表內
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);
        int nums = 0;
        while (true)
        {
            // 從起點開始找周圍的點 並放入開啟列表中
            // 左上 x-1 y-1
            FindNearlyNodeToOpenList(start.x - 1, start.y - 1, SD, start, end);
            // 上 x y-1
            FindNearlyNodeToOpenList(start.x      , start.y - 1, 1 , start, end);
            // 右上 x+1 y-1
            FindNearlyNodeToOpenList(start.x + 1, start.y - 1, SD, start, end);
            // 左 x-1 y
            FindNearlyNodeToOpenList(start.x - 1, start.y      , 1 , start, end);
            // 右 x+1 y
            FindNearlyNodeToOpenList(start.x + 1, start.y      , 1 , start, end);
            // 左下 x-1 y+1
            FindNearlyNodeToOpenList(start.x - 1, start.y + 1, SD, start, end);
            // 下 x y+1
            FindNearlyNodeToOpenList(start.x      , start.y + 1, 1 , start, end);
            // 右下 x+1 y+1
            FindNearlyNodeToOpenList(start.x + 1, start.y + 1, SD, start, end);
        
            //選出開啟列表中 馴鹿消耗最小的點
            openList.Sort(SortOpenList);
            //放入關閉列表中 然後再從列表中移除
            closeList.Add(openList[0]);
            //找的這個點 又變成新的點 進行下次巡路運算
            start = openList[0];
            openList.RemoveAt(0);
            // 如果這個點已經是終點了 那麼得到最終結果返回出去
            // 如果這個點不是 終點 那麼繼續尋路
            
            if (start == end)
            {
                //找完了 找到路徑了
                List<AstartNode> path = new List<AstartNode>();
                path.Add(end);
                while (end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                
                //列表反轉的API
                path.Reverse();
                return path;
            }

            nums++;
            if (nums > 30)
                return null;
        }
    }
    /// <summary>
    /// 排序函數
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int SortOpenList(AstartNode a, AstartNode b)
    {
        if (a.f > b.f)
            return 1;
        else if (a.f == b.f)
            return 1;
        else
            return -1;
    }
    
    /// <summary>
    /// 把鄰近的點放入開啟列表中的函數
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void FindNearlyNodeToOpenList(int x, int y,float g,AstartNode father,AstartNode end)
    {
        //邊界判斷
        if (x < 0 || x >= mapW ||
            y < 0 || y >= mapH)
            return;
        AstartNode node = nodes[x, y];
        //判斷點位 是否在邊界 是否阻擋 是否在開啟或關閉列表中 如果不都是 才放入開啟列表
        if(node == null || 
           node.type == E_Node_Type.Stop||
           closeList.Contains(node)||
           openList.Contains(node))
            return;
        //計算f值;
        //f = g+h
        //紀錄父對象
        node.father = father;
        //計算g 我離起點的距離 就是我父親裡起點的距離 + 我離父親的距離
        node.g = father.g + g;
        //估算與終點的距離
        node.h = Math.Abs(end.x - node.x) + Math.Abs(end.y - node.y);
        //計算距離
        node.f = node.g + node.h;
        // add /////////////////////////////////////////////
        //有个问题 如果节点已经在开启列表中 但下一个最小值点也可以算到该点 那么该点的父节点不会变为这个最小值点 就会导致路径不是最短的 应该还要添加判断：
        if (openList.Contains(node))
        {
            float gThis = father.g + g;
            if (gThis < node.g)
            {
                node.g = gThis;
                node.f = node.g + node.h;
                node.father = father;
                return;
            }
            else
            {
                return;
            }
        }
        // add /////////////////////////////////////////////
        //如果通過上面的合法驗證 就存到開啟列表中
#if UNITY_EDITOR
        if(SetAstartTileMap.Instance.isTestInfo)
            SetAstartTileMap.Instance.UpdataTextInfo(x+1,y,node.g,node.h,node.f);
#endif
        openList.Add(node);
    }

    /// <summary>
    /// 隨機取得某節點周圍的節點
    /// </summary>
    /// <param name="x">節點座標x</param>
    /// <param name="y">節點座標y</param>
    /// <param name="zMax">搜尋最大半徑</param>
    /// <param name="zMin">搜尋最小半徑</param>
    /// <returns></returns>
    [CanBeNull]
    public AstartNode RandomNotStopNode(int x,int y,int zMax = 1 ,int zMin = 0)
    {
        if (zMin > zMax)
        {
            Debug.Log("最小值不可能大於最大值");
            return null;
        }
        Random random = new Random();
        List<AstartNode> nodeList = new List<AstartNode>();
        int c = 0;
        for (int i = -zMax; i <= zMax; i++)
        {
            for (int j = -zMax; j <= zMax ; j++)
            {
                int nx = x + i;
                int ny = y + j;
                //邊界判斷
                if (!(nx < 0 || nx >= mapW || ny < 0 || ny >= mapH))
                {
                    //判定不是本身
                    if (i!= 0 || j != 0)
                    {
                        if (Mathf.Abs(i) <= zMin && Mathf.Abs(j) <= zMin)
                        {
                            continue; // 在排除范围内，跳过
                        }
                        
                        if (nodes[nx, ny].type == E_Node_Type.Walk)
                        {
                            c++;
#if UNITY_EDITOR
                            if(SetAstartTileMap.Instance.isTestInfo)
                                SetAstartTileMap.Instance.ChangeColorTextInfo(nx+1, ny);
#endif
                            nodeList.Add(nodes[nx, ny]);
                        }
                    }
                }
            }
        }
        if (nodeList.Count > 0)
        {
            Debug.Log("周圍共有"+c+"可行走的點");
            return nodeList[random.Next(0, nodeList.Count)];
        }
        return null;
    }
    public bool IsNodesStop(int x, int y)
    {
        return nodes[x, y].type == E_Node_Type.Stop;
    }
    /// <summary>
    /// 判定某個點的上下左右是否為阻擋
    /// </summary>
    private bool NodesStop(int x, int y)
    {
        return nodes[x, y - 1].type == E_Node_Type.Stop ||
               nodes[x, y + 1].type == E_Node_Type.Stop ||
               nodes[x - 1, y].type == E_Node_Type.Stop ||
               nodes[x + 1, y].type == E_Node_Type.Stop;
    }
}
