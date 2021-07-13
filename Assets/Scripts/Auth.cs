using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LoginResult
{
    public string privateKey;
    public string publicAddress;
}

[Serializable]
public class LoginException
{
    public string name;
    public string message;
}

[Serializable]
public class LoginResponse
{
    public string status;
    public LoginException reason = null;
    public LoginResult value = null;
}

public class Auth : MonoBehaviour
{
    public Text accountText;

    private static AndroidJavaObject torusDirectPlugin;

    public static AndroidJavaObject TorusDirectPlugin
    {
        get
        {
            if (torusDirectPlugin == null)
            {
                AndroidJavaClass cls = new AndroidJavaClass("org.torusresearch.unity.torusdirect.Plugin");
                torusDirectPlugin = cls.CallStatic<AndroidJavaObject>("getInstance");
            }
            return torusDirectPlugin;
        }
    }

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("TorusDirectPlugin.init");
            TorusDirectPlugin.Call("init",
                "https://scripts.toruswallet.io/redirect.html",
                "testnet",
                "torusapp://org.torusresearch.torusdirectandroid/redirect"
            );
        }

        Application.deepLinkActivated += onDeepLinkActivated;
        if (!String.IsNullOrEmpty(Application.absoluteURL))
        {
            onDeepLinkActivated(Application.absoluteURL);
        }
        DontDestroyOnLoad(gameObject);
    }

    void onDeepLinkActivated(string url)
    {
        Debug.Log($"Deep Link Activated: {url}");
    }

    public void OnClickGoogleLogin()
    {
        Debug.Log("Logging in with Google");
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("TorusDirectPlugin.triggerLogin");
            TorusDirectPlugin.Call("triggerLogin",
                gameObject.name,
                "OnPostLogin",
                "google",
                "google-lrc",
                "221898609709-obfn3p63741l5333093430j3qeiinaa8.apps.googleusercontent.com"
            );
        }
    }

    public void OnClickFacebookLogin()
    {
        Debug.Log("Logging in with Facebook");
    }

    public void OnPostLogin(string message)
    {
        LoginResponse response = JsonUtility.FromJson<LoginResponse>(message);
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
