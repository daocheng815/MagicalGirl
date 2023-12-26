using System;
using System.Collections;
using Events;
using JetBrains.Annotations;
using UnityEngine;
namespace Audio
{
    public class AudioMange : DontDestroySingleton<AudioMange>
    {
        public GameObject player;
        public AudioClip pickUpSource;
        public AudioClip uiClick;

        public float allVolume = 1f;

        public void UiClick()
        {
            AudioPlay("uiClick",0.5f);
        }
        public void AudioPlay(string audioName,float audioVolume = 1f)
        {
            AudioClip audioClip = ReturnAudioClip(audioName);
            if(audioClip == null) return;
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
        }
        
        [CanBeNull]
        private AudioClip ReturnAudioClip(string audioName)
        {
            AudioClip audioClip = null;
            if (audioName == "pickUpSource") audioClip = pickUpSource;
            if (audioName == "uiClick") audioClip = uiClick;
            return audioClip;
        }
    }
}
