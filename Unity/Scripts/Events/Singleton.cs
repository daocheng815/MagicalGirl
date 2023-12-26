using UnityEngine;

//Singleton單例模式
namespace Events
{
    /// <summary>
    /// 單例模式，限定本場景
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
    
        private static T _instance;
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            if(_instance != null)
                Destroy(gameObject);
            else
                _instance = (T)this;
        }
        public static bool IsInitalized
        {
            get { return _instance != null; }
        }
        protected virtual void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }
    }
}
