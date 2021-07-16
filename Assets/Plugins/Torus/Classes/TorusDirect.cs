using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Torus.Classes
{
    public class TorusDirect
    {

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TorusDirect_iOS_ShowAlert(string title, string message);
#else
        private static void TorusDirect_iOS_ShowAlert(string title, string message)
        {
            throw new Exception("TorusDirect: Calling iOS method in a non-iOS platform.");
        }
#endif

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
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                TorusDirect_iOS_ShowAlert("Couldn't login", "iOS platform is not supported. It's coming soon!");
            }
            else
            {
                Debug.LogWarning("TorusDirect: Unsupported platform");
            }
        }

        [Serializable]
        public class TriggerLoginParams
        {
            public string typeOfLogin;
            public string verifier;
            public string clientId;
            public TorusJWTParams jwtParams;
        }

        public static void TriggerLogin(TorusCallback callback, TorusTypeOfLogin typeOfLogin, string verifier, string clientId, TorusJWTParams jwtParams = null)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass cls = new AndroidJavaClass("org.torusresearch.unity.torusdirect.Plugin"))
                {
                    using (AndroidJavaObject plugin = cls.CallStatic<AndroidJavaObject>("getInstance"))
                    {
                        plugin.Call("triggerLogin", callback.gameObject.name, callback.method,
                            JsonUtility.ToJson(new TriggerLoginParams
                            {
                                typeOfLogin = GetTypeOfLoginString(typeOfLogin),
                                verifier = verifier,
                                clientId = clientId,
                                jwtParams = jwtParams
                            })
                        );
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                TorusDirect_iOS_ShowAlert("Couldn't login", "iOS platform is not supported. It's coming soon!");
            }
            else
            {
                Debug.LogWarning("TorusDirect: Unsupported platform");
            }
        }

        [Serializable]
        public class GetTorusKeyParams
        {
            public string verifier;
            public string verifierId;
            public TorusVerifierParams verifierParams;
            public string idToken;
        }

        public static void GetTorusKey(TorusCallback callback, string verifier, string verifierId, string idToken, TorusVerifierParams verifierParams = null)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass cls = new AndroidJavaClass("org.torusresearch.unity.torusdirect.Plugin"))
                {
                    using (AndroidJavaObject plugin = cls.CallStatic<AndroidJavaObject>("getInstance"))
                    {
                        TorusVerifierParams mergedVerifierParams = verifierParams != null ? verifierParams : new TorusVerifierParams();
                        if (string.IsNullOrEmpty(mergedVerifierParams.verifier_id)) mergedVerifierParams.verifier_id = verifierId;

                        plugin.Call("getTorusKey", callback.gameObject.name, callback.method,
                            JsonUtility.ToJson(new GetTorusKeyParams
                            {
                                verifier = verifier,
                                verifierId = verifierId,
                                verifierParams = mergedVerifierParams,
                                idToken = idToken
                            })
                        );
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                TorusDirect_iOS_ShowAlert("Couldn't login", "iOS platform is not supported. It's coming soon!");
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