@file:JvmName("Plugin")
@file:Suppress("Unused")

package org.torusresearch.unity.torusdirect

import android.util.Log
import com.unity3d.player.UnityPlayer
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import org.torusresearch.torusdirect.TorusDirectSdk
import org.torusresearch.torusdirect.types.DirectSdkArgs
import org.torusresearch.torusdirect.types.LoginType
import org.torusresearch.torusdirect.types.SubVerifierDetails
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

        CoroutineScope(Dispatchers.Default).launch {
            val sdk = TorusDirectSdk(args, UnityPlayer.currentActivity)
            launch(Dispatchers.Main) {
                Log.d("${tag}#triggerLogin", "Initialized TorusDirect SDK successfully")
            }

            sdk.triggerLogin(
                SubVerifierDetails(
                    LoginType.valueOfLabel(typeOfLogin),
                    verifier,
                    clientId
                )
            ).join()
            launch(Dispatchers.Main) {
                Log.d("${tag}#triggerLogin", "Logged in successfully")
            }
        }
    }
}