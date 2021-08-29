using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameBehaviour : MonoBehaviour
{
    
    // Managers
    protected static PlayerController _PLAYER { get { return PlayerController.INSTANCE; } }
    protected static LockManager _LM { get { return LockManager.INSTANCE; } }

    protected static CameraController _CC { get { return CameraController.INSTANCE; } }
    protected static Pause _P { get { return Pause.INSTANCE; } }
}

public class Singleton<T> : GameBehaviour where T : MonoBehaviour
{
    private static T instance_;
    public static T INSTANCE
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = GameObject.FindObjectOfType<T>();
                if (instance_ == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    singleton.AddComponent<T>(); // Awake gets gets called called inside AddComponent
                }
            }
            return instance_;
        }
    }
    protected virtual void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this as T;
            //DontDestroyOnLoad (gameObject );
        }
        else
        {
            Destroy(gameObject);
        }
    }
}


