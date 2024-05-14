using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class VC2CSwitchUD : MonoBehaviour
{
    [FormerlySerializedAs("TopVc2")] [FormerlySerializedAs("VC2_1")] [Header("�W��")]
    public GameObject topVc2;
    [FormerlySerializedAs("BottomVc2")] [FormerlySerializedAs("VC2_2")] [Header("�U��")]
    public GameObject bottomVc2;
    [FormerlySerializedAs("Vc2C")] public VC2C vc2C;

    private CinemachineVirtualCamera _cT;
    private CinemachineVirtualCamera _cB;

    private enum OnEnters { Top, Bottom }
    private OnEnters _onEnter = OnEnters.Top;
    private void Awake()
    {
        _cT = topVc2.GetComponent<CinemachineVirtualCamera>();
        _cB = bottomVc2.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        Vector2 otherObjectPosition = collision.transform.position;
        Vector2 thisObjectPosition = transform.position;
        if (otherObjectPosition.y > thisObjectPosition.y)
        {
            Debug.Log("�q�W��i�J");
            _onEnter = OnEnters.Top;
        }
        else if (otherObjectPosition.y < thisObjectPosition.y)
        {
            Debug.Log("�q�U��i�J");
            _onEnter = OnEnters.Bottom;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Vector2 otherObjectPosition = other.transform.position;
        Vector2 thisObjectPosition = transform.position;
        if (otherObjectPosition.y > thisObjectPosition.y && _onEnter == OnEnters.Bottom)
        {
            Debug.Log("�q�W�����}");
            topVc2.SetActive(false);
            bottomVc2.SetActive(true);
            //��s��v��
            vc2C.CV = _cB;
            //��s��v���Ǹ�
            vc2C.UpdateCameraNum();
        }
        else if (otherObjectPosition.y < thisObjectPosition.y && _onEnter == OnEnters.Top)
        {
            Debug.Log("�q�U�����}");
            topVc2.SetActive(true);
            bottomVc2.SetActive(false);
            //��s��v��
            vc2C.CV = _cT;
            //��s��v���Ǹ�
            vc2C.UpdateCameraNum();
        }
    }
}