using System.Collections;
using UnityEngine;

public class PlayerEffeect : MonoBehaviour
{
    GameObject particleGO;
    Animator animator;
    PlayerController playerController;
    TouchingDirections touchingDirections;
    // ²É¤l®ÄªG
    public GameObject particlePrefab;
    public GameObject particlePrefab2;
    public Transform spawnPoint;
    public Transform spawnPoint2;
    public bool onoff = false;
    public bool onoff2 =false;

    void Start()
    {
        touchingDirections = GetComponent<TouchingDirections>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private bool iseff = false;
    private bool isononff2on = false;
    // Update is called once per frame
    void Update()
    {
        if(onoff)
        {
            eff_1();
        }
        if(onoff2&& !isononff2on)
        {
            
            eff_2();
        }
    }
    void eff_2()
    {
        if ((animator.GetBool(AnimationStrings.isMoving) || (animator.GetBool(AnimationStrings.isRunning) && animator.GetBool(AnimationStrings.isMoving)) || animator.GetBool(AnimationStrings.Slide))
            && animator.GetBool(AnimationStrings.isGrounded) && !animator.GetBool(AnimationStrings.lockVelocity) && !animator.GetBool(AnimationStrings.attackTrigger))

        {
            GameObject particleGO = Instantiate(particlePrefab2, spawnPoint.position, Quaternion.identity);
            particleGO.transform.SetParent(gameObject.transform);
            isononff2on = true;

            if (!(((animator.GetBool(AnimationStrings.isMoving) || (animator.GetBool(AnimationStrings.isRunning) && animator.GetBool(AnimationStrings.isMoving)) || animator.GetBool(AnimationStrings.Slide))
            && animator.GetBool(AnimationStrings.isGrounded) && !animator.GetBool(AnimationStrings.lockVelocity) && !animator.GetBool(AnimationStrings.attackTrigger))))
            {
                Destroy(particleGO);
            }
        }
        else
        {
            isononff2on=false;
        }
           
    }
    void eff_1()
    {
        if (
            (animator.GetBool(AnimationStrings.isMoving) || (animator.GetBool(AnimationStrings.isRunning) && animator.GetBool(AnimationStrings.isMoving)) || animator.GetBool(AnimationStrings.Slide))
            && animator.GetBool(AnimationStrings.isGrounded) && !animator.GetBool(AnimationStrings.lockVelocity) && !animator.GetBool(AnimationStrings.attackTrigger))
        {
            if (!touchingDirections.IsOnwall)
            {
                if (!iseff)
                {
                    if (playerController.IsFacingRight)
                    {

                        StartCoroutine(effeect(0.25f));
                        StartCoroutine(effeect(0.3f));
                    }
                    else
                    {

                        StartCoroutine(effeectR(0.25f));
                        StartCoroutine(effeectR(0.3f));
                    }

                }
            }
        }
    }
    IEnumerator effeect(float delayInSeconds)
    {
        iseff = true;
        float timer = 0f;

        GameObject particleGO = Instantiate(particlePrefab, spawnPoint.position, Quaternion.identity);
        
        while (timer < delayInSeconds)
        {
            if(animator.GetBool(AnimationStrings.attackTrigger))
            {
                Destroy(particleGO);
            }

            timer += Time.deltaTime;
            yield return null;
        }
        iseff = false;
        Destroy(particleGO);
        
    }
    IEnumerator effeectR(float delayInSeconds)
    {
        iseff = true;
        float timer = 0f;

        Quaternion reverseRotation = Quaternion.Euler(0f, 180f, 0f);
        GameObject particleGO = Instantiate(particlePrefab, spawnPoint.position, reverseRotation);
        while (timer < delayInSeconds)
        {
            if (animator.GetBool(AnimationStrings.attackTrigger))
            {
                Destroy(particleGO);
            }

            timer += Time.deltaTime;
            yield return null;
        }
        iseff = false;
        Destroy(particleGO);

    }


}
