using UnityEngine;
using UnityEngine.Events;

public class NPC_bubble : MonoBehaviour,INpcBubbleTeble
{

    public UnityEvent bubbleCommand;
   
    public GameObject bubble;
    public bool istrue;
    private bool IsCollision;

    private bool _isBubbleEnable = true;

    public bool IsBubbleEnable
    {
        get { return _isBubbleEnable; }
        set
        {
            if (!value)
                bubble_hide();
            _isBubbleEnable = value;
        }
    }

    public void bubble_show()
    {
        if (_isBubbleEnable)
        {
            bubble.SetActive(true);
            istrue = true;
        }
    }
    public void bubble_hide()
    {
        if (_isBubbleEnable)
        {
            bubble.SetActive(false);
            istrue = false;
        }
    }
    private void Update()
    {
        if(!_isBubbleEnable)
            return;
        if(bubbleCommand != null)
        {
            if(Input.GetKeyDown(KeyCode.Z) && istrue)
            {
                bubbleCommand.Invoke();
            }

        }
    }

}
