using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Debug.Log("Destroying other instance");
            Destroy(FindObjectOfType<T>().gameObject);
        }
    }
}


public class SingletonPersistent<T> : MonoBehaviour
    where T : Component
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
        }
        else
        {
            Debug.Log("Destroying other instance");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(FindObjectOfType<T>());
    }
}