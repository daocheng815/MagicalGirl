
public class CSingleton<T> where T : new()
{
    private static T instance;
    private static readonly object lockObject = new object();

    public static T Instance
    {
        get
        {
            lock (lockObject)
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }
    }

    protected CSingleton() { }
}