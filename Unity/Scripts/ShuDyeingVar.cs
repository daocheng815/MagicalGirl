using UnityEngine;
public class ShuDyeingVar : MonoBehaviour
{
    public int filthyVar;
    public float var => (float)(maxFilthyVar - filthyVar) / maxFilthyVar;
    [SerializeField]private int maxFilthyVar = 100;
    [SerializeField]private int minFilthyVar = 0;
    
    public void FilthyAdd(int num)
    {
        filthyVar += num;
        if(filthyVar <= minFilthyVar)
            filthyVar = minFilthyVar;
        else if(filthyVar >= maxFilthyVar)
            filthyVar = maxFilthyVar;
    }

    public void FilthySub(int num)
    {
        filthyVar -= num;
        if(filthyVar <= minFilthyVar)
            filthyVar = minFilthyVar;
        else if(filthyVar >= maxFilthyVar)
            filthyVar = maxFilthyVar;
    }
    
    public bool TestFilthyVar(int num)
    {
        return filthyVar == num;
    }
}
