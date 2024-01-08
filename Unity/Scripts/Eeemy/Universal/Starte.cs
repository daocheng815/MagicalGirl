using UnityEngine;

namespace Ebt
{
    public enum AnimatorStates
    {
        Idle,
        Attack,
        Dead,
    }

    public class Animat
    {
        private AnimatorStates _state;
        private Animator _animator;
        /// <summary>
        /// 構造函數，初始化_animator元件
        /// </summary>
        /// <param name="animator"></param>
        public Animat(Animator animator)
        {
            _animator = animator;
        }
        /// <summary>
        /// 轉換動畫狀態機，並且回傳動畫時間
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(AnimatorStates state)
        {
            _state = state;
            _animator.Play(_state.ToString());
        }
        /// <summary>
        /// 回傳當前動畫時間
        /// </summary>
        /// <returns></returns>
        public float? NowAnimatorTime()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(_state.ToString()))
            {
                return stateInfo.normalizedTime * stateInfo.length;
            }
            return null;
        }
    }
}