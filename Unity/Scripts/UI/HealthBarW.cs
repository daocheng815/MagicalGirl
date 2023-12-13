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
        if (HeaithCurrent / HeaithMax == 0)
        {
            healtbar.fillAmount = 0f;
        }
        else if (HeaithCurrent / HeaithMax <= 0.033f)
        {
            healtbar.fillAmount = 0.033f;
        }
        else
        {
            healtbar.fillAmount = HeaithCurrent / HeaithMax;
        }
        
    }
}
