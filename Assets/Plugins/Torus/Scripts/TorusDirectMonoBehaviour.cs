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
        [Serializable]
        public class LoginConfig
        {
            public string verifier;
            public string clientID;
            public LoginConfig(string verifier, string clientID)
            {
                this.verifier = verifier;
                this.clientID = clientID;
            }
        }

        public static TorusCredentials credentials { get; private set; }
        public static Exception exception { get; private set; }

        public TorusNetwork network = TorusNetwork.Testnet;
        public string browserRedirectURL = "https://scripts.toruswallet.io/redirect.html";
        public string appRedirectURI = "torusapp://org.torusresearch.torusdirectandroid/redirect";

        public LoginConfig loginWithGoogle = new LoginConfig("google-lrc", "221898609709-obfn3p63741l5333093430j3qeiinaa8.apps.googleusercontent.com");
        public LoginConfig loginWithFacebook = new LoginConfig("facebook-lrc", "617201755556395");
        public LoginConfig loginWithTwitch = new LoginConfig("twitch-lrc", "f5and8beke76mzutmics0zu4gw10dj");
        public LoginConfig loginWithDiscord = new LoginConfig("discord-lrc", "682533837464666198");

        public UnityEvent onPreLogin;
        public UnityEvent<TorusEvent> onPostLogin;

        public virtual void Awake()
        {
            TorusDirect.Init(
                browserRedirectUri: new Uri(browserRedirectURL),
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
            TriggerLogin(TorusTypeOfLogin.Google, loginWithGoogle.verifier, loginWithGoogle.clientID);
        }

        public void LoginWithFacebook()
        {
            TriggerLogin(TorusTypeOfLogin.Facebook, loginWithFacebook.verifier, loginWithFacebook.clientID);
        }

        public void LoginWithTwitch()
        {
            TriggerLogin(TorusTypeOfLogin.Twitch, loginWithTwitch.verifier, loginWithTwitch.clientID);
        }

        public void LoginWithDiscord()
        {
            TriggerLogin(TorusTypeOfLogin.Discord, loginWithDiscord.verifier, loginWithDiscord.clientID);
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