using System;
using System.Collections;
using currentLevel;
using UnityEngine;
using Unity.VisualScripting;

public class InLevel : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LevelState.LevelStateEnum myLevelState;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField] private float uIdelayTime = 3.5f;
    [SerializeField] private bool hasEntered = false;
    [SerializeField] private bool isTriggerEntered = false;
    public AVGSystem avgSystem;

    private void Start()
    {
        StartCoroutine(TriggerDelayTime(0.5f));
    }
    private IEnumerator TriggerDelayTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isTriggerEntered = true;
    }
    // 2DGame就要用2D的碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggerEntered)
        {
            if (!avgSystem.isOn)
            {
                if (hasEntered) return; 

                if ((layerMask.value & (1 << other.gameObject.layer)) != 0)
                {
                    Debug.Log(other.gameObject.name + "進入地圖");
                    StartCoroutine(DelayedAction(delayTime));
                    hasEntered = true; 
                }
            }
        }
    }
    private IEnumerator DelayedAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        CurrentLevel.Instance.levelState.CurrentState = myLevelState;
        CurrentLevel.Instance.OnUiShowAndHide(uIdelayTime);
        hasEntered = false;
    }
}
