using UnityEngine;
using UnityEngine.Events;
namespace Events
{
    public abstract class GameMessageEvents
    {
        public static UnityAction<string, float> AddMessage;
    }
}