using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class 
    Detection : MonoBehaviour
{
    [SerializeField]private bool rayShow = true;

    [SerializeField]private float scaleXOffset = 1f;
    [SerializeField]private float rayLengthRight = 1.0f;
    [SerializeField]private float rayLengthDown = 2.0f;
    [SerializeField]private float VerticalrayLengthDown = 1.0f;
    [SerializeField]private LayerMask wallLayer; 
    
    //���o½���V
    private float scaleX => transform.localScale.x;

    //�I���g�u���V�q
    private Vector2 _rayDirectionRight ;
    private Vector2 _rayDirectionDown ;
    //������a�����V�q
    private Vector2 _rayDirectionDownVertical;
    //�����V�q
    private Vector2 VectorHorizontal => new Vector2(0, 1);
    //2D�I��
    private RaycastHit2D _hitRight;
    private RaycastHit2D _hitDown;
    
    //���A
    public bool isSlope;
    public bool isWall;
    
    private void Awake()
    {
        _rayDirectionDown = new Vector2(0, -1f);
    }

    private void FixedUpdate()
    {
        //���
        _rayDirectionRight = new Vector2(scaleX, 0f);
        var position = new Vector2(transform.position.x  ,transform.position.y+ scaleXOffset) ;
        Debug.DrawRay(position, _rayDirectionRight * rayLengthRight, Color.blue);
        _hitRight = Physics2D.Raycast(position, _rayDirectionRight, rayLengthRight, wallLayer);
        if (_hitRight.collider != null)
        {
            if(rayShow)
                Debug.Log("�I��������I");
        }
        //�P�w�O�I�������
        isWall = _hitRight.collider;
        //�������a������m
        Debug.DrawRay(position, _rayDirectionDown * rayLengthDown, Color.red);
        _hitDown = Physics2D.Raycast(position, _rayDirectionDown, rayLengthDown, wallLayer);
        //�I�����ܧ����H���󪺪k�u(normal)�V�q�A�]��n�����ثe�����⩳�U���ײv�C
        if (_hitDown.collider != null)
        {
            _rayDirectionDownVertical = _hitDown.normal;
        }
        var newPosition = new Vector2(transform.position.x , transform.position.y + VerticalrayLengthDown) ;
        Debug.DrawRay(newPosition, _rayDirectionDownVertical * rayLengthDown, Color.yellow);
        //�P�w�O�_������a���W
        isSlope = _rayDirectionDownVertical == VectorHorizontal;
        
        // �o�ӧP�w�V�q���覡�i�H�Q�Φb�\�h���a��A�Ҧp:�שY�ɲ��ʪ��V�q�A���ثe�٨S�s�@�C
    }
    
}

