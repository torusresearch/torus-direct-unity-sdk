package org.torusresearch.unity.torusdirect;

import android.util.Log;

import com.unity3d.player.UnityPlayer;

import org.torusresearch.torusdirect.TorusDirectSdk;
import org.torusresearch.torusdirect.types.DirectSdkArgs;
import org.torusresearch.torusdirect.types.LoginType;
import org.torusresearch.torusdirect.types.SubVerifierDetails;
import org.torusresearch.torusdirect.types.TorusNetwork;

public class TorusDirectPlugin {
    private static final String tag = TorusDirectPlugin.class.getSimpleName();
    private static final TorusDirectPlugin instance = new TorusDirectPlugin();

    private DirectSdkArgs args;

    private TorusDirectPlugin() {
    }

    public static TorusDirectPlugin getInstance() {
        return instance;
    }

    public void init(String browserRedirectUri, String network, String redirectUri) {
        Log.d(tag + "init", "browserRedirectUri=" + browserRedirectUri
                + " redirectUri=" + redirectUri + " "
                + " network=" + network);
        args = new DirectSdkArgs(browserRedirectUri, TorusNetwork.valueOfLabel(network), redirectUri);
    }

    public void triggerLogin(String typeOfLogin, String verifier, String clientId) {
        if (args == null)
            throw new RuntimeException("TorusDirect.init must be called before triggerLogin.");
        Log.d(tag + "triggerLogin", "typeOfLogin=" + typeOfLogin
                + " verifier=" + verifier
                + " clientId=" + clientId);
        TorusDirectSdk sdk = new TorusDirectSdk(args, UnityPlayer.currentActivity);
        sdk.triggerLogin(new SubVerifierDetails(LoginType.valueOfLabel(typeOfLogin), verifier, clientId));
    }
}
