
using System;
using Events;
using System.Collections.Generic;
using UnityEngine;

public class GameMessageManger : MonoBehaviour
{
    [SerializeField] private GameObject messagesPrefab;
    [SerializeField] private int maxQueue = 6;
    private readonly Queue<GameObject> _messagesQueue = new Queue<GameObject>();
    [SerializeField] private GameObject grid;
    private void OnEnable()
    {
        GameMessageEvents.AddMessage += AddMessage;
    }

    private void OnDisable()
    {
        GameMessageEvents.AddMessage -= AddMessage;
    }
    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Invoke("A1",0.07f + i);
            Invoke("A2",0.37f + i);
            Invoke("A3",0.53f + i);
        }
    }
    private void A1()
    {
        GameMessageEvents.AddMessage.Invoke("abb",3f);
    }
    private void A2()
    {
        GameMessageEvents.AddMessage.Invoke("12312313",3f);
    }
    private void A3()
    {
        GameMessageEvents.AddMessage.Invoke("½Ö§A°Ú??",3f);
    }
    public void AddMessage(string text,float destroyTime = 3f)
    {
        GameObject message = Instantiate(messagesPrefab, transform.position, Quaternion.identity);
        message.GetComponent<GameMessagePrefab>().UpdateMessageText(text);
        message.transform.SetParent(grid.transform);
        message.GetComponent<RectTransform>().localScale = Vector3.one;
        _messagesQueue.Enqueue(message);
        Invoke("DelayMessage",destroyTime);
    }
    private void DelayMessage()
    {
        
        if (_messagesQueue.Count > 0)
        {
            GameObject obj = _messagesQueue.Dequeue();
            if (obj != null)
            {
                obj.GetComponent<GameMessagePrefab>().DestroyOrFadeAnimator(0.5f);
            }
        }
    }
    private void Update()
    {
        if (_messagesQueue.Count > maxQueue)
        {
            DelayMessage();
        }
    }
}
