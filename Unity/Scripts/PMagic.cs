using Events;
using UnityEngine;
// �]�k
public class PMagic : MonoBehaviour
{
    public int P_MaxMagic = 150;
    public int P_Magic = 150;
    private float o_Time = 0f;
    public float P_Time = 1f;
    public bool IsMagic(int nc)
    {
        return P_Magic > nc;
    }

    void Start()
    {
        //StartCoroutine(RegenerateMagic(2f));

    }

    private void Update()
    {
        MagicBar.Instance.MagicMax = P_MaxMagic;
        MagicBar.Instance.MagicCurrent = P_Magic;

        o_Time += Time.deltaTime;
        if(o_Time >= P_Time)
        {
            if (P_Magic < P_MaxMagic)
            {
                P_Magic += 1;
            }
            o_Time = 0f;
        }
    }


    public void OnMagic(int Mc)
    {
        UiEvents.ShowValUi.Invoke();
        if (P_Magic >= Mc)
        {
            P_Magic -= Mc;
        }
        else { P_Magic = 0; }
    }
   
}
