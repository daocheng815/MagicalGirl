using System.Collections.Generic;
using UnityEngine;
namespace Daocheng.BehaviorTrees
{
    public class Blockboard
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();
        
        public T Get<T>(string key)
        {
            if (data.ContainsKey(key) && data[key] is T)
            {
                return (T)data[key];
            }

            return default(T);
        }

        public void Set<T>(string key, T value)
        {
            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }

    }
}