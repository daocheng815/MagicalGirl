using UnityEngine;
public class MathfManger : MonoBehaviour
{
    public static bool Approximately(float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) <= tolerance;
    }
}
