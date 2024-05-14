using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

namespace UI
{
    public class MenuVideio : MonoBehaviour
    {
        public bool videoPlay = true;
        
        public GameObject UI;
        public VideoClip v1;
        public VideoClip v2;

        public int vc = 1;
        
        private VideoPlayer _nowVideo;
        public Camera cameras;
        public bool play;

        private Vector3 _mousePosition;
        public float mouseNotMoveTime = 5f;
        public float nowMouseNotMoveTime;
        public bool mouseNotMoveIsTime;

        public float nextTime = 3f;
        public float nowNextTime;
        public bool next;

        [SerializeField]private TextMeshProUGUI svTest;
        private const string SvTestV1 = "待機影片 ( v1 ) :";
        private const string SvTestV2 = "待機影片 ( v2 ) :";
        

        public void SV(int s)
        {
            switch (s)
            {
                case 1:
                    svTest.text = SvTestV1;
                    break;
                case 2:
                    svTest.text = SvTestV2;
                    break;
            }
            vc = s;
        }
        private void Update()
        {
            if(!videoPlay)
                return;
            C();
            if (!play && mouseNotMoveIsTime && !next)
            { 
                play = true;
                next = true;
                _nowVideo = gameObject.AddComponent<VideoPlayer>();
                VideoClip v = null;
                switch (vc)
                {
                    case 1:
                        v = v1;
                        break;
                    case 2:
                        v = v2;
                        break;
                    default:
                        v = v1;
                        break;
                }
                _nowVideo.clip = v;
                _nowVideo.renderMode = VideoRenderMode.CameraNearPlane;
                _nowVideo.targetCamera = cameras;
                _nowVideo.Play();
                float time = (float)_nowVideo.clip.length;
                StartCoroutine(Play(time + 1));
                UI.SetActive(false);
            }
        }

        IEnumerator Play(float time)
        {
            float nowTime = 0f;
            while (nowTime <= time)
            {
                nowTime += Time.deltaTime;
                yield return null;
                if ((_nowVideo != null && nowTime >= 1f&& !_nowVideo.isPlaying)||!mouseNotMoveIsTime)
                {
                    play = false;
                    UI.SetActive(true);
                    Destroy(_nowVideo);
                    break;
                }
            }
            yield return Next();

        }

        IEnumerator Next()
        {
            float nowTime = 0f;
            while (nowTime <= nextTime)
            {
                nowTime += Time.deltaTime;
                yield return null;
                if (!mouseNotMoveIsTime)
                {
                    play = false;
                    UI.SetActive(true);
                    next = false;
                    break;
                }
            }
            next = false;
        }
        private void C()
        {
            nowMouseNotMoveTime += Time.deltaTime;
            if (Input.mousePosition != _mousePosition || Input.anyKeyDown||Input.GetAxis("Mouse ScrollWheel")!=0)
            {
                Debug.Log("鼠標移動了");
                mouseNotMoveIsTime = false;
                nowMouseNotMoveTime = 0f;
            }
            _mousePosition = Input.mousePosition;
            if (nowMouseNotMoveTime >= mouseNotMoveTime)
                mouseNotMoveIsTime = true;
        }
    }
}