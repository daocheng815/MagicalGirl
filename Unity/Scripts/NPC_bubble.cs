using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC_bubble : MonoBehaviour
{

    public UnityEvent bubbleCommand;
   
    public GameObject bubble;
    public bool istrue;
    
    public void bubble_show()
    { 
        bubble.SetActive(true);
        istrue = true;
    }
    public void bubble_hide()
    {
        bubble.SetActive(false);
        istrue = false;
    }

    private void Update()
    {
        if(bubbleCommand != null)
        {
            if(Input.GetKeyDown(KeyCode.Z) && istrue)
            {
                bubble_hide();
                bubbleCommand.Invoke();
            }

        }
    }
}
