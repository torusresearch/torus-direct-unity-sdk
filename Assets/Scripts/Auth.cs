using System;
using UnityEngine;
using UnityEngine.UI;
using Torus;

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
    }

    public void OnClickGoogleLogin()
    {
        Debug.Log("Logging in with Google");

        accountText.text = "Logging in...";
        TorusDirect.TriggerLogin(
            callback: TorusDirect.Callback(gameObject, "OnPostLogin"),
            typeOfLogin: TorusTypeOfLogin.Google,
            verifier: "google-lrc",
            clientId: "221898609709-obfn3p63741l5333093430j3qeiinaa8.apps.googleusercontent.com"
        );
    }

    public void OnClickFacebookLogin()
    {
        Debug.Log("Logging in with Facebook");

        accountText.text = "Logging in...";
        TorusDirect.TriggerLogin(
            callback: TorusDirect.Callback(gameObject, "OnPostLogin"),
            typeOfLogin: TorusTypeOfLogin.Facebook,
            verifier: "facebook-lrc",
            clientId: "617201755556395"
        );
    }

    public void OnPostLogin(string message)
    {
        try
        {
            TorusCredentials credentials = TorusDirect.ResumeAuth(message);
            accountText.text = credentials.publicAddress;
            Debug.Log($"Login succeeded: {credentials.publicAddress}");
        }
        catch (TorusUserCancelledException)
        {
            accountText.text = "User cancelled!";
        }
        catch (TorusNoAllowedBrowserFoundException)
        {
            accountText.text = "No allowed browser!";
        }
        catch (TorusException e)
        {
            accountText.text = "Something went wrong!";
            Debug.Log($"Login failed: {e}");
        }
    }
}