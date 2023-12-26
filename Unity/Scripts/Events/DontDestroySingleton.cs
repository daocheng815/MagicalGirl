using UnityEngine;

namespace Events
{
    /// <summary>
    /// 單例模式，且讓物件不會消失。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DontDestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // 單例實例
        private static T _instance;

        // 取得單例的方法
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 如果尚未建立實例，則尋找現有的實例
                    _instance = FindObjectOfType<T>();

                    // 如果仍然沒有找到，則創建一個新的實例
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return _instance;
            }
        }

        // 在 Awake 中執行 DontDestroyOnLoad
        private void Awake()
        {
            // 確保只有一個實例存在，並且不會在場景切換時被銷毀
            if (_instance == null || _instance == this)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // 如果存在其他實例，則銷毀這個物件
                Destroy(gameObject);
            }
        }

        // 在其他方法中使用 Singleton<T>.Instance 來訪問單例
    }
}    
        
