using System;
using System.Collections.Generic;
using Daocheng.BehaviorTrees;
using Daocheng.BehaviorTrees.Conditionals;
using Daocheng.BehaviorTrees.Decorator;
using DG.Tweening;
using Eeemy;
using Events;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Sequence = Daocheng.BehaviorTrees.Sequence;

//賽雷斯
public class Sairesuu_boss : BehaviorTree
{
    
    public enum Mods
    {
        Idle,
        Vision,
        GenerateSword,
        OnSword
    }
    public Mods mods = Mods.Idle;
    
    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int health = 500;

    [SerializeField] private float eyeDistanceMax = 15;
    [SerializeField] private float eyeDistanceMin = 4;
    [SerializeField] private float speed = 5f;
    
    [SerializeField] private Move move;
    [SerializeField] private SearchToPlayers searchToPlayers;
    [SerializeField] private Damageable damageable;
    private Animator _animator;

    [Header("劍")]
    [SerializeField] private GameObject sword;
    [SerializeField]private List<GameObject> _swordList = new List<GameObject>();

    [SerializeField]private bool spawanSword;

    [Header("盾")] 
    [SerializeField] private GameObject shield;
    [SerializeField]private List<GameObject> _shieldList = new List<GameObject>();
    private bool[] hs = new bool[3];
    [SerializeField]private bool shieldSword;
    [SerializeField]private bool shieldIsAliveSword;
    [Header("碰撞物")]
    [SerializeField] private GameObject collider;
    [Header("掉落物")]
    [SerializeField] private GameObject broken;
    public GameObject brokenR;
    public GameObject brokenL;
    [Header("巡路節點")]
    public List<Vector2> pathNodes;
    private void SetComponents()
    {
        move = gameObject.AddComponent<Move>();
        searchToPlayers = gameObject.AddComponent<SearchToPlayers>();
        damageable = gameObject.AddComponent<Damageable>();
        _animator = GetComponent<Animator>();
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
            //Blackboard.Get<List<Vector2>>(Names.PathNodes).Clear();
            Blackboard.Get<List<Vector2>>(Names.PathNodes);
            Blackboard.Set(Names.CurrentIndex,0);
            Blackboard.Set(Names.ObtainPath,false);
        });
        
        var t2 = new Sequence(new BoolConditional(() => searchToPlayers.distance < move.eyeDistanceMin),set, debugTest2,
            dFlip);
        
        var path = new Sequence(distanceRangeTask , debugTest1 , pf , flip , wsf);
        
        var text = new Selector(path,t2);
        
        Root = text;
    }
    
    

    private bool isDead = false;
    protected override void OnUpdate()
    {
        pathNodes = Blackboard.Get<List<Vector2>>(Names.PathNodes);

        if (!damageable.IsAlive)
        {
            foreach (var s in _shieldList)
            {
                s.GetComponent<RotateTransform>().Destroys();
            }
            _shieldList.Clear();
            shieldSword = false;
            
            foreach (var g in _swordList)
            {
                if(g != null)
                    Destroy(g);
            }
        }
            
        if (!damageable.IsAlive &&!isDead)
        {
            isDead = true;
            _animator.Play("Dead");
            Sairesuu_boss_plot plot = null;
            if (GetComponent<Sairesuu_boss_plot>() != null)
            {
                plot = GetComponent<Sairesuu_boss_plot>();
                plot.WallFade(0);
                plot.avgSystem.OnDramticEmotion("Sairesuu_2");
            }
            ItemEvents.
                ItemDropsTheWorld.Invoke(gameObject.transform.position,new Vector2(100,1),10,new Vector3(1,0,0.3f));
            Destroy(gameObject,2f);
        }
            
        
        switch (mods)
        {
            case Mods.Idle:
                Idle();
                break;
            case Mods.Vision:
                Vision();
                break;
            case Mods.GenerateSword:
                GenerateSword();
                break;
            case Mods.OnSword:
                OnSword();
                break;
        }

        Shield();
    }
    

    private void Shield()
    {
        if (shieldSword && !shieldIsAliveSword)
        {
            var f = false;
            foreach (var s in _shieldList)
            {
                if (s.GetComponent<RotateTransform>().isExist)
                {
                    shieldIsAliveSword = true;
                    f = true;
                    break;
                }
            }
            if (f)
            {
                foreach (var s in _shieldList)
                {
                    s.GetComponent<RotateTransform>().Destroys();
                }
                _shieldList.Clear();
                
                shieldSword = false;
            }
        }
        

        if (!hs[0])
        {
            if (!shieldSword)
            {
                hs[0] = true;
                spawanS();
            }
        }
        else if(damageable.health <= 350 && !hs[1])
        {
            if (!shieldSword)
            {
                hs[1] = true;
                spawanS();
            }
        }
        else if(damageable.health <= 150 && !hs[2])
        {
            if (!shieldSword)
            {
                hs[2] = true;
                spawanS();
            }
        }
    }

    private void spawanS()
    {
        //var a = Instantiate(collider, transform.position, quaternion.identity);
        //var c = a.GetComponent<CircleCollider2D>();
        //DOTween.To(() => c.radius, x => c.radius = x, 7f, 0.3f).OnComplete((() =>
        //{
        //    Destroy(a);
        //    shieldIsAliveSword = false;
        //    shieldSword = true;
        //    SS(12);
        //}));
        shieldIsAliveSword = false;
        shieldSword = true;
        SS(12);
    }
    private void Idle()
    {
        if (searchToPlayers.distance <= eyeDistanceMax)
        {
            mods = Mods.Vision;
        }
    }
    private void Vision()
    {

        Debug.Log(_swordList.Count);
        if (_swordList.Count == 0)
        {
            mods = Mods.GenerateSword;
        }
        if (searchToPlayers.distance > eyeDistanceMax)
        {
            mods = Mods.Idle;
        }
    }

    [Header("劍生成參數")] 
    [SerializeField]private float rotationSpeed = 0.5f;

    [SerializeField]private int swordNum = 0;
    private void GenerateSword()
    {
        if (!spawanSword)
        {
            if (damageable.health <= 150)
            {
                SW(6);
            }
            else if (damageable.health <= 200)
            {
                SW(5);
            }
            else if (damageable.health <= 300)
            {
                SW(4);
            }
            else if (damageable.health <= 350)
            {
                SW(3);
            }
            else if (damageable.health <= 500)
            {
                SW(2);
            }
            
            mods = Mods.OnSword;
        }
        spawanSword = true;
    }
    private void SW(int num)
    {
        var subV = 360 / num;
        var n = 0f;
        swordNum = 0;
        for (int i = 0; i < num; i++)
        {
            Spawansword(n);
            swordNum++;
            n += subV;
        }
    }
    private void Spawansword(float rangle)
    {
        var g = Instantiate(sword, transform.position, quaternion.identity);
        g.gameObject.SetActive(true);
        _swordList.Add(g);
        var s = g.GetComponent<Sword>();
        s.initAngle = rangle;
        s.centerObject = gameObject.transform;
        s.rotationSpeed = rotationSpeed;
    }
    [Header("盾生成參數")] 
    [SerializeField]private float rotationShieldNumSpeed = 0.5f;

    [SerializeField]private int shieldNum = 0;
    private void SS(int num)
    {
        var subV = 360 / num;
        var n = 0f;
        shieldNum = 0;
        for (int i = 0; i < num; i++)
        {
            SpawanShield(n);
            shieldNum++;
            n += subV;
        }
    }
    private void SpawanShield(float rangle)
    {
        var g = Instantiate(shield, transform.position, quaternion.identity);
        g.gameObject.SetActive(true);
        _shieldList.Add(g);
        var s = g.GetComponent<RotateTransform>();
        s.initAngle = rangle;
        s.centerObject = gameObject.transform;
        s.rotationSpeed = rotationShieldNumSpeed;
    }
    
    [Header("劍攻擊")] 
    [SerializeField]private float delaytime = 3f;
    [SerializeField]private float nowDelaytime;
    [SerializeField]private float sharpGrid = 0.5f;
    [SerializeField]private float nowsharpGrid;
    
    private void OnSword()
    {
        nowDelaytime += Time.deltaTime;
        if (nowDelaytime >= delaytime)
        {
            nowsharpGrid += Time.deltaTime;
            if (nowsharpGrid >= sharpGrid)
            {
                nowsharpGrid = 0;

                var n = 0;
                for (int i = 0; i < _swordList.Count; i++)
                {
                    if (_swordList[i].gameObject != null)
                    {
                        _animator.Play("Attack");
                        //當surround False 劍就會射出
                        _swordList[i].GetComponent<Sword>().surround = false;
                        break;
                    }
                    n++;
                    if (n == _swordList.Count)
                    {
                        spawanSword = false;
                        _swordList.Clear();
                        mods = Mods.Idle;
                        nowDelaytime = 0f;
                        nowsharpGrid = 0f;
                        return;
                    }
                }
            }
        }
    }
}
