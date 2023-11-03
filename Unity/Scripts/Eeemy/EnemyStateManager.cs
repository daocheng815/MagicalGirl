namespace EnemyState
{
    public class EnemyStateManager
    {
        public enum EnemyState
        {
            Idle,//等待
            Patrol,//巡邏
            Pursue,//追擊玩家
            Attack,//攻擊
            Hit,//受傷
            Dead//死亡
        }

        private EnemyState currentState = EnemyState.Idle;

        public EnemyState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }
        
        public EnemyStateManager()
        {
            CurrentState = EnemyState.Idle;
        }
        
        public bool IsCurrentState(string stateToCheck)
        {
            return currentState.ToString() == stateToCheck;
        }
        
        public string GetCurrentStateString()
        {
            return currentState.ToString();
        }
    }
}