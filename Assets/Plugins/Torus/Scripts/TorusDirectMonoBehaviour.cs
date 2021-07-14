using System;
using UnityEngine;
using UnityEngine.Events;
using Torus.Classes;

namespace Torus
{
    public class TorusEvent
    {
        public TorusCredentials credentials { get; }
        public Exception exception { get; }

        public TorusEvent(TorusCredentials credentials, Exception exception)
        {
            this.credentials = credentials;
            this.exception = exception;
        }
    }

    public class TorusDirectMonoBehaviour : MonoBehaviour
    {
        public static TorusCredentials credentials { get; private set; }
        public static Exception exception { get; private set; }

        public TorusNetwork network = TorusNetwork.Testnet;
        public string browserRedirectURI = "https://scripts.toruswallet.io/redirect.html";
        public string appRedirectURI = "torusapp://org.torusresearch.torusdirectandroid/redirect";
        public string googleVerifier = "google-lrc";
        public string googleClientID = "221898609709-obfn3p63741l5333093430j3qeiinaa8.apps.googleusercontent.com";
        public string facebookVerifier = "facebook-lrc";
        public string facebookClientID = "617201755556395";

        public UnityEvent onPreLogin;
        public UnityEvent<TorusEvent> onPostLogin;

        public virtual void Awake()
        {
            TorusDirect.Init(
                browserRedirectUri: new Uri(browserRedirectURI),
                redirectUri: string.IsNullOrEmpty(appRedirectURI) ? null : new Uri(appRedirectURI),
                network: network
            );
        }

        public void TriggerLogin(TorusTypeOfLogin typeOfLogin, string verifier, string clientId)
        {
            __OnPreLogin__();
            TorusDirect.TriggerLogin(
                callback: TorusDirect.Callback(gameObject, "__OnPostLogin__"),
                typeOfLogin: typeOfLogin,
                verifier: verifier,
                clientId: clientId
            );
        }

        public void LoginWithGoogle()
        {
            TriggerLogin(TorusTypeOfLogin.Google, googleVerifier, googleClientID);
        }

        public void LoginWithFacebook()
        {
            TriggerLogin(TorusTypeOfLogin.Facebook, facebookVerifier, facebookClientID);
        }

        public void __OnPreLogin__()
        {
            onPreLogin.Invoke();
        }

        public void __OnPostLogin__(string message)
        {
            TorusCredentials localCredentials = null;
            Exception localException = null;
            try
            {
                localCredentials = TorusDirect.ResumeAuth(message);
            }
            catch (Exception e)
            {
                localException = e;
            }
            finally
            {
                credentials = localCredentials;
                exception = localException;
                onPostLogin.Invoke(new TorusEvent(localCredentials, localException));
            }
        }
    }
}