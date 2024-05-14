using Audio;
using SaveLord;
using UnityEngine;

namespace PlayAction
{
    public class Door : MonoBehaviour,IPlayAction,ILordInterface
    {
        public GameObject door;

        [SerializeField] private AVGSystem avgSystem;
        private bool lockDoor
        {
            get => EventRecordManger.Instance.GetBoolVal(lookDoorName,true);
            set => EventRecordManger.Instance.SetBoolVal(lookDoorName, value);
        }
        
        [SerializeField] private string lookDoorName;
        
        [SerializeField] private bool lockAction = true;
        
        [SerializeField]private bool openDoor;
        public bool LockAction
        {
            get => lockAction;
            set => lockAction = value;
        }

        [SerializeField] private int actionCount;

        public int ActionCount
        {
            get => actionCount;
            set => actionCount = value;
        }

        private void Awake()
        {
            avgSystem = GameObject.Find("AVGSystem").GetComponent<AVGSystem>();
        }

        public void Init()
        {
            Debug.Log($"鎖狀態 : {lockDoor}");
            if (!lockDoor)
            {
                openDoor = false;
                door.SetActive(false);
            }
        }
        public void Action()
        {
            if (lockDoor)
            {
                if (invventoryManger.Instance.UsingKey(1, 8)||invventoryManger.Instance.UsingKey(0, 8))
                {
                    avgSystem.StartScript("OpenDoorKeyOn");
                    lockDoor = false;
                    door.SetActive(openDoor);
                    AudioMange.Instance.AudioPlay("openDoor",0.5f);
                    openDoor = !openDoor;
                    return;
                }
                if (!avgSystem.isDialog && actionCount == 1 )
                    avgSystem.StartScript("OpenDoorOne1");
                else if (!avgSystem.isDialog)
                    avgSystem.StartScript("OpenDoorOne");
            }
            else
            {
                AudioMange.Instance.AudioPlay("openDoor",0.5f);
                door.SetActive(openDoor);
                openDoor = !openDoor;
            }
        }
    }
}