using UnityEngine;
using UnityEngine.UI;
using Torus;
using Torus.Scripts;

public class LoginHandler : MonoBehaviour
{
    public Text accountText;

    public void onPreLogin()
    {
        accountText.text = "Logging in...";
    }

    public void OnPostLogin(TorusEvent e)
    {
        if (e.exception != null)
        {
            if (e.exception is TorusUserCancelledException)
            {
                accountText.text = "User cancelled!";
            }
            else if (e.exception is TorusNoAllowedBrowserFoundException)
            {
                accountText.text = "No allowed browser!";
            }
            else
            {
                accountText.text = "Something went wrong!";
            }

            Debug.Log($"Login failed: {e.exception}");
            return;
        }

        accountText.text = e.credentials.publicAddress;
        Debug.Log($"Login succeeded: {e.credentials.publicAddress}");
    }
}
