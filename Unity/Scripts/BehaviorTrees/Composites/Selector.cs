using System.Linq;
using UnityEngine;
namespace Daocheng.BehaviorTrees
{
    /// <summary>
    /// 當有節點成功就返回成功
    /// </summary>
    public class Selector:Composites
    {
        public Selector(params Node[] nodes)
        {
            children = nodes;
        }
        protected override Status OnEvaluate(Transform agent,Blockboard blackboard)
        {
            bool isRunning = false;
            foreach (Node child in children)
            {
                Status s = child.Evaluate(agent, blackboard);
                switch (s)
                {
                    case Status.Running:
                        isRunning = true;
                        break;
                    case Status.Success:
                        return Status.Success;
                }
            }
            return isRunning ? Status.Running : Status.Failure;
        }
    }
}