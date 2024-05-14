using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


public class GameSettingsMenu : MonoBehaviour
{
    [BoxGroup("Version")][SerializeField]private TextMeshProUGUI version;
    
    private const string TextSpeedT1 = "文字跳過速度 ( 快速 ) :",TextSpeedT2 = "文字跳過速度 ( 普通 ) :",TextSpeedT3 = "文字跳過速度 ( 慢 ) :";
    [BoxGroup("Textspeed")][SerializeField] private TextMeshProUGUI textSpeedT;
    [BoxGroup("Textspeed")][SerializeField]private float textspeedT1s = 0.001f, textspeedT2s = 0.005f, textspeedT3s = 0.01f;
    
    private void Awake()
    {
        version.text = GameSettings.Instance.version;
        float currentTextSpeed = GameSettings.Instance.TextSpeed;
        if (currentTextSpeed == textspeedT1s)
            textSpeedT.text = TextSpeedT1;
        else if (currentTextSpeed == textspeedT2s)
            textSpeedT.text = TextSpeedT2;
        else if (currentTextSpeed == textspeedT3s)
            textSpeedT.text = TextSpeedT3;
    }

    public void OnTextSpeed(int s)
    {
        switch (s)
        {
            case 1:
                GameSettings.Instance.TextSpeed = textspeedT1s;
                textSpeedT.text = TextSpeedT1;
                break;
            case 2:
                GameSettings.Instance.TextSpeed = textspeedT2s;
                textSpeedT.text = TextSpeedT2;
                break;
            case 3:
                GameSettings.Instance.TextSpeed = textspeedT3s;
                textSpeedT.text = TextSpeedT3;
                break;
            default:
                GameSettings.Instance.TextSpeed = textspeedT3s;
                textSpeedT.text = TextSpeedT3;
                break;
        }
        Debug.Log($"{GameSettings.Instance.TextSpeed}");
    }
}
