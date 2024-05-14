using System;
using SaveLord;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayAction
{
    public enum PlaySkill
    {
        DoubleJump,
        OnWallClimbIdle,
        Slide,
        OnRangeAttack,
        Run
    }
    public class PlaySkills:MonoBehaviour
    {

        public bool doubleJump
        {
            get => EventRecordManger.Instance.GetBoolVal("player_doubleJump",false);
            set => EventRecordManger.Instance.SetBoolVal("player_doubleJump", value);
        }
        public bool onWallClimbIdle
        {
            get => EventRecordManger.Instance.GetBoolVal("player_onWallClimbIdle",false);
            set => EventRecordManger.Instance.SetBoolVal("player_onWallClimbIdle", value);
        }

        public bool Slide
        {
            get => EventRecordManger.Instance.GetBoolVal("player_Slide",false);
            set => EventRecordManger.Instance.SetBoolVal("player_Slide", value);
        }
        public bool OnRangeAttack
        {
            get => EventRecordManger.Instance.GetBoolVal("player_OnRangeAttack",false);
            set => EventRecordManger.Instance.SetBoolVal("player_OnRangeAttack", value);
        }
        
        public bool Run
        {
            get => EventRecordManger.Instance.GetBoolVal("player_Run",false);
            set => EventRecordManger.Instance.SetBoolVal("player_Run", value);
        }
    }
}