using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileLaunch : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefad;
    public PMagic pmagic;

    public void FireProjectile()
    {
        pmagic.OnMagic(20);
        GameObject projectlie = Instantiate(projectilePrefad, launchPoint.position,projectilePrefad.transform.rotation * Quaternion.Euler(0, 0, -180));
        
        Vector3 origScale = projectlie.transform.localScale;

        projectlie.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1,
            origScale.y,
            origScale.z
            );
    }
}
