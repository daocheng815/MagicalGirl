using System;
using System.Collections;
using UnityEngine;

namespace Eeemy
{
    public abstract class EnemySate : MonoBehaviour
    {
        private Animator _animator;
        private Transform _transform;
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _transform = transform;
        }
        
        #region Animator
        public enum StateAnimator
        {
            Attack,
            Death,
            Hit,
            InIdle,
            InTrue,
            Idle,
        }

        private bool CurrentClipName(StateAnimator s)
        {
            return _animator.GetCurrentAnimatorStateInfo(0).IsName(s.ToString());
        }
        public StateAnimator _stateAnimator = StateAnimator.InIdle;
        private Coroutine _currentCoroutine;
        protected void ChangeAnimState(StateAnimator s ,Action animatorState = null,Action animatorEnd = null)
        {
            if (_stateAnimator == s && CurrentClipName(s)) return;
            animatorState?.Invoke();
            _stateAnimator = s;
            _animator.StopPlayback();
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _animator.Play(s.ToString());
            if(animatorEnd != null)
                _currentCoroutine = StartCoroutine(End());
            IEnumerator End()
            {
                yield return null;
                yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
                animatorEnd?.Invoke();
                _currentCoroutine = null;
            }
        }

        protected Coroutine WaitForAnimCoroutine;
        protected void WaitForAnim(StateAnimator stateAnimator,Action end = null)
        {
            if(WaitForAnimCoroutine != null)
                StopCoroutine(WaitForAnimCoroutine);
            WaitForAnimCoroutine = StartCoroutine(PlayAndWaitForAnimation(stateAnimator,end));
        }

        /// <summary>
        /// 等待撥放完動畫後切換狀態
        /// </summary>
        /// <param name="stateAnimator">動畫</param>
        /// <param name="s">狀態</param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected IEnumerator PlayAndWaitForAnimation(StateAnimator stateAnimator,Action end = null)
        {
            yield return null;
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            ChangeAnimState(stateAnimator);
            end?.Invoke();
        }
#endregion

#region State

        public enum State
        {
            Idle,
            Attack,
            Pursue,
            Death,
            Hit,
            
            FuryAttack,
            RunAway,
            QuickDash,//快速衝刺
        }
        public State state = State.Idle;

        protected bool CheckState(State s)
        {
            return state == s;
        }
        private bool _runState;
        private Coroutine _stateCoroutine;
        protected void ChangeState(State s,Action start = null,Action end = null)
        {
            if(state == s)
                return;
            start?.Invoke();
            _runState = false;
            Debug.Log($"狀態 {state.ToString()} 開始");
            if(_stateCoroutine != null)
                StopCoroutine(_stateCoroutine);
            if (!_runState)
                _stateCoroutine = StartCoroutine(RunState(end));
            state = s;
        }
        private IEnumerator RunState(Action end)
        {
            yield return null;
            _runState = true;
            yield return new WaitUntil((() => !_runState));
            Debug.Log($"狀態 {state.ToString()} 結束");
            end?.Invoke();
        }
#endregion

#region Direction
        protected enum WalkableDirection { Left, Right }
        protected WalkableDirection _walkDirection = WalkableDirection.Right;

        protected WalkableDirection WalkDirection
        {
            get => _walkDirection;
            set
            {
                Vector2 l = _transform.localScale;
                if (_walkDirection != value)
                    _transform.localScale = new Vector2(l.x * -1, l.y);
                _walkDirection = value;
            }
        }

        protected void FlipDirection()
        {
            if (WalkDirection == WalkableDirection.Right)
                WalkDirection = WalkableDirection.Left;
            else if (WalkDirection == WalkableDirection.Left)
                WalkDirection = WalkableDirection.Right;
        }
#endregion
    }
}