using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auth : MonoBehaviour
{
    private static AndroidJavaObject torusDirectPlugin;

    public static AndroidJavaObject TorusDirectPlugin
    {
        get
        {
            if (torusDirectPlugin == null)
            {
                AndroidJavaClass cls = new AndroidJavaClass("org.torusresearch.unity.torusdirect.Plugin");
                torusDirectPlugin = cls.CallStatic<AndroidJavaObject>("getInstance");
            }
            return torusDirectPlugin;
        }
    }

    void Start()
    {
        Debug.Log("TorusDirectPlugin.init");
        if (Application.platform == RuntimePlatform.Android)
        {
            TorusDirectPlugin.Call("init",
                "https://scripts.toruswallet.io/redirect.html",
                "testnet",
                "torusapp://org.torusresearch.torusdirectandroid/redirect"
            );
        }
    }

    private float elapsedTime = 0f;
    private bool triggeredLogin = false;

    void Update()
    {
        if (triggeredLogin) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime > 10)
        {
            Debug.Log("TorusDirectPlugin.triggerLogin");
            if (Application.platform == RuntimePlatform.Android)
            {
                TorusDirectPlugin.Call("triggerLogin",
                    "google",
                    "google-lrc",
                    "221898609709-obfn3p63741l5333093430j3qeiinaa8.apps.googleusercontent.com"
                );
            }
            triggeredLogin = true;
        }
    }
}
