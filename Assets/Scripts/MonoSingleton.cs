using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError(typeof(T).ToString() + " in NUll");
                GameObject newSingletonObj = new GameObject(typeof(T).ToString());
                newSingletonObj.AddComponent<T>();

            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this as T;
    }

    public virtual void Init()
    {
        // optional to override
    }
}
