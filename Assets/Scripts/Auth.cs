using System;
using UnityEngine;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{
    public Text accountText;

    void Awake()
    {
        TorusDirect.Init(
            browserRedirectUri: new Uri("https://scripts.toruswallet.io/redirect.html"),
            redirectUri: new Uri("torusapp://org.torusresearch.torusdirectandroid/redirect"),
            network: TorusNetwork.Testnet
        );
        DontDestroyOnLoad(gameObject);
    }

    public void OnClickGoogleLogin()
    {
        Debug.Log("Logging in with Google");
        TorusDirect.TriggerLogin(
            callbackGameObject: gameObject,
            callbackMethod: "OnPostLogin",
            typeOfLogin: TorusTypeOfLogin.google,
            verifier: "google-lrc",
            clientId: "221898609709-obfn3p63741l5333093430j3qeiinaa8.apps.googleusercontent.com"
        );
    }

    public void OnClickFacebookLogin()
    {
        Debug.Log("Logging in with Facebook");
        TorusDirect.TriggerLogin(
            callbackGameObject: gameObject,
            callbackMethod: "OnPostLogin",
            typeOfLogin: TorusTypeOfLogin.facebook,
            verifier: "facebook-lrc",
            clientId: "617201755556395"
        );
    }

    public void OnPostLogin(string message)
    {
        TorusResponse response = JsonUtility.FromJson<TorusResponse>(message);
        if (response.status == "fulfilled")
        {
            Debug.Log($"Login Succeeded: {response.value.publicAddress}");
            accountText.text = response.value.publicAddress;
        }
        else
        {
            Debug.Log($"Login Failed - {response.reason.name}: {response.reason.message}");
            accountText.text = "Login Failed";
        }
    }
}
