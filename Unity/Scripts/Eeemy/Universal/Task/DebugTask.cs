using UnityEngine;

namespace Daocheng.BehaviorTrees
{
    public class DebugTask : Task
    {
        private string text;
        public DebugTask(string text)
        {
            this.text = text;
        }
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            Debug.Log(text);
            return Status.Success;
        }
    }
}