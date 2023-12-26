namespace State
{
    public class EnemyStateManager
    {
        //創建一個列舉來儲存所有狀態機
        public enum EnemyState
        {
            Idle,//等待
            Patrol,//巡邏
            Pursue,//追擊玩家
            Attack,//攻擊
            Hit,//受傷
            Dead,//死亡
            Jump//跳躍
        }
        
        //初始化當前狀態與前一個狀態
        private EnemyState currentState = EnemyState.Idle;
        public EnemyState previousState = EnemyState.Idle;

        //設定狀態機
        public EnemyState CurrentState
        {
            get { return currentState; }
            set
            {
                previousState = currentState;
                currentState = value;
            }
        }
        
        //初始化狀態
        public EnemyStateManager()
        {
            CurrentState = EnemyState.Idle;
        }
        
        //判定當前狀態是否是stateToCheck
        public bool IsCurrentState(string stateToCheck)
        {
            return currentState.ToString() == stateToCheck;
        }
        
        //取得當前狀態String
        public string GetCurrentStateString()
        {
            return currentState.ToString();
        }
    }
}