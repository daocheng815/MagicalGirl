using UnityEngine;
namespace Daocheng.BehaviorTrees
{
    public class DistanceRangeTask : Task
    {
        private Move Move => Blackboard.Get<Move>(Names.Move);
        private SearchToPlayers SearchToPlayers => Blackboard.Get<SearchToPlayers>(Names.SearchToPlayers);
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            if (SearchToPlayers.distance < Move.eyeDistanceMax && SearchToPlayers.distance > Move.eyeDistanceMin)
            {
                return Status.Success;
            }
            Move.MoveDirection = Vector2.zero;
            return Status.Failure;
        }
    }
}