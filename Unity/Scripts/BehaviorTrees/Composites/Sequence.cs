using UnityEngine;
namespace Daocheng.BehaviorTrees
{
    /// <summary>
    /// 當有節點失敗就返回失敗
    /// </summary>
    public class Sequence:Composites
    {
        public Sequence(params Node[] nodes)
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
                    case Status.Failure:
                        return Status.Failure;
                }
            }
            return isRunning ? Status.Running : Status.Success;
        }
    }
}