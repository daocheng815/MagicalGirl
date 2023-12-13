using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static float HeaithCurrent;
    public static float HeaithMax;
    private Image healtbar;

    public float decreaseSpeed = 10.0f; 

    private float currentHealth; 

    void Start()
    {
        healtbar = GetComponent<Image>();
        currentHealth = HeaithCurrent;
    }

    void Update()
    {
        currentHealth = Mathf.Lerp(currentHealth, HeaithCurrent, Time.deltaTime * decreaseSpeed);
        healtbar.fillAmount = currentHealth / HeaithMax;
    }

}
