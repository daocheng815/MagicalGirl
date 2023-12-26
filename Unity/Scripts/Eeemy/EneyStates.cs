namespace State
{
    //狀態機的所有子狀態
    internal class EneyStates
    {
        internal static readonly string Idle = EnemyStateManager.EnemyState.Idle.ToString();
        internal static readonly string Attack = EnemyStateManager.EnemyState.Attack.ToString();
        internal static readonly string Dead = EnemyStateManager.EnemyState.Dead.ToString();
        internal static readonly string Hit = EnemyStateManager.EnemyState.Hit.ToString();
        internal static readonly string Pursue = EnemyStateManager.EnemyState.Pursue.ToString();
        internal static readonly string Patrol = EnemyStateManager.EnemyState.Patrol.ToString();
        internal static readonly string Jump = EnemyStateManager.EnemyState.Jump.ToString();
    }
}


