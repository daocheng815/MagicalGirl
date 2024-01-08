using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ebt
{
    
//////////////////初始節點////////////////////////////////////////////////////////////////////////
    
    /// <summary>
    /// 行為樹節點基類
    /// </summary>
    public abstract class Node
    {
        public abstract bool Execute(GameObject obj,Blockboard blackboard);
    }
    /// <summary>
    /// 根節點
    /// </summary>
    public class Root : Node
    {
        private readonly Node childNode;

        public Root(Node childNode)
        {
            this.childNode = childNode;
        }

        public override bool Execute(GameObject obj,Blockboard blackboard)
        {
            // 根节点只执行其子节点
            return childNode.Execute(obj,blackboard);
        }
    }
    /// <summary>
    /// 葉節點
    /// </summary>
    public class Leaf : Node
    {
        private readonly Action<GameObject,Blockboard> action;

        public Leaf(Action<GameObject,Blockboard> action)
        {
            this.action = action;
        }

        public override bool Execute(GameObject obj,Blockboard blackboard)
        {
            // 叶子节点执行具体的动作
            action?.Invoke(obj,blackboard);
            return true; // 或者根据实际情况返回执行结果
        }
    }
    
//////////////////Composites組合節點////////////////////////////////////////////////////////////////////////
    
    /// <summary>
    /// 平行判斷只要以下節點一個成功就全部成功
    /// </summary>
    public class Parallel : Node
    {
        private Node[] children;

        public Parallel(params Node[] children)
        {
            this.children = children;
        }

        public override bool Execute(GameObject obj, Blockboard blackboard)
        {
            bool success = false;

            // Iterate through each child node
            foreach (Node child in children)
            {
                // Execute the child node
                bool childResult = child.Execute(obj, blackboard);

                // If any child succeeds, set the success flag to true and break the loop
                if (childResult)
                {
                    success = true;
                    break;
                }
            }

            // Return the overall result
            return success;
        }
    }
    /// <summary>
    /// 節點選擇器(按照順序執行下級節點)
    /// </summary>
    public class Selector:Node
    {
        private readonly Node[] nodes;

        public Selector(params Node[] nodes)
        {
            this.nodes = nodes;
        }

        public override bool Execute(GameObject obj,Blockboard blackboard)
        {
            foreach (var node in nodes)
            {
                if (node.Execute(obj,blackboard))
                {
                    return true;//執行成功
                }
            }
            return false;//所有節點都執行失敗
        }
    }
    /// <summary>
    /// 順序條件節點(下級節點若有失敗就退出)
    /// </summary>
    public class Sequence:Node
    {
        private readonly Node[] nodes;

        public Sequence(params Node[] nodes)
        {
            this.nodes = nodes;
        }

        public override bool Execute(GameObject obj,Blockboard blackboard)
        {
            foreach (var node in nodes)
            {
                if (!node.Execute(obj,blackboard))
                {
                    return false; //有一個節點失敗，則整個序列失敗
                }
            }
            return true;//所有節點都成功
        }
    } 
    
//////////////////Decorator 裝飾節點////////////////////////////////////////////////////////////////////////
    
    /// <summary>
    /// 循環行為樹
    /// </summary>
    public class BehaviorTreeLoop : Node
    {
        private readonly Node rootNode;
        private readonly Func<bool> OnConditionMet;
        public BehaviorTreeLoop(Node rootNode, Func<bool> onConditionMet)
        {
            this.rootNode = rootNode;
            OnConditionMet = onConditionMet;
        }

        public override bool Execute(GameObject obj,Blockboard blackboard)
        {
            while (ShouldContinueLoop())
            {
                rootNode.Execute(obj,blackboard);
                
            }
            
            return true;
        }
        
        private bool ShouldContinueLoop()
        {
            return OnConditionMet.Invoke();
        }
    }
    /// <summary>
    /// 需要延遲時間的節點
    /// </summary>
    public class LimitedFrequencyNode : Node
    {
        private readonly Node childNode;
        private readonly float frequency;
        private float timer;

        public LimitedFrequencyNode(Node childNode, float frequency)
        {
            this.childNode = childNode;
            this.frequency = frequency;
            this.timer = 0f;
        }

        public override bool Execute(GameObject obj,Blockboard blackboard)
        {
            timer += Time.deltaTime;
            if (timer >= frequency)
            {
                bool result = childNode.Execute(obj,blackboard);
                timer = 0f;
                return result;
            }
            return false;
        }
    } 
    
//////////////////Conditional條件節點////////////////////////////////////////////////////////////////////////
    
    /// <summary>
    /// 條件節點
    /// </summary>
    public class ConditionNode : Node
    {
        private readonly Func<GameObject, Blockboard, bool> condition;
        public Action<int> OnConditionMet;

        public ConditionNode(Func<GameObject, Blockboard, bool> condition)
        {
            this.condition = condition;
        }
        public override bool Execute(GameObject monster, Blockboard blackboard)
        {
            if (condition(monster, blackboard))
                OnConditionMet?.Invoke(10);
            return condition(monster, blackboard);
        }
    }

//////////////////Actions節點////////////////////////////////////////////////////////////////////////
    
    /// <summary>
    /// 測試行為樹
    /// </summary>
    public class ContTest : Node
    {
        private readonly string text;
        
        public override bool Execute(GameObject obj,Blockboard blackboard)
        {
            Debug.Log(text);
            return true;
        }
    }
    /// <summary>
    /// 顏色修改，測試
    /// </summary>
    public class ContColor : Node
    {
        private SpriteRenderer sr;
        private Color color;
        public ContColor(SpriteRenderer sr,Color color)
        {
            this.sr = sr;
            this.color = color;
        }
        public override bool Execute(GameObject obj,Blockboard blackboard)
        {
            //DebugTask.Log(blackboard.Get<String>("Ts"));
            sr.color = color;
            return true;
        }       
    }
    /// <summary>
    /// 巡路並記錄在黑板
    /// </summary>
    public class FindPath : Node
    {
        private Func<Vector3> getStartPos;
        private Func<Vector3> getEndPos;
        public FindPath(Func<Vector3> getStartPos, Func<Vector3> getEndPos)
        {
            this.getStartPos = getStartPos;
            this.getEndPos = getEndPos;
        }
        public override bool Execute(GameObject obj, Blockboard blackboard)
        {
            // 使用 getStartPos.Invoke() 和 getEndPos.Invoke() 获取巡逻的开始和结束位置
            var node = SetAstartTileMap.Instance.FastFindPath(getStartPos.Invoke(), getEndPos.Invoke());
            if (node == null) return false;
            List<Vector2> nodes = SetAstartTileMap.Instance.NodesCtoW(node);
            blackboard.Set("nodes",nodes);
            return true;
        }
    }
}