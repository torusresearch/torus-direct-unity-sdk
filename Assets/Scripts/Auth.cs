using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auth : MonoBehaviour
{
    private static AndroidJavaClass unityPlayer;

    private static AndroidJavaObject torusDirectPlugin;

    public static AndroidJavaClass UnityPlayer
    {
        get
        {
            if (unityPlayer == null)
            {
                unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            }
            return unityPlayer;
        }
    }

    public static AndroidJavaObject TorusDirectPlugin
    {
        get
        {
            if (torusDirectPlugin == null)
            {
                AndroidJavaClass cls = new AndroidJavaClass("org.torusresearch.unity.torusdirect.TorusDirectPlugin");
                torusDirectPlugin = cls.CallStatic<AndroidJavaObject>("getInstance");
            }
            return torusDirectPlugin;
        }
    }

    void Start()
    {
        Debug.Log("Auth started");

        if (Application.platform == RuntimePlatform.Android)
        {
            TorusDirectPlugin.Call("init", "<browser redirect URL>", "testnet", "<redirect URL>", UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"));
        }
        else
        {
            Debug.Log("Not Android");
        }
    }

    void Update()
    {

    }
}
