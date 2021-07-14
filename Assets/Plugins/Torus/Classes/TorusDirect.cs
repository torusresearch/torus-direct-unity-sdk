using System;
using UnityEngine;

namespace Torus
{
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
                case TorusTypeOfLogin.Google: return "google";
                case TorusTypeOfLogin.Facebook: return "facebook";
                case TorusTypeOfLogin.Reddit: return "reddit";
                case TorusTypeOfLogin.Discord: return "discord";
                case TorusTypeOfLogin.Twitch: return "twitch";
                case TorusTypeOfLogin.Github: return "github";
                case TorusTypeOfLogin.Apple: return "apple";
                case TorusTypeOfLogin.LinkedIn: return "linkedin";
                case TorusTypeOfLogin.Twitter: return "twitter";
                case TorusTypeOfLogin.Line: return "line";
                case TorusTypeOfLogin.EmailPassword: return "email_password";
                case TorusTypeOfLogin.JWT: return "jwt";
                default: throw new Exception("Unknown type of login.");
            }
        }

        public static TorusCallback Callback(GameObject gameObject, string method)
        {
            return new TorusCallback(gameObject, method);
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

        public static void TriggerLogin(TorusCallback callback, TorusTypeOfLogin typeOfLogin, string verifier, string clientId)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass cls = new AndroidJavaClass("org.torusresearch.unity.torusdirect.Plugin"))
                {
                    using (AndroidJavaObject plugin = cls.CallStatic<AndroidJavaObject>("getInstance"))
                    {
                        plugin.Call("triggerLogin", callback.gameObject.name, callback.method, GetTypeOfLoginString(typeOfLogin), verifier, clientId);
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
}