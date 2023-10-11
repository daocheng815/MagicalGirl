using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// ���t�S��
public class ParallaxEffeect : MonoBehaviour
{
    //�ޥ���v���������ܼ�
    public Camera cam;
    //�ޥέn���H���ؼ�
    public Transform folllowTarget;
    public float canSpeed = 10f;
    //�x�s���骺��l��m
    Vector2 startingPostition;
    //�x�s���骺��lz�b
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
