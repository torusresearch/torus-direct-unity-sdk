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
        public class SimpleLoginConfig
        {
            public string verifier;
            public string clientID;
        }

        [Serializable]
        public class DomainSpecificLoginConfig : SimpleLoginConfig
        {
            public string domain;
        }

        [Serializable]
        public class CustomLoginConfig : DomainSpecificLoginConfig
        {
            public string verifierIdField;
            public bool isVerifierIdCaseSensitive = true;
        }

        public static TorusCredentials credentials { get; private set; }
        public static Exception exception { get; private set; }

        public TorusNetwork network = TorusNetwork.Testnet;
        public string browserRedirectURL = "https://scripts.toruswallet.io/redirect.html";
        public string appRedirectURI = "torusapp://org.torusresearch.torusdirectandroid/redirect";

        public SimpleLoginConfig loginWithGoogle = new SimpleLoginConfig
        {
            verifier = "google-lrc",
            clientID = "221898609709-obfn3p63741l5333093430j3qeiinaa8.apps.googleusercontent.com"
        };
        public SimpleLoginConfig loginWithFacebook = new SimpleLoginConfig
        {
            verifier = "facebook-lrc",
            clientID = "617201755556395"
        };
        public SimpleLoginConfig loginWithTwitch = new SimpleLoginConfig
        {
            verifier = "twitch-lrc",
            clientID = "f5and8beke76mzutmics0zu4gw10dj"
        };
        public SimpleLoginConfig loginWithDiscord = new SimpleLoginConfig
        {
            verifier = "discord-lrc",
            clientID = "682533837464666198"
        };
        public DomainSpecificLoginConfig loginWithApple = new DomainSpecificLoginConfig
        {
            verifier = "torus-auth0-apple-lrc",
            clientID = "m1Q0gvDfOyZsJCZ3cucSQEe9XMvl9d9L",
            domain = "torus-test.auth0.com"
        };
        public DomainSpecificLoginConfig loginWithGithub = new DomainSpecificLoginConfig
        {
            verifier = "torus-auth0-github-lrc",
            clientID = "PC2a4tfNRvXbT48t89J5am0oFM21Nxff",
            domain = "torus-test.auth0.com"
        };
        public DomainSpecificLoginConfig loginWithLinkedin = new DomainSpecificLoginConfig
        {
            verifier = "torus-auth0-linkedin-lrc",
            clientID = "59YxSgx79Vl3Wi7tQUBqQTRTxWroTuoc",
            domain = "torus-test.auth0.com"
        };
        public DomainSpecificLoginConfig loginWithTwitter = new DomainSpecificLoginConfig
        {
            verifier = "torus-auth0-twitter-lrc",
            clientID = "A7H8kkcmyFRlusJQ9dZiqBLraG2yWIsO",
            domain = "torus-test.auth0.com"
        };
        public DomainSpecificLoginConfig loginWithLine = new DomainSpecificLoginConfig
        {
            verifier = "torus-auth0-line-lrc",
            clientID = "WN8bOmXKNRH1Gs8k475glfBP5gDZr9H1",
            domain = "torus-test.auth0.com"
        };
        public DomainSpecificLoginConfig loginWithEmailPassword = new DomainSpecificLoginConfig
        {
            verifier = "torus-auth0-email-password",
            clientID = "sqKRBVSdwa4WLkaq419U7Bamlh5vK1H7",
            domain = "torus-test.auth0.com"
        };
        public CustomLoginConfig loginWithEmailPasswordless = new CustomLoginConfig
        {
            verifier = "torus-auth0-passwordless",
            clientID = "P7PJuBCXIHP41lcyty0NEb7Lgf7Zme8Q",
            domain = "torus-test.auth0.com",
            verifierIdField = "name",
            isVerifierIdCaseSensitive = false
        };
        public CustomLoginConfig loginWithSMSPasswordless = new CustomLoginConfig
        {
            verifier = "torus-auth0-sms-passwordless",
            clientID = "nSYBFalV2b1MSg5b2raWqHl63tfH3KQa",
            domain = "torus-test.auth0.com",
            verifierIdField = "name",
            isVerifierIdCaseSensitive = false
        };

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

        public void TriggerLogin(TorusTypeOfLogin typeOfLogin, string verifier, string clientId, TorusJWTParams jwtParams = null)
        {
            __OnPreLogin__();
            TorusDirect.TriggerLogin(
                callback: TorusDirect.Callback(gameObject, "__OnPostLogin__"),
                typeOfLogin: typeOfLogin,
                verifier: verifier,
                clientId: clientId,
                jwtParams: jwtParams
            );
        }

        public void GetTorusKey(TorusCallback callback, string verifier, string verifierId, string idToken, TorusVerifierParams verifierParams = null)
        {
            __OnPreLogin__();
            TorusDirect.GetTorusKey(
                callback: TorusDirect.Callback(gameObject, "__onPostLogin__"),
                verifier: verifier,
                verifierId: verifierId,
                verifierParams: verifierParams,
                idToken: idToken
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

        public void LoginWithApple()
        {
            TriggerLogin(TorusTypeOfLogin.Apple, loginWithApple.verifier, loginWithApple.clientID, new TorusJWTParams { domain = loginWithApple.domain });
        }

        public void LoginWithGithub()
        {
            TriggerLogin(TorusTypeOfLogin.Github, loginWithGithub.verifier, loginWithGithub.clientID, new TorusJWTParams { domain = loginWithGithub.domain });
        }

        public void LoginWithLinkedIn()
        {
            TriggerLogin(TorusTypeOfLogin.LinkedIn, loginWithLinkedin.verifier, loginWithLinkedin.clientID, new TorusJWTParams { domain = loginWithLinkedin.domain });
        }

        public void LoginWithTwitter()
        {
            TriggerLogin(TorusTypeOfLogin.Twitter, loginWithTwitter.verifier, loginWithTwitter.clientID, new TorusJWTParams { domain = loginWithTwitter.domain });
        }

        public void LoginWithLine()
        {
            TriggerLogin(TorusTypeOfLogin.Line, loginWithLine.verifier, loginWithLine.clientID, new TorusJWTParams { domain = loginWithLine.domain });
        }

        public void LoginWithEmailPassword()
        {
            TriggerLogin(TorusTypeOfLogin.EmailPassword, loginWithEmailPassword.verifier, loginWithEmailPassword.clientID, new TorusJWTParams { domain = loginWithEmailPassword.domain });
        }

        public void LoginWithEmailPasswordless()
        {
            TriggerLogin(TorusTypeOfLogin.JWT, loginWithEmailPasswordless.verifier, loginWithEmailPasswordless.clientID, new TorusJWTParams
            {
                domain = loginWithEmailPasswordless.domain,
                verifierIdField = loginWithEmailPasswordless.verifierIdField,
                isVerifierIdCaseSensitive = loginWithEmailPasswordless.isVerifierIdCaseSensitive
            });
        }

        public void LoginWithSMSPasswordless()
        {
            TriggerLogin(TorusTypeOfLogin.JWT, loginWithSMSPasswordless.verifier, loginWithSMSPasswordless.clientID, new TorusJWTParams
            {
                domain = loginWithSMSPasswordless.domain,
                verifierIdField = loginWithSMSPasswordless.verifierIdField,
                isVerifierIdCaseSensitive = loginWithSMSPasswordless.isVerifierIdCaseSensitive
            });
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