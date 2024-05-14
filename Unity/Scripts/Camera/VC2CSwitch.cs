
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class VC2CSwitch : MonoBehaviour
{
    [FormerlySerializedAs("LeftVc2")] [FormerlySerializedAs("VC2_1")] [Header("����")]
    public GameObject leftVc2;
    [FormerlySerializedAs("RightVc2")] [FormerlySerializedAs("VC2_2")] [Header("�k��")]
    public GameObject rightVc2;
    [FormerlySerializedAs("Vc2C")] public VC2C vc2C;

    private CinemachineVirtualCamera _cL;
    private CinemachineVirtualCamera _cR;

    private enum OnEnters { Left , Right }
    private OnEnters _onEnter = OnEnters.Left;
    private void Awake()
    {
        _cL = leftVc2.GetComponent<CinemachineVirtualCamera>();
        _cR = rightVc2.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        Vector2 otherObjectPosition = collision.transform.position;
        Vector2 thisObjectPosition = transform.position;
        if (otherObjectPosition.x > thisObjectPosition.x)
        {
            Debug.Log("�q�k���i�J");
            _onEnter = OnEnters.Right;
        }
        else if (otherObjectPosition.x < thisObjectPosition.x)
        {
            // ?��??�J
            Debug.Log("�q�����i�J");
            _onEnter = OnEnters.Left;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Vector2 otherObjectPosition = other.transform.position;
        Vector2 thisObjectPosition = transform.position;
        if (otherObjectPosition.x > thisObjectPosition.x && _onEnter == OnEnters.Left)
        {
            Debug.Log("�q�k�����}");
            leftVc2.SetActive(false);
            rightVc2.SetActive(true);
            //��s��v��
            vc2C.CV = _cR;
            //��s��v���Ǹ�
            vc2C.UpdateCameraNum();

        }
        else if (otherObjectPosition.x < thisObjectPosition.x && _onEnter == OnEnters.Right)
        {
            Debug.Log("�q�������}");
            leftVc2.SetActive(true);
            rightVc2.SetActive(false);
            //��s��v��
            vc2C.CV = _cL;
            //��s��v���Ǹ�
            vc2C.UpdateCameraNum();
        }

    }
}
