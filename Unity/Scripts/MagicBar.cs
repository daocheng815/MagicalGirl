using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{

    public static float MagicCurrent;
    public static float MagicMax;
    private Image healtbar;

    public float decreaseSpeed = 10.0f;

    private float currentMagic;

    void Start()
    {
        healtbar = GetComponent<Image>();
        currentMagic = MagicCurrent;
    }

    void Update()
    {
        currentMagic = Mathf.Lerp(currentMagic, MagicCurrent, Time.deltaTime * decreaseSpeed);
        healtbar.fillAmount = currentMagic / MagicMax;
    }

}

