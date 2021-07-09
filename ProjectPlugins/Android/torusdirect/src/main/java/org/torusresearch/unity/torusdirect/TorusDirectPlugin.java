package org.torusresearch.unity.torusdirect;

import android.content.Context;
import android.util.Log;

import org.torusresearch.torusdirect.TorusDirectSdk;
import org.torusresearch.torusdirect.types.DirectSdkArgs;
import org.torusresearch.torusdirect.types.TorusNetwork;

public class TorusDirectPlugin {
    private static final String tag = TorusDirectPlugin.class.getSimpleName();
    private static final TorusDirectPlugin instance = new TorusDirectPlugin();

    private TorusDirectSdk sdk;

    private TorusDirectPlugin() {
    }

    public static TorusDirectPlugin getInstance() {
        return instance;
    }

    public void init(String browserRedirectUri, String network, String redirectUri, Context context) {
        Log.d(tag + "init", "browserRedirectUri=" + browserRedirectUri
                + " redirectUri=" + redirectUri + " "
                + " network=" + network);
        sdk = new TorusDirectSdk(new DirectSdkArgs(browserRedirectUri, TorusNetwork.valueOfLabel(network), redirectUri), context);
    }
}
