using UnityEngine;
using UnityEngine.Events;
public class CharacterEvents
{
    public static UnityAction<GameObject, int> characterDamaged;
    public static UnityAction<GameObject, int> characterCritDamaged;
    public static UnityAction<GameObject, int> characterHealed;
    public static UnityAction<GameObject, string> characterText;
    public static UnityAction<GameObject, string> characterShuDyeing;
    public static UnityAction<GameObject, string> characterCrit;
    public static UnityAction<GameObject, string> characterTextW;
}

