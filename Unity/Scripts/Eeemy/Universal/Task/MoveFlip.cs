using UnityEngine;

namespace Daocheng.BehaviorTrees
{
    public class MoveFlip : Task
    {
        private Move Move => Blackboard.Get<Move>(Names.Move);
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            
            var rb = agent.GetComponent<Rigidbody2D>();
            if (Move == null||rb == null)
                return Status.Failure;
            if (rb.velocity.x < 0) 
                Move.LeftToRight();
            else if (rb.velocity.x > 0 )
                Move.RightToLeft();
            return Status.Success;
        }
    }
}