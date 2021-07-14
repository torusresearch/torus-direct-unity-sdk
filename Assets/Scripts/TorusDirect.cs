using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusDirect : MonoBehaviour
{
    public static string getNetworkString(TorusNetwork network)
    {
        switch (network)
        {
            case TorusNetwork.Mainnet:
                return "mainnet";
            case TorusNetwork.Testnet:
                return "testnet";
            default:
                throw new Exception("Unknown network.");
        }
    }

    public static void Init(Uri browserRedirectUri, TorusNetwork network, Uri redirectUri = null)
    {
        Uri mergedRedirectUri = redirectUri != null ? redirectUri : browserRedirectUri;
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass cls = new AndroidJavaClass("org.torusresearch.unity.torusdirect.Plugin"))
            {
                using (AndroidJavaObject plugin = cls.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    plugin.Call("init", browserRedirectUri.ToString(), getNetworkString(network), redirectUri.ToString());
                }
            }
        }
        else
        {
            Debug.LogWarning("TorusDirect: Unsupported platform");
        }
    }
}
