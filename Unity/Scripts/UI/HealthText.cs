using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    

    public Vector3 moveSpeed = new Vector3(0,75,0);
    public Vector3 randomSpawnOffset = new Vector3(0, 0, 0); // 隨機偏移位置
    public float timetofade = 1f;

    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;
    private float timeElapsed = 0f;
    private Color starttColor;


    // Start is called before the first frame update
    private void Awake()
    {

        
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        starttColor = textMeshPro.color;

        //隨機生成新的位置
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-randomSpawnOffset.x, randomSpawnOffset.x), UnityEngine.Random.Range(-randomSpawnOffset.y, randomSpawnOffset.y), 0);
        textTransform.position += randomOffset;
    }

    // Update is called once per frame
    void Update()
    {

        timeElapsed += Time.deltaTime;

        if (timeElapsed < timetofade)
        {
            float newAlpha = starttColor.a * (1 - (timeElapsed / timetofade));
            textMeshPro.color = new Color(starttColor.r, starttColor.g, starttColor.b, newAlpha); ;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
