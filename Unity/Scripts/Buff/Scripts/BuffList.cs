using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New BuffList",menuName = "Buff/New BuffList")]
public class BuffList : ScriptableObject
{
    public List<Buff> buffs =new List<Buff>();
}