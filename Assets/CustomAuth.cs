using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CustomAuth : MonoBehaviour
{
    AndroidJavaObject customAuth;
    
    void Start()
    {
        var args = new AndroidJavaObject("org.torusresearch.customauth.types.CustomAuthArgs",
            "https://scripts.toruswallet.io/redirect.html",
            1, //TorusNetwork.Testnet
            "torusapp://org.torusresearch.customauthandroid/redirect");
        customAuth = new AndroidJavaObject("org.torusresearch.customauth.CustomAuth");
    }

    void Update()
    {
        
    }
}
