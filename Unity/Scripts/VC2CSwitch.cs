using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VC2CSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ON = false;
    public GameObject VC2_1;
    public GameObject VC2_2;
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ee");
        VC2_1.SetActive(ON);
        VC2_2.SetActive(!ON);
        ON = !ON;
    }
}
