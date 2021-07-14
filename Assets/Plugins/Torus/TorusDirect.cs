using System;
using UnityEngine;

public class TorusDirect
{
    public static string GetNetworkString(TorusNetwork network)
    {
        switch (network)
        {
            case TorusNetwork.Mainnet: return "mainnet";
            case TorusNetwork.Testnet: return "testnet";
            default: throw new Exception("Unknown network.");
        }
    }

    public static string GetTypeOfLoginString(TorusTypeOfLogin typeOfLogin)
    {
        switch (typeOfLogin)
        {
            case TorusTypeOfLogin.google: return "google";
            case TorusTypeOfLogin.facebook: return "facebook";
            case TorusTypeOfLogin.reddit: return "reddit";
            case TorusTypeOfLogin.discord: return "discord";
            case TorusTypeOfLogin.twitch: return "twitch";
            case TorusTypeOfLogin.github: return "github";
            case TorusTypeOfLogin.apple: return "apple";
            case TorusTypeOfLogin.linkedin: return "linkedin";
            case TorusTypeOfLogin.twitter: return "twitter";
            case TorusTypeOfLogin.line: return "line";
            case TorusTypeOfLogin.email_password: return "email_password";
            case TorusTypeOfLogin.jwt: return "jwt";
            default: throw new Exception("Unknown type of login.");
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
                    plugin.Call("init", browserRedirectUri.ToString(), GetNetworkString(network), redirectUri.ToString());
                }
            }
        }
        else
        {
            Debug.LogWarning("TorusDirect: Unsupported platform");
        }
    }

    public static void TriggerLogin(GameObject callbackGameObject, string callbackMethod, TorusTypeOfLogin typeOfLogin, string verifier, string clientId)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass cls = new AndroidJavaClass("org.torusresearch.unity.torusdirect.Plugin"))
            {
                using (AndroidJavaObject plugin = cls.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    plugin.Call("triggerLogin", callbackGameObject.name, callbackMethod, GetTypeOfLoginString(typeOfLogin), verifier, clientId);
                }
            }
        }
        else
        {
            Debug.LogWarning("TorusDirect: Unsupported platform");
        }
    }

    public static TorusCredentials ResumeAuth(string message)
    {
        TorusResponse response = JsonUtility.FromJson<TorusResponse>(message);
        if (response.status == "fulfilled")
        {
            return new TorusCredentials(response.value.privateKey, response.value.publicAddress);
        }
        else
        {
            switch (response.reason.name)
            {
                case "UserCancelledException":
                    throw new TorusUserCancelledException();
                case "NoAllowedBrowserFoundException":
                    throw new TorusNoAllowedBrowserFoundException();
                default:
                    throw new TorusException(response.reason.name, response.reason.message);
            }
        }
    }
}
