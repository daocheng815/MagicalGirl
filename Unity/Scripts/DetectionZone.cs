using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DetectionZone : MonoBehaviour
{
    public UnityEvent NoCollidersRemain;
    public UnityEvent NoCollidersRemaEND;

    // °»´ú½d³ò
    public List<Collider2D> detectColliders = new List<Collider2D>();

    public bool setLayer = false;

    public LayerMask ISLayer;
    

    Collider2D col;


    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NoCollidersRemaEND.Invoke();

        if(setLayer)
        {

            if ((ISLayer & (1 << collision.gameObject.layer)) != 0)
            {
                //Debug.Log(collision);
                detectColliders.Add(collision);
            }
        }
        else
        {
            detectColliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectColliders.Remove(collision);

        if (detectColliders.Count <= 0 )
        {
            if (setLayer)
            { if ((ISLayer & (1 << collision.gameObject.layer)) != 0) { NoCollidersRemain.Invoke();} }
            else
            {NoCollidersRemain.Invoke(); }    
        }
    }
}
