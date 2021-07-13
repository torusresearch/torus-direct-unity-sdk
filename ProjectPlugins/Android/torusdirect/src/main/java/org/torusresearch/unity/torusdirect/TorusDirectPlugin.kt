@file:JvmName("Plugin")
@file:Suppress("Unused")

package org.torusresearch.unity.torusdirect

import android.util.Log
import com.unity3d.player.UnityPlayer
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import org.json.JSONObject
import org.torusresearch.torusdirect.TorusDirectSdk
import org.torusresearch.torusdirect.types.*
import org.torusresearch.torusdirect.utils.Helpers

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

    fun triggerLogin(
        gameObject: String,
        callback: String,
        typeOfLogin: String,
        verifier: String,
        clientId: String
    ) {
        if (args == null) throw Exception("TorusDirect.init must be called before triggerLogin.")

        val activity = UnityPlayer.currentActivity
        Log.d(
            "${tag}#triggerLogin", arrayOf(
                "gameObject=$gameObject",
                "callback=$callback",
                "typeOfLogin=$typeOfLogin",
                "verifier=$verifier",
                "clientId=$clientId",
                "activity=${activity.packageName}"
            ).joinToString(" ")
        )

        CoroutineScope(Dispatchers.Default).launch {
            try {
                val sdk = TorusDirectSdk(args, activity)
                Log.d("${tag}#triggerLogin", "Initialized TorusDirect SDK successfully")

                val result = sdk.triggerLogin(
                    SubVerifierDetails(
                        LoginType.valueOfLabel(typeOfLogin),
                        verifier,
                        clientId,
                        Auth0ClientOptions.Auth0ClientOptionsBuilder("").build(),
                        activity == null
                    )
                ).join()
                Log.d("${tag}#triggerLogin", "Logged in successfully")
                launch(Dispatchers.Main) {
                    val json = JSONObject()
                    json.put("status", "fulfilled")
                    val value = JSONObject()
                    value.put("privateKey", result.privateKey)
                    value.put("publicAddress", result.publicAddress)
                    json.put("value", value)
                    UnityPlayer.UnitySendMessage(gameObject, callback, json.toString())
                }
            } catch (completionException: Throwable) {
                val e = Helpers.unwrapCompletionException(completionException)
                Log.e("${tag}#triggerLogin", "Login failed - ${e}")
                Log.e("${tag}#triggerLogin", "Login stacktrace - ${e.stackTraceToString()}")
                launch(Dispatchers.Main) {
                    val json = JSONObject()
                    json.put("status", "rejected")
                    val reason = JSONObject()
                    reason.put("name", e::class.simpleName)
                    reason.put("message", e.message)
                    json.put("reason", reason)
                    UnityPlayer.UnitySendMessage(gameObject, callback, json.toString())
                }
            }
        }
    }
}