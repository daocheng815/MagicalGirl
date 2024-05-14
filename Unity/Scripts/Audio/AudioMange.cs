using System;
using System.Collections;
using Events;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio
{
    public class AudioMange : DontDestroySingleton<AudioMange>
    {
        public GameObject player;
        public AudioClip pickUpSource;
        public AudioClip uiClick;
        public AudioClip openDoor;
        public AudioClip playFall;
        public AudioClip playSlide;
        
        public float allVolume = 1f;

        public void UiClick()
        {
            AudioPlay("uiClick",0.5f);
        }
        public AudioSource AudioPlay(string audioName,float audioVolume = 1f)
        {
            AudioClip audioClip = ReturnAudioClip(audioName);
            if(audioClip == null) return null;
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.volume = Mathf.Clamp(audioVolume * allVolume,0f,1f);
            audioSource.Play();
            StartCoroutine(Delay(audioSource));
            IEnumerator Delay(AudioSource audioSources)
            {
                yield return new WaitForSeconds(audioClip.length);
                Destroy(audioSources);
            }
            return audioSource;
        }
        
        [CanBeNull]
        private AudioClip ReturnAudioClip(string audioName)
        {
            AudioClip audioClip = null;
            switch (audioName)
            {
                case "pickUpSource" :
                    audioClip = pickUpSource;
                    break;
                case "uiClick" :
                    audioClip = uiClick;
                    break;
                case "openDoor" :
                    audioClip = openDoor;
                    break;
                case "playFall" :
                    audioClip = playFall;
                    break;
                case "playSlide" :
                    audioClip = playSlide;
                    break;
            }
            return audioClip;
        }
    }
}
