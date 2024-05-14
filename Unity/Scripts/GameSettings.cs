using Events;
using UnityEngine;


public class GameSettings : DontDestroySingleton<GameSettings>
{
    private const float InitTextSpeed = 0.01f;
    public float TextSpeed
    {
        get
        {
            if (!PlayerPrefs.HasKey("textSpeed"))
            {
                return InitTextSpeed;
            }
            return PlayerPrefs.GetFloat("textSpeed");
        }
        set => PlayerPrefs.SetFloat("textSpeed",value);
    }

    public string version = "Demo v0.0.0.2";
}
