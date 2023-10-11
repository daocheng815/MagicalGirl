using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{

    RectTransform rt;
    Vector3 originalPosition;
    Vector3 startPosition;
    float moveDuration = 1.0f; 

    
    public AnimationCurve speedCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    );

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        originalPosition = rt.position;
        startPosition = new Vector3(rt.position.x, Screen.height, rt.position.z);

        
        rt.position = startPosition;

       
        StartCoroutine(MoveWithSpeedCurve());
    }

   
    IEnumerator MoveWithSpeedCurve()
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = rt.position;

        while (elapsedTime < moveDuration)
        {
            float normalizedTime = elapsedTime / moveDuration;
            float speed = speedCurve.Evaluate(normalizedTime); 

            rt.position = Vector3.Lerp(initialPosition, originalPosition, normalizedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        
        rt.position = originalPosition;
    }
}
