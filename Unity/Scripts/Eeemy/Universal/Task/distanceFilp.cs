using UnityEngine;

namespace Daocheng.BehaviorTrees
{
    public class distanceFlip:Task
    {
        private SearchToPlayers SearchToPlayers => Blackboard.Get<SearchToPlayers>(Names.SearchToPlayers);
        private Move Move => Blackboard.Get<Move>(Names.Move);
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            if (!SearchToPlayers.distanceBools)
                Move.FilpDirection();
            return Status.Success;
        }
    }
}