 using Events;
using SaveLord;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayAction
{

    public class SkillBook : MonoBehaviour,IPlayAction,ILordInterface
    {
        private AVGSystem _avgSystem;
        private PlaySkills _playSkills;
        public bool LockAction { get; set; }
        public int ActionCount { get; set; }

        private void Awake()
        {
            _avgSystem = GameObject.Find("AVGSystem").GetComponent<AVGSystem>();
            _playSkills = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlaySkills>();
        }

        public PlaySkill playSkills;
        public GameObject obj;
        public GameObject[] s_obj;
        public void Action()
        {
            switch (playSkills)
            {
                case PlaySkill.DoubleJump :
                    if (_avgSystem != null)
                        _avgSystem.Dramtic_emotion_4();
                    _playSkills.doubleJump = true;
                    GameMessageEvents.AddMessage("學習到二段跳躍，可以嘗試重複按下跳躍鍵",5f);
                    break;
                case PlaySkill.OnWallClimbIdle :
                    if (_avgSystem != null)
                        _avgSystem.Dramtic_emotion_4();
                    _playSkills.onWallClimbIdle = true;
                    GameMessageEvents.AddMessage("學習到攀牆，可以嘗試按下C鍵再向反方向跳，這樣可以跳的更高",5f);
                    break;
                case PlaySkill.Slide :
                    if (_avgSystem != null)
                        _avgSystem.Dramtic_emotion_4();
                    _playSkills.Slide = true;
                    GameMessageEvents.AddMessage("學習到快速移動，可以嘗試按下Ctrl鍵來高速的移動",5f);
                    break;
                case PlaySkill.OnRangeAttack :
                    if (_avgSystem != null)
                        _avgSystem.Dramtic_emotion_4();
                    _playSkills.OnRangeAttack = true;
                    GameMessageEvents.AddMessage("學習到射箭，現在嘗試看看對著靶射箭",5f);
                    break;
                case PlaySkill.Run :
                    if (_avgSystem != null)
                        _avgSystem.Dramtic_emotion_4();
                    _playSkills.Run = true;
                    GameMessageEvents.AddMessage("學習到奔跑，連續按下方向鍵會更快",5f);
                    break;
            }
            obj.SetActive(false);
            gameObject.SetActive(false);
        }

        public void Init()
        {
            switch (playSkills)
            {
                case PlaySkill.DoubleJump :
                    if (_playSkills.doubleJump)
                    {
                        obj.SetActive(false);
                        foreach (var g in s_obj)
                        {
                            g.SetActive(false);
                        }
                        gameObject.SetActive(false);
                    }
                    break;
                case PlaySkill.OnWallClimbIdle :
                    if (_playSkills.onWallClimbIdle)
                    {
                        obj.SetActive(false);
                        foreach (var g in s_obj)
                        {
                            g.SetActive(false);
                        }
                        gameObject.SetActive(false);
                    }
                    break;
                case PlaySkill.Slide :
                    if (_playSkills.Slide)
                    {
                        obj.SetActive(false);
                        foreach (var g in s_obj)
                        {
                            g.SetActive(false);
                        }
                        gameObject.SetActive(false);
                    }
                    break;
                case PlaySkill.OnRangeAttack :
                    if (_playSkills.OnRangeAttack)
                    {
                        obj.SetActive(false);
                        foreach (var g in s_obj)
                        {
                            g.SetActive(false);
                        }
                        gameObject.SetActive(false);
                    }
                    break;
                case PlaySkill.Run :
                    if (_playSkills.Run)
                    {
                        obj.SetActive(false);
                        foreach (var g in s_obj)
                        {
                            g.SetActive(false);
                        }
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
}