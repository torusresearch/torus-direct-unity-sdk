@file:JvmName("Plugin")
@file:Suppress("Unused")

package org.torusresearch.unity.torusdirect

import android.net.Uri
import android.util.Log
import androidx.browser.customtabs.CustomTabsIntent
import com.unity3d.player.UnityPlayer
import org.torusresearch.torusdirect.TorusDirectSdk
import org.torusresearch.torusdirect.types.DirectSdkArgs
import org.torusresearch.torusdirect.types.TorusNetwork

val instance = TorusDirectPlugin()

class TorusDirectPlugin internal constructor() {
    companion object {
        private val tag = TorusDirectPlugin::class.simpleName
    }

    private var args: DirectSdkArgs? = null

    fun init(browserRedirectUri: String, network: String, redirectUri: String) {
        Log.d(
            "${tag}#init",
            arrayOf(
                "browserRedirectUri=$browserRedirectUri",
                "redirectUri=$redirectUri",
                "network=$network"
            ).joinToString(" ")
        )
        args = DirectSdkArgs(browserRedirectUri, TorusNetwork.valueOfLabel(network), redirectUri)
    }

    fun triggerLogin(typeOfLogin: String, verifier: String, clientId: String) {
        if (args == null) throw Exception("TorusDirect.init must be called before triggerLogin.")
        Log.d(
            "${tag}#triggerLogin", arrayOf(
                "typeOfLogin=$typeOfLogin",
                "verifier=$verifier",
                "clientId=$clientId",
                "activity=${UnityPlayer.currentActivity.packageName}"
            ).joinToString(" ")
        )

        @Suppress("UNUSED_VARIABLE") val sdk = TorusDirectSdk(args, UnityPlayer.currentActivity)
        Log.d(tag + "triggerLogin", "Initialized TorusDirect SDK successfully")

        val customTabsBuilder = CustomTabsIntent.Builder()
        val customTabsIntent = customTabsBuilder.build()
        customTabsIntent.launchUrl(UnityPlayer.currentActivity, Uri.parse("https://customauth.io"))
    }
}