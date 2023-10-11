using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarW : MonoBehaviour
{

    public static float HeaithCurrent;
    public static float HeaithMax;
    private Image healtbar;

    // Start is called before the first frame update
    void Start()
    {
        healtbar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healtbar.fillAmount = HeaithCurrent / HeaithMax;
    }
}
