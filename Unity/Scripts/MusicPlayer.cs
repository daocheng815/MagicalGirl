using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource introSource, loopSource;
    public Damageable PlayerDamageable;


    void Start()
    {
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }

    private void Update()
    {
        if (!PlayerDamageable.IsAlive)
        {
            introSource.Stop();
            loopSource.Stop();
        }
    }
}
