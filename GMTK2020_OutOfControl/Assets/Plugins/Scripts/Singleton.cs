using UnityEngine;

[DisallowMultipleComponent]
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //=====================================================================================================================//
    //=================================================== Private Fields ==================================================//
    //=====================================================================================================================//

    #region Private Fields

    protected static T instance;

    private static readonly object _lock = new Object();
    //protected static bool applicationIsQuitting;

    #endregion

    //=====================================================================================================================//
    //=================================================== Public Fields ===================================================//
    //=====================================================================================================================//

    #region Public Fields

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (instance != null)
                    return instance;

                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    var prefab = Resources.Load<GameObject>("Control/" + typeof(T));
                    instance = prefab != null
                        ? Instantiate(prefab).GetComponent<T>()
                        : new GameObject(typeof(T).ToString()).AddComponent<T>();
                }

                if (Application.isPlaying)
                    DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    #endregion

    //=====================================================================================================================//
    //=============================================== Unity Event Methods =================================================//
    //=====================================================================================================================//

    #region Unity Event Functions

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
            Destroy(gameObject);
    }

    protected virtual void Initialize()
    {
    }

    #endregion
}