using System;
using System.Collections.Generic;
using UnityEngine;
using Ebt;
public class TestBT:MonoBehaviour
{
    
    private Node _rootNode;
    private SearchToPlayers sp;
    private readonly Blockboard _blockboard = new Blockboard();
    
    private void Awake()
    {
        sp = GetComponent<SearchToPlayers>();
    }
    
    
    private void Start()
    {
        // // 创建主行为树
        // var findPath = new FindPath(() => transform.position, () => sp.playerPos);
        // var findPathTime = new LimitedFrequencyNode(findPath,0.5f);
        // var debug = new LimitedFrequencyNode(new Leaf(ABV), 0.5f);
        // var sl1 = new Selector(findPathTime,debug);
        // var sq1 = new Sequence(new ConditionNode((o,b) => sp.distance <= 10),sl1);
        // _rootNode = new Root(sq1);
        
    }

    public void ABV(GameObject g, Blockboard b)
    {
        var a = b.Get<List<Vector2>>("nodes");
        Debug.Log(a.Count);
    }

    public Vector2 GetPos()
    {
        return transform.position;
    }

    public void SetPos(Vector2 newPos)
    {
        transform.position = newPos;
    }

    private void Update()
    {
        // _rootNode.Execute(gameObject,_blockboard);
    }
}
