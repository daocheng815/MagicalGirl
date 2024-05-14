
namespace PlayAction
{
    /// <summary>
    /// 互動物件
    /// </summary>
    interface IPlayAction
    {
        public bool LockAction { get; set; }
        public int ActionCount { get; set; }
        public void Action();
    }
}
