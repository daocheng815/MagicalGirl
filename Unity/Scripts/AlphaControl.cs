using System.Collections;
using UnityEngine;

public class AlphaControl : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float targetAlpha = 1.0f;
    float fadeSpeed = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void niplay()
    {
        Debug.Log("玩家進入");
        targetAlpha = 0.7f;
        StartCoroutine(FadeOut());
    }
    public void outplay()
    {
        Debug.Log("玩家離開");
        targetAlpha = 1.0f;
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {
        while (spriteRenderer.color.a < targetAlpha)
        {
            Color newColor = spriteRenderer.color;
            newColor.a += fadeSpeed / 2 * Time.deltaTime;
            spriteRenderer.color = newColor;
            yield return null;
        }
    }
    private IEnumerator FadeOut()
    {
        while (spriteRenderer.color.a > targetAlpha)
        {
            Color newColor = spriteRenderer.color;
            newColor.a -= fadeSpeed  * Time.deltaTime;
            spriteRenderer.color = newColor;
            yield return null;
        }
    }

}
