using System;
using UnityEngine;

namespace Daocheng.BehaviorTrees.Decorator
{
    public class Actions:Decorator
    {
        private Action _action;
        public Actions(Action action)
        {
            _action = action;
        }
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            _action.Invoke();
            return Status.Success;
        }
    }
}