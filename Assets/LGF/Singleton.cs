using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    private static T _instance;
    static object singletonLock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (singletonLock)
                {
                    if (_instance == null)
                        _instance = new T();
                }
            }

            return _instance;
        }
    }
}