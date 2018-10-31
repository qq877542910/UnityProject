using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = System.Activator.CreateInstance<T>();
            }
            return instance;
        }
    }
}
