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
        callbackGameObject: String,
        callbackMethod: String,
        paramsString: String
    ) {
        if (args == null) throw Exception("TorusDirect.init must be called before triggerLogin.")

        val activity = UnityPlayer.currentActivity
        Log.d(
            "${tag}#triggerLogin", arrayOf(
                "callbackGameObject=$callbackGameObject",
                "callbackMethod=$callbackMethod",
                "params=$paramsString",
                "activity=${activity::class.qualifiedName}"
            ).joinToString(" ")
        )

        val paramsJSON = JSONObject(paramsString)
        CoroutineScope(Dispatchers.Default).launch {
            try {
                val sdk = TorusDirectSdk(args, activity)
                Log.d("${tag}#triggerLogin", "Initialized TorusDirect SDK successfully")

                val result = sdk.triggerLogin(
                    SubVerifierDetails(
                        LoginType.valueOfLabel(paramsJSON.getString("typeOfLogin")),
                        paramsJSON.getString("verifier"),
                        paramsJSON.getString("clientId"),
                        mapJwtParams(paramsJSON.getJSONObject("jwtParams")),
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
                    UnityPlayer.UnitySendMessage(
                        callbackGameObject,
                        callbackMethod,
                        json.toString()
                    )
                }
            } catch (completionException: Throwable) {
                val e = Helpers.unwrapCompletionException(completionException)
                if (e !is UserCancelledException && e !is NoAllowedBrowserFoundException) {
                    Log.e("${tag}#triggerLogin", "Login failed - ${e}")
                    Log.e("${tag}#triggerLogin", "Login stacktrace - ${e.stackTraceToString()}")
                }
                launch(Dispatchers.Main) {
                    val json = JSONObject()
                    json.put("status", "rejected")
                    val reason = JSONObject()
                    reason.put("name", e::class.simpleName)
                    reason.put("message", e.message)
                    json.put("reason", reason)
                    UnityPlayer.UnitySendMessage(
                        callbackGameObject,
                        callbackMethod,
                        json.toString()
                    )
                }
            }
        }
    }

    fun getTorusKey(
        callbackGameObject: String,
        callbackMethod: String,
        paramsString: String
    ) {
        if (args == null) throw Exception("TorusDirect.init must be called before getTorusKey.")

        val activity = UnityPlayer.currentActivity
        Log.d(
            "${tag}#getTorusKey", arrayOf(
                "callbackGameObject=$callbackGameObject",
                "callbackMethod=$callbackMethod",
                "params=$paramsString",
                "activity=${activity::class.qualifiedName}"
            ).joinToString(" ")
        )

        val paramsJSON = JSONObject(paramsString)
        CoroutineScope(Dispatchers.Default).launch {
            try {
                val sdk = TorusDirectSdk(args, activity)
                Log.d("${tag}#getTorusKey", "Initialized TorusDirect SDK successfully")

                val verifierParamsMap = hashMapOf<String, Any>()
                val verifierParamsJSON = paramsJSON.getJSONObject("verifierParams")
                for (key in verifierParamsJSON.keys())
                    verifierParamsMap[key] = verifierParamsJSON.get(key)

                val result = sdk.getTorusKey(
                    paramsJSON.getString("verifier"),
                    paramsJSON.getString("verifierId"),
                    verifierParamsMap,
                    paramsJSON.getString("idToken")
                ).join()
                Log.d("${tag}#getTorusKey", "Got Torus key successfully")
                launch(Dispatchers.Main) {
                    val json = JSONObject()
                    json.put("status", "fulfilled")
                    val value = JSONObject()
                    value.put("privateKey", result.privateKey)
                    value.put("publicAddress", result.publicAddress)
                    json.put("value", value)
                    UnityPlayer.UnitySendMessage(
                        callbackGameObject,
                        callbackMethod,
                        json.toString()
                    )
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
                    UnityPlayer.UnitySendMessage(
                        callbackGameObject,
                        callbackMethod,
                        json.toString()
                    )
                }
            }
        }
    }

    private fun mapJwtParams(jwtParams: JSONObject?): Auth0ClientOptions? {
        if (jwtParams == null || !jwtParams.has("domain")) {
            return Auth0ClientOptions.Auth0ClientOptionsBuilder("").build()
        }
        val builder = Auth0ClientOptions.Auth0ClientOptionsBuilder(jwtParams.getString("domain"))
        if (jwtParams.has("isVerifierIdCaseSensitive")) {
            builder.setVerifierIdCaseSensitive(jwtParams.getBoolean("isVerifierIdCaseSensitive"))
        }
        if (jwtParams.has("client_id")) {
            builder.setClient_id(jwtParams.getString("client_id"))
        }
        if (jwtParams.has("leeway")) {
            builder.setLeeway(jwtParams.getString("leeway"))
        }
        if (jwtParams.has("verifierIdField")) {
            builder.setVerifierIdField(jwtParams.getString("verifierIdField"))
        }
        if (jwtParams.has("display")) {
            builder.setDisplay(Display.valueOfLabel(jwtParams.getString("display")))
        }
        if (jwtParams.has("prompt")) {
            builder.setPrompt(Prompt.valueOfLabel(jwtParams.getString("prompt")))
        }
        if (jwtParams.has("max_age")) {
            builder.setMax_age(jwtParams.getString("max_age"))
        }
        if (jwtParams.has("ui_locales")) {
            builder.setUi_locales(jwtParams.getString("ui_locales"))
        }
        if (jwtParams.has("id_token_hint")) {
            builder.setId_token_hint(jwtParams.getString("id_token_hint"))
        }
        if (jwtParams.has("login_hint")) {
            builder.setLogin_hint(jwtParams.getString("login_hint"))
        }
        if (jwtParams.has("acr_values")) {
            builder.setAcr_values(jwtParams.getString("acr_values"))
        }
        if (jwtParams.has("scope")) {
            builder.setScope(jwtParams.getString("scope"))
        }
        if (jwtParams.has("audience")) {
            builder.setAudience(jwtParams.getString("audience"))
        }
        if (jwtParams.has("connection")) {
            builder.setConnection(jwtParams.getString("connection"))
        }
        if (jwtParams.has("additionalParams")) {
            val additionalParamsMap = hashMapOf<String, String>()
            val additionalParamsJSON = jwtParams.getJSONObject("additionalParams")
            for (key in additionalParamsJSON.keys())
                additionalParamsMap[key] = additionalParamsJSON.getString(key)
            builder.setAdditionalParams(additionalParamsMap)
        }
        return builder.build()
    }
}