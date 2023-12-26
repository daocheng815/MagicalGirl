
using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{

    public float fadeTime = 0.5f;
    public float fadDelelay = 0.0f;
    private float timeElapsed = 0f;
    private float fadeDelayElapsed = 0f;
    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;

    // OnStateEnter is called when a transition starts and the States machine starts to evaluate this States
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        spriteRenderer =animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        objToRemove = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

    // 進入這個動畫後持續更新以下程式
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 物件更新透明度(Alpha)直到為0，時間(fadeTime)結束後Destroy物件
        if(fadDelelay < fadeDelayElapsed) 
        {
            fadeDelayElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed += Time.deltaTime;

            float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime));
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            // Debug.Log(newAlpha);
            if (timeElapsed > fadeTime)
            {
                Destroy(objToRemove);
            }
        }
        
    }

    
}
