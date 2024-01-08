
using System;
using UnityEngine;

namespace Daocheng.BehaviorTrees.Conditionals
{
    public class BoolConditional : Conditionals
    {
        private Func<bool> _bc;
        public BoolConditional(Func<bool> bc)
        {
            _bc = bc;
        }
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            if (_bc.Invoke())
                return Status.Success;
            return Status.Failure;
        }
    }
}