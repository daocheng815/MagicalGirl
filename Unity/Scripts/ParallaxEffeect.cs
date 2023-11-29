using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// 視差特效
public class ParallaxEffeect : MonoBehaviour
{
    //引用攝影機的全域變數
    public Camera cam;
    //引用要跟隨的目標
    public Transform folllowTarget;
    public float canSpeed = 10f;
    //儲存物體的初始位置
    Vector2 startingPostition;
    //儲存物體的初始z軸
    float startingZ;

    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPostition;
    float zDistanceFromTarget => transform.position.z - folllowTarget.transform.position.z;
    
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane* canSpeed;
    // Start is called before the first frame update
    void Start()
    {
        startingPostition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPostition + camMoveSinceStart / parallaxFactor;
        transform.position = new Vector3(newPosition.x , newPosition.y , startingZ);
    }
}
