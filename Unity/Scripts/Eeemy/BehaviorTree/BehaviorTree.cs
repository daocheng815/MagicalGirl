using System;
using UnityEngine;

namespace Ebt
{
    /// <summary>
    /// 行為樹節點基類
    /// </summary>
    public abstract class Node
    {
        public abstract bool Execute(GameObject obj);
    }
    /// <summary>
    /// 節點選擇器
    /// </summary>
    public class Selector:Node
    {
        private readonly Node[] nodes;

        public Selector(params Node[] nodes)
        {
            this.nodes = nodes;
        }

        public override bool Execute(GameObject obj)
        {
            foreach (var node in nodes)
            {
                if (node.Execute(obj))
                {
                    return true;//執行成功
                }
            }
            return false;//所有節點都執行失敗
        }
    }

    public class Sequence:Node
    {
        private readonly Node[] nodes;

        public Sequence(params Node[] nodes)
        {
            this.nodes = nodes;
        }

        public override bool Execute(GameObject obj)
        {
            foreach (var node in nodes)
            {
                if (!node.Execute(obj))
                {
                    return false; //有一個節點失敗，則整個序列失敗
                }
            }
            return true;//所有節點都成功
        }
    }
}