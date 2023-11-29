using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuDyeingVarUI : MonoBehaviour
{
    
    public Image var;
    void Start()
    {
        var.fillAmount =  PlayerVar.Instance.shuDyeingVar.var;
        //var.color = new Color();
    }
    void Update()
    {
        var.fillAmount = PlayerVar.Instance.shuDyeingVar.var;
    }
}
