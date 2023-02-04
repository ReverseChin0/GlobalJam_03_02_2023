using UnityEngine;

public class SingletonAsComponent<T> : MonoBehaviour where T : SingletonAsComponent<T>
{

    private static T __Instance;

    // Flag that protects us against on destroy time access
    private bool _alive = true;
    public static bool IsAlive
    {
        get
        {
            if (__Instance == null)
                return false;

            return __Instance._alive;
        }
    }

    protected static SingletonAsComponent<T> _Instance
    {
        get
        {
            if (!__Instance)
            {
                T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];
                if (managers != null)
                {
                    if (managers.Length == 1)
                    {
                        __Instance = managers[0];
                        return __Instance;
                    }
                    else
                    {
                        if (managers.Length > 1)
                        {
                            Debug.LogError("You have more than one " + typeof(T).Name + " in the sceme. You only need 1, it's a singleton!");
                            for (int i = 0; i < managers.Length; i++)
                            {
                                T manager = managers[i];
                                Destroy(manager.gameObject);
                            }
                        }
                    }
                }

                GameObject go = new GameObject(typeof(T).Name, typeof(T));
                __Instance = go.GetComponent<T>();

                //DontDestroyOnLoad (__Instance.gameObject);
            }

            return __Instance;
        }

        set
        {
            __Instance = value as T;
        }
    }

    /*
	 * If the singleton is destroy the toggle the _alive flag to false, so other classes
	 * can be aware of this and avoid any try to access to it.
	*/
    void OnDestroy()
    {
        _alive = false;
    }

    void OnApplicationQuit()
    {
        _alive = false;
    }
}