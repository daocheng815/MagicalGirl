using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    //跟隨物件 可以製作抓取敵人的 SKILL
    public Transform target;
    public Vector3 offset;
    public bool SetFollowObject = false;
    public bool Setoffset = false;
    public bool followOnlyX = false;
    public bool followOnlyY = false;

    public float smoothSpeed = 5.0f;

    public bool smooth = false;

    private void LateUpdate()
    {
        if (target != null && SetFollowObject)
        {
            Vector3 desiredPosition;

            if (Setoffset)
            {
                desiredPosition = target.position + offset;
            }
            else
            {
                desiredPosition = target.position;
            }

            
            if (followOnlyX)
            {
                desiredPosition.y = transform.position.y;
            }

            if (followOnlyY)
            {
                desiredPosition.x = transform.position.x;
            }


            if (smooth)
                transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            else
                transform.position = desiredPosition;

        }
    }
}
