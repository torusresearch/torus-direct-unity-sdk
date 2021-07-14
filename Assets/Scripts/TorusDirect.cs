using UnityEngine;
using UnityEngine.UI;
using Torus;

public class TorusDirect : TorusDirectMonoBehaviour
{
    // An example UI object to be updated before/after login
    public Text accountText;

    public override void Awake()
    {
        // Init TorusDirect
        base.Awake();

        // Register event listeners
        this.onPreLogin.AddListener(OnPreLogin);
        this.onPostLogin.AddListener(OnPostLogin);
    }

    public void LoginWithCustomConfig() {
        /* 
         * Login with custom configurations, e.g. JWT, 2nd Google project, etc
         * You will probably not need this, prebuilt functions of TorusDirectMonoBehaviour + scene configurations 
         * cover most common use cases

        this.TriggerLogin(
            typeOfLogin: TorusTypeOfLogin.JWT,
            verifier: "<verifier>",
            clientId: "<client ID>"
        );
        */
    }

    void OnPreLogin()
    {
        this.accountText.text = "Logging in...";
    }

    void OnPostLogin(TorusEvent ev)
    {
        if (ev.exception != null)
        {
            if (ev.exception is TorusUserCancelledException)
            {
                this.accountText.text = "User cancelled!";
            }
            else if (ev.exception is TorusNoAllowedBrowserFoundException)
            {
                this.accountText.text = "No allowed browser!";
            }
            else
            {
                this.accountText.text = "Something went wrong!";
            }

            Debug.Log($"Login failed: {ev.exception}");
            return;
        }

        this.accountText.text = ev.credentials.publicAddress;
        Debug.Log($"Login succeeded: {ev.credentials.publicAddress}");
    }
}
