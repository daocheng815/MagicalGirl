using System.Collections.Generic;
using Daocheng.BehaviorTrees;
using Daocheng.BehaviorTrees.Conditionals;
using Daocheng.BehaviorTrees.Decorator;
using UnityEngine;
//賽雷斯
public class Sairesuu_boss : BehaviorTree
{
    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int health = 500;

    [SerializeField] private float eyeDistanceMax = 12;
    [SerializeField] private float eyeDistanceMin = 4;
    [SerializeField] private float speed = 5f;
    
    [SerializeField] private Move move;
    [SerializeField] private SearchToPlayers searchToPlayers;
    [SerializeField] private Damageable damageable;
    public List<Vector2> pathNodes;
    private void SetComponents()
    {
        move = gameObject.AddComponent<Move>();
        searchToPlayers = gameObject.AddComponent<SearchToPlayers>();
        damageable = gameObject.AddComponent<Damageable>();
    }
    private void SetValues()
    {
        Blackboard.Set("Damageable",damageable);
        Blackboard.Set("Move",move);
        Blackboard.Set("SearchToPlayers",searchToPlayers);

        damageable.MaxHealth = maxHealth;
        damageable.health = health;
    
        move.eyeDistanceMax = eyeDistanceMax;
        move.eyeDistanceMin = eyeDistanceMin;
        move.Speed = speed;
    }
    
    /// <summary>
    /// 初始設定
    /// </summary>
    protected override void OnSetup()
    {
        //加入元件
        SetComponents();
        //初始化 數值
        SetValues();
        //建構行為樹
        var distanceRangeTask = new DistanceRangeTask();
        var findPath = new FindPathTask(()=>searchToPlayers.GetPos(),()=>searchToPlayers.GetPlayerPos(),4,3,true,true);
        var pathMove = new PathMoveTask();
        var pf = new Selector(findPath, pathMove);
        var flip = new Sequence(new LimitedExecutionNode(0.2f),new MoveFlip());
        
        //檢查是否停下
        var whyStop = new BoolConditional(() => move.Stop);
        var setObtainPath = new Actions(() => Blackboard.Set(Names.ObtainPath, false));
        var findRandom = new FindRandomPath(1, 0);
        var wsf = new Sequence(whyStop, setObtainPath, findRandom);
        
        var debugTest1 = new DebugTask("進入玩家範圍");
        var debugTest2 = new DebugTask("離開玩家範圍");
        var dFlip = new Sequence(new LimitedExecutionNode(0.2f),new distanceFlip());
        var set = new Actions(() =>
        {
            Blackboard.Get<List<Vector2>>(Names.PathNodes).Clear();
            Blackboard.Set(Names.CurrentIndex,0);
            Blackboard.Set(Names.ObtainPath,false);
        });
        
        var t2 = new Sequence(new BoolConditional(() => searchToPlayers.distance < move.eyeDistanceMin),set, debugTest2,
            dFlip);
        
        var path = new Sequence(distanceRangeTask , debugTest1 , pf , flip , wsf);
        
        var text = new Selector(path,t2);
        
        Root = text;
    }

    protected override void OnUpdate()
    {
        pathNodes = Blackboard.Get<List<Vector2>>(Names.PathNodes);
    }
}
