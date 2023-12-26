using UnityEngine;

public class PlayOneShotBehaviourLoop : StateMachineBehaviour
{
    public AudioClip soundToPlay;
    public float volume = 1f;
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;
    public bool loopSound = false;

    public float playDelay = 0.25f;
    private float timeSinceEntered = 0;
    private bool hasDelayedSoundPlayed = false;
    private bool isPlayingAnimation = false;

    private AudioSource audioSource;
    

    // OnStateEnter is called when a transition starts and the States machine starts to evaluate this States
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            audioSource = animator.gameObject.AddComponent<AudioSource>(); 
            audioSource.clip = soundToPlay;
            audioSource.volume = volume;
            audioSource.loop = loopSound; 
            audioSource.Play();
        }
        timeSinceEntered = 0f;
        hasDelayedSoundPlayed = false;
        isPlayingAnimation = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasDelayedSoundPlayed)
        {
            timeSinceEntered += Time.deltaTime;
            if (timeSinceEntered > playDelay)
            {
                audioSource = animator.gameObject.AddComponent<AudioSource>(); 
                audioSource.clip = soundToPlay;
                audioSource.volume = volume;
                audioSource.loop = loopSound;
                audioSource.Play();
                hasDelayedSoundPlayed = true;
            }
        }

        // Check if the animation is still playing
        isPlayingAnimation = !animator.IsInTransition(layerIndex);
    }

    // OnStateExit is called when a transition ends and the States machine finishes evaluating this States
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit && !isPlayingAnimation)
        {
            audioSource = animator.gameObject.AddComponent<AudioSource>(); 
            audioSource.clip = soundToPlay;
            audioSource.volume = volume;
            audioSource.loop = loopSound;
            audioSource.Play();
        }
        if (loopSound)
        {
            while (audioSource.volume <= 1f)
            {
                //Debug.Log(audioSource.volume);
                audioSource.volume -= Time.deltaTime/0.5f;
                if (audioSource.volume <= 0f)
                {
                    audioSource.volume = 0f;
                    audioSource.Stop();
                    Destroy(audioSource);
                    break;
                }
            }

        }
    }
}
