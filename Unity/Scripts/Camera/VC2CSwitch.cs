
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class VC2CSwitch : MonoBehaviour
{
    [FormerlySerializedAs("LeftVc2")] [FormerlySerializedAs("VC2_1")] [Header("左側")]
    public GameObject leftVc2;
    [FormerlySerializedAs("RightVc2")] [FormerlySerializedAs("VC2_2")] [Header("右側")]
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
            Debug.Log("從右側進入");
            _onEnter = OnEnters.Right;
        }
        else if (otherObjectPosition.x < thisObjectPosition.x)
        {
            // ?左??入
            Debug.Log("從左側進入");
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
            Debug.Log("從右側離開");
            leftVc2.SetActive(false);
            rightVc2.SetActive(true);
            //更新攝影機
            vc2C.CV = _cR;
            //更新攝影機序號
            vc2C.UpdateCameraNum();

        }
        else if (otherObjectPosition.x < thisObjectPosition.x && _onEnter == OnEnters.Right)
        {
            Debug.Log("從左側離開");
            leftVc2.SetActive(true);
            rightVc2.SetActive(false);
            //更新攝影機
            vc2C.CV = _cL;
            //更新攝影機序號
            vc2C.UpdateCameraNum();
        }

    }
}
