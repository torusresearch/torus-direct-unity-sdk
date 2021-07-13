@file:JvmName("Plugin")
@file:Suppress("Unused")

package org.torusresearch.unity.torusdirect

import android.util.Log
import com.unity3d.player.UnityPlayer
import org.json.JSONObject
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

    fun triggerLogin(
        gameObject: String,
        callback: String,
        typeOfLogin: String,
        verifier: String,
        clientId: String
    ) {
        if (args == null) throw Exception("TorusDirect.init must be called before triggerLogin.")
        Log.d(
            "${tag}#triggerLogin", arrayOf(
                "gameObject=$gameObject",
                "callback=$callback",
                "typeOfLogin=$typeOfLogin",
                "verifier=$verifier",
                "clientId=$clientId",
                "activity=${UnityPlayer.currentActivity.packageName}"
            ).joinToString(" ")
        )

        val json = JSONObject()
        json.put("status", "rejected")
        val reason = JSONObject()
        reason.put("name","NotImplementedException")
        reason.put("message", "Method not implemented.")
        json.put("reason", reason)
        UnityPlayer.UnitySendMessage(gameObject, callback, json.toString())
//        CoroutineScope(Dispatchers.Default).launch {
//            try {
//                val sdk = TorusDirectSdk(args, UnityPlayer.currentActivity)
//                Log.d("${tag}#triggerLogin", "Initialized TorusDirect SDK successfully")
//
//                val result = sdk.triggerLogin(
//                    SubVerifierDetails(
//                        LoginType.valueOfLabel(typeOfLogin),
//                        verifier,
//                        clientId
//                    )
//                ).join()
//                Log.d("${tag}#triggerLogin", "Logged in successfully")
//                launch(Dispatchers.Main) {
//                    val json = JSONObject()
//                    json.put("status", "fulfilled")
//                    val value = JSONObject()
//                    value.put("privateKey", result.privateKey)
//                    value.put("publicAddress", result.publicAddress)
//                    json.put("value", value)
//                    UnityPlayer.UnitySendMessage(gameObject, callback, json.toString())
//                }
//            } catch (e: Throwable) {
//                Log.d("${tag}#triggerLogin", "Failed to login")
//                launch(Dispatchers.Main) {
//                    val json = JSONObject()
//                    json.put("status", "rejected")
//                    json.put("reason", e.message)
//                    UnityPlayer.UnitySendMessage(gameObject, callback, json.toString())
//                }
//            }
//        }
    }
}